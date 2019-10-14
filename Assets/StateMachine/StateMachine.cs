using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KLWStateMachine
{
    public interface IStateMachine
    {
        MonoBehaviour Component { get; }
        StateMapping CurrentStateMap { get; }
    }

    public class StateMapping
    {
        public Action EnterCall = DoNothing;
        public Func<IEnumerator> EnterRoutine = DoNothingCoroutine;
        public Action ExitCall = DoNothing;
        public Func<IEnumerator> ExitRoutine = DoNothingCoroutine;
        public Action Finally = DoNothing;
        public bool HasEnterRoutine;
        public bool HasExitRoutine;
        public object State;
        public Action Update = DoNothing;

        public StateMapping(object state)
        {
            State = state;
        }

        public static void DoNothing()
        {
        }

        public static IEnumerator DoNothingCoroutine()
        {
            yield break;
        }
    }

    public class StateMachine<T> : IStateMachine where T : struct, IConvertible, IComparable
    {
        private readonly StateMachineRunner _engine;
        private readonly Dictionary<object, StateMapping> _stateLookup;
        private IEnumerator _currentTransition;
        private StateMapping _destinationState;
        private IEnumerator _enterRoutine;

        private IEnumerator _exitRoutine;
        private bool _isInTransition;
        private StateMapping _lastState;
        private IEnumerator _queuedChange;

        public StateMachine(StateMachineRunner engine, MonoBehaviour component)
        {
            _engine = engine;
            Component = component;

            var values = Enum.GetValues(typeof(T));
            if (values.Length < 1)
            {
                throw new ArgumentException("Enum provided to Initialize must have at least 1 visible definition");
            }

            _stateLookup = new Dictionary<object, StateMapping>();
            for (var i = 0; i < values.Length; i++)
            {
                var mapping = new StateMapping((Enum) values.GetValue(i));
                _stateLookup.Add(mapping.State, mapping);
            }


            var classMethods =
                component.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public |
                                BindingFlags.NonPublic);


            var baseMethods =
                component.GetType()
                    .BaseType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public |
                                         BindingFlags.NonPublic);


            List<MethodInfo> methodArray = new List<MethodInfo>();
            methodArray.AddRange(classMethods);
            methodArray.AddRange(baseMethods);


            for (var i = 0; i < methodArray.Count; i++)
            {
                var attributes = (FSMAttribute[]) methodArray[i].GetCustomAttributes(typeof(FSMAttribute), false);
                if (attributes.Length == 1)
                {
                    var attribute = attributes[0];
                    Enum key;
                    try
                    {
                        key = (Enum) Enum.Parse(typeof(T), attribute.EnumValue);
                    }
                    catch (ArgumentException)
                    {
                        //Not an method as listed in the state enum
                        continue;
                    }
                    var targetState = _stateLookup[key];
                    switch (attribute.ActionName)
                    {
                        case FSMActionName.Enter:
                            if (methodArray[i].ReturnType == typeof(IEnumerator))
                            {
                                targetState.HasEnterRoutine = true;
                                targetState.EnterRoutine = CreateDelegate<Func<IEnumerator>>(methodArray[i], component);
                            }
                            else
                            {
                                targetState.HasEnterRoutine = false;
                                targetState.EnterCall = CreateDelegate<Action>(methodArray[i], component);
                            }
                            break;
                        case FSMActionName.Exit:
                            if (methodArray[i].ReturnType == typeof(IEnumerator))
                            {
                                targetState.HasExitRoutine = true;
                                targetState.ExitRoutine = CreateDelegate<Func<IEnumerator>>(methodArray[i], component);
                            }
                            else
                            {
                                targetState.HasExitRoutine = false;
                                targetState.ExitCall = CreateDelegate<Action>(methodArray[i], component);
                            }
                            break;
                        case FSMActionName.Finally:
                            targetState.Finally = CreateDelegate<Action>(methodArray[i], component);
                            break;
                        case FSMActionName.Update:
                            targetState.Update = CreateDelegate<Action>(methodArray[i], component);
                            break;
                    }
                }
            }
            CurrentStateMap = new StateMapping(null);
        }

        public T LastState
        {
            get
            {
                if (_lastState == null) return default(T);
                return (T) _lastState.State;
            }
        }

        public T State
        {
            get { return (T) CurrentStateMap.State; }
        }

        public StateMapping CurrentStateMap { get; private set; }
        public MonoBehaviour Component { get; private set; }

        public event Action<T> Changed;


        private Type CreateDelegate<Type>(MethodInfo method, Object target) where Type : class
        {
            var ret = Delegate.CreateDelegate(typeof(Type), target, method) as Type;
            if (ret == null)
            {
                throw new ArgumentException("Unabled to create delegate for method called " + method.Name);
            }
            return ret;
        }

        public void ChangeState(T newState)
        {
            if (_stateLookup == null)
            {
                throw new Exception(
                    "States have not been configured, please call initialized before trying to set state");
            }
            if (!_stateLookup.ContainsKey(newState))
            {
                throw new Exception("No state with the name " + newState +
                                    " can be found. Please make sure you are called the correct type the statemachine was initialized with");
            }

            var nextState = _stateLookup[newState];
            if (CurrentStateMap == nextState) return;

            //Cancel any queued changes.
            if (_queuedChange != null)
            {
                _engine.StopCoroutine(_queuedChange);
                _queuedChange = null;
            }

            if (_isInTransition)
            {
                if (_exitRoutine != null) //We are already exiting current state on our way to our previous target state
                {
                    //Overwrite with our new target
                    _destinationState = nextState;
                    return;
                }
                if (_enterRoutine != null)
                    //We are already entering our previous target state. Need to wait for that to finish and call the exit routine.
                {
                    //Damn, I need to test this hard
                    _queuedChange = WaitForPreviousTransition(nextState);
                    _engine.StartCoroutine(_queuedChange);
                    return;
                }
            }

            if ((CurrentStateMap != null && CurrentStateMap.HasExitRoutine) || nextState.HasEnterRoutine)
            {
                _isInTransition = true;
                _currentTransition = ChangeToNewStateRoutine(nextState);
                _engine.StartCoroutine(_currentTransition);
            }
            else
            {
                if (CurrentStateMap != null)
                {
                    CurrentStateMap.ExitCall();
                    CurrentStateMap.Finally();
                }

                _lastState = CurrentStateMap;
                CurrentStateMap = nextState;
                if (CurrentStateMap != null)
                {
                    CurrentStateMap.EnterCall();
                    if (Changed != null)
                    {
                        Changed((T) CurrentStateMap.State);
                    }
                }
                _isInTransition = false;
            }
        }

        private IEnumerator ChangeToNewStateRoutine(StateMapping newState)
        {
            _destinationState = newState; //Chache this so that we can overwrite it and hijack a transition

            if (CurrentStateMap != null)
            {
                if (CurrentStateMap.HasExitRoutine)
                {
                    _exitRoutine = CurrentStateMap.ExitRoutine();

                    if (_exitRoutine != null) //Don't wait for exit if we are overwriting
                    {
                        yield return _engine.StartCoroutine(_exitRoutine);
                    }

                    _exitRoutine = null;
                }
                else
                {
                    CurrentStateMap.ExitCall();
                }

                CurrentStateMap.Finally();
            }

            _lastState = CurrentStateMap;
            CurrentStateMap = _destinationState;

            if (CurrentStateMap != null)
            {
                if (CurrentStateMap.HasEnterRoutine)
                {
                    _enterRoutine = CurrentStateMap.EnterRoutine();

                    if (_enterRoutine != null)
                    {
                        yield return _engine.StartCoroutine(_enterRoutine);
                    }

                    _enterRoutine = null;
                }
                else
                {
                    CurrentStateMap.EnterCall();
                }

                //Broadcast change only after enter transition has begun. 
                if (Changed != null)
                {
                    Changed((T) CurrentStateMap.State);
                }
            }

            _isInTransition = false;
        }

        private IEnumerator WaitForPreviousTransition(StateMapping nextState)
        {
            while (_isInTransition)
            {
                yield return null;
            }

            ChangeState((T) nextState.State);
        }

        public static StateMachine<T> Initialize(MonoBehaviour component)
        {
            var engine = component.GetComponent<StateMachineRunner>() ??
                         component.gameObject.AddComponent<StateMachineRunner>();
            return engine.Initialize<T>(component);
        }


        public static StateMachine<T> Initialize(MonoBehaviour component, T startState)
        {
            var engine = component.GetComponent<StateMachineRunner>() ??
                         component.gameObject.AddComponent<StateMachineRunner>();
            return engine.Initialize(component, startState);
        }
    }
}