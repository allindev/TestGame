using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EnemyBuffs
{
    public class WeakenVirusBuff : BaseVirusBuff
    {

        private float _curWeakenSpeed;
        private VirusMove _virusMove;
        private VirusBuffMrg _buffMrg;
        private bool _isActive;
        public WeakenVirusBuff(Transform target)
            : base(target)
        {
            _curWeakenSpeed = 1;
            VirusPropEnum = VirusPropEnum.Weaken;
            _virusMove = target.GetComponent<VirusMove>();
            _buffMrg = target.GetComponent<VirusBuffMrg>();
            _isActive = true;
        }

        public override void OnUpdate()
        {
            if (_isActive)
            {
                _virusMove.Speed /= _curWeakenSpeed;
                _curWeakenSpeed -= Time.deltaTime;
                if (_curWeakenSpeed < 0.5f)
                    _curWeakenSpeed = 0.5f;
                _virusMove.Speed *= _curWeakenSpeed;
            }
            else
            {
                _virusMove.Speed /= _curWeakenSpeed;
                _curWeakenSpeed += Time.deltaTime;
                if (_curWeakenSpeed >= 1)
                {
                    _curWeakenSpeed = 1;
                    _buffMrg.RemoveBuff(VirusPropEnum.Weaken);
                }
                _virusMove.Speed *= _curWeakenSpeed;
            }
        }

        public override void Stop()
        {
            _isActive = false;
        }
    }
}
