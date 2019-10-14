using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KLWAnimatorStateMachineUtil
{
    public class AnimatorStateMachineUtil : MonoBehaviour
    {
        protected Animator _animator;
        protected int _lastState;
        public bool AutoUpdate = true;
        protected Dictionary<int, string> HashToAnimString;
        protected Lookup<int, Action> StateHashToEnterMethod;
        protected Lookup<int, Action<Animator>> StateHashToEnterParameterMethod;
        protected Lookup<int, Action> StateHashToExitMethod;
        protected Lookup<int, Action<Animator>> StateHashToExitParameterMethod;
        protected Lookup<int, Action> StateHashToUpdateMethod;
        protected Lookup<int, Action<Animator>> StateHashToUpdateParameterMethod;

        public Animator Animator
        {
            get { return _animator; }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            DiscoverStateMethods();
        }

        private void Update()
        {
            if (AutoUpdate)
                StateMachineUpdate();
        }

        private void OnValidate()
        {
            DiscoverStateMethods();
        }

        public void StateMachineUpdate()
        {
            var stateId = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            if (_lastState != stateId)
            {
                if (StateHashToExitMethod.Contains(_lastState))
                    foreach (var action in StateHashToExitMethod[_lastState])
                        action.Invoke();

                if (StateHashToExitParameterMethod.Contains(_lastState))
                    foreach (var action in StateHashToExitParameterMethod[_lastState])
                        action.Invoke(_animator);

                if (StateHashToEnterMethod.Contains(stateId))
                    foreach (var action in StateHashToEnterMethod[stateId])
                        action.Invoke();

                if (StateHashToEnterParameterMethod.Contains(stateId))
                    foreach (var action in StateHashToEnterParameterMethod[stateId])
                        action.Invoke(_animator);
            }

            if (StateHashToUpdateMethod.Contains(stateId))
                foreach (var action in StateHashToUpdateMethod[stateId])
                    action.Invoke();
            if (StateHashToUpdateParameterMethod.Contains(stateId))
                foreach (var action in StateHashToUpdateParameterMethod[stateId])
                    action.Invoke(_animator);
            _lastState = stateId;
        }

        private void DiscoverStateMethods()
        {
            HashToAnimString = new Dictionary<int, string>();
            var components = gameObject.GetComponents<MonoBehaviour>();

            var enterStateMethods = new List<StateMethod>();
            var updateStateMethods = new List<StateMethod>();
            var exitStateMethods = new List<StateMethod>();

            var enterStateMethodHasParameters = new List<StateMethodHasParameter>();
            var updateStateMethodHasParameters = new List<StateMethodHasParameter>();
            var exitStateMethodHasParameters = new List<StateMethodHasParameter>();


            foreach (var component in components)
            {
                if (component == null) continue;

                var type = component.GetType();
                var methods =
                    type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly |
                                    BindingFlags.InvokeMethod);

                foreach (var method in methods)
                {
                    object[] attributes;

                    attributes = method.GetCustomAttributes(typeof(StateUpdateMethod), true);
                    foreach (StateUpdateMethod attribute in attributes)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 0)
                            updateStateMethods.Add(CreateStateMethod(attribute.State, method, component));
                        else if ((parameters.Length == 1) && (parameters[0].ParameterType == typeof(Animator)))
                            updateStateMethodHasParameters.Add(CreateStateMethodHasParameter(attribute.State, method,
                                component));
                    }


                    attributes = method.GetCustomAttributes(typeof(StateEnterMethod), true);
                    foreach (StateEnterMethod attribute in attributes)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 0)
                            enterStateMethods.Add(CreateStateMethod(attribute.State, method, component));
                        else if ((parameters.Length == 1) && (parameters[0].ParameterType == typeof(Animator)))
                            enterStateMethodHasParameters.Add(CreateStateMethodHasParameter(attribute.State, method,
                                component));
                    }

                    attributes = method.GetCustomAttributes(typeof(StateExitMethod), true);
                    foreach (StateExitMethod attribute in attributes)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 0)
                            exitStateMethods.Add(CreateStateMethod(attribute.State, method, component));
                        else if ((parameters.Length == 1) && (parameters[0].ParameterType == typeof(Animator)))
                            exitStateMethodHasParameters.Add(CreateStateMethodHasParameter(attribute.State, method,
                                component));
                    }
                }
            }


            StateHashToUpdateMethod = (Lookup<int, Action>) updateStateMethods.ToLookup(p => p.StateHash, p => p.Method);
            StateHashToEnterMethod = (Lookup<int, Action>) enterStateMethods.ToLookup(p => p.StateHash, p => p.Method);
            StateHashToExitMethod = (Lookup<int, Action>) exitStateMethods.ToLookup(p => p.StateHash, p => p.Method);

            StateHashToUpdateParameterMethod =
                (Lookup<int, Action<Animator>>) updateStateMethodHasParameters.ToLookup(p => p.StateHash, p => p.Method);
            StateHashToEnterParameterMethod =
                (Lookup<int, Action<Animator>>) enterStateMethodHasParameters.ToLookup(p => p.StateHash, p => p.Method);
            StateHashToExitParameterMethod =
                (Lookup<int, Action<Animator>>) exitStateMethodHasParameters.ToLookup(p => p.StateHash, p => p.Method);
        }

        private StateMethod CreateStateMethod(string state, MethodInfo method, MonoBehaviour component)
        {
            var stateHash = Animator.StringToHash(state);
            HashToAnimString[stateHash] = state;
            var stateMethod = new StateMethod();
            stateMethod.StateHash = stateHash;
            stateMethod.Method = () => { method.Invoke(component, null); };
            return stateMethod;
        }

        private StateMethodHasParameter CreateStateMethodHasParameter(string state, MethodInfo method,
            MonoBehaviour component)
        {
            var stateHash = Animator.StringToHash(state);
            HashToAnimString[stateHash] = state;
            var stateMethod = new StateMethodHasParameter();
            stateMethod.StateHash = stateHash;
            stateMethod.Method = animator => { method.Invoke(component, new object[] {animator}); };
            return stateMethod;
        }
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StateUpdateMethod : Attribute
    {
        public string State;

        public StateUpdateMethod(string state)
        {
            State = state;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StateEnterMethod : Attribute
    {
        public string State;

        public StateEnterMethod(string state)
        {
            State = state;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StateExitMethod : Attribute
    {
        public string State;

        public StateExitMethod(string state)
        {
            State = state;
        }
    }

    public class StateMethod
    {
        public Action Method;
        public int StateHash;
    }

    public class StateMethodHasParameter
    {
        public Action<Animator> Method;
        public int StateHash;
    }
}