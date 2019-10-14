using System;
using System.Collections.Generic;
using UnityEngine;

namespace KLWStateMachine
{
    [AddComponentMenu("")]
    public class StateMachineRunner : MonoBehaviour
    {
        private readonly List<IStateMachine> _stateMachineList = new List<IStateMachine>();
        private bool _userMonoBehaviourUpdate = true;

        public bool UserMonoBehaviourUpdate
        {
            get { return _userMonoBehaviourUpdate; }
            set { _userMonoBehaviourUpdate = value; }
        }

        public StateMachine<T> Initialize<T>(MonoBehaviour component) where T : struct, IConvertible, IComparable
        {
            var fsm = new StateMachine<T>(this, component);
            _stateMachineList.Add(fsm);
            return fsm;
        }

        public StateMachine<T> Initialize<T>(MonoBehaviour component, T startState)
            where T : struct, IConvertible, IComparable
        {
            var fsm = Initialize<T>(component);
            fsm.ChangeState(startState);
            return fsm;
        }

        public void OnUpdate()
        {
            for (var i = 0; i < _stateMachineList.Count; i++)
            {
                var fsm = _stateMachineList[i];
                if (fsm.Component.enabled)
                {
                    fsm.CurrentStateMap.Update();
                }
            }
        }

        private void Update()
        {
            if (UserMonoBehaviourUpdate)
            {
                OnUpdate();
            }
        }
    }
}