using UnityEngine;

namespace EnemyBuffs
{
    public class ActiveVirusBuff : BaseVirusBuff
    {

        private float _curActiveSpeed;
        private VirusMove _virusMove;
        private VirusBuffMrg _buffMrg;
        private bool _isActive;
        public ActiveVirusBuff(Transform target) : base(target)
        {
            VirusPropEnum = VirusPropEnum.Active;
            _virusMove = target.GetComponent<VirusMove>();
            _buffMrg = target.GetComponent<VirusBuffMrg>();
            _curActiveSpeed = 1;
            _isActive = true;
        }

        public override void OnUpdate()
        {
            if (_isActive)
            {
                _virusMove.Speed /= _curActiveSpeed;
                _curActiveSpeed += Time.deltaTime * 2;
                if (_curActiveSpeed >= 2)
                    _curActiveSpeed = 2;
                _virusMove.Speed *= _curActiveSpeed;
            }
            else
            {
                _virusMove.Speed /= _curActiveSpeed;
                _curActiveSpeed -= Time.deltaTime * 2;
                if (_curActiveSpeed <= 1)
                {
                    _curActiveSpeed = 1;
                    _buffMrg.RemoveBuff(VirusPropEnum.Active);
                }
                _virusMove.Speed *= _curActiveSpeed;
            }
        }

        public override void Stop()
        {
            _isActive = false;
        }

    }
}
