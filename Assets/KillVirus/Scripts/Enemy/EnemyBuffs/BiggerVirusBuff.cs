using UnityEngine;

namespace EnemyBuffs
{
    public class BiggerVirusBuff : BaseVirusBuff
    {

        private float _curBiggerSpeed;
        private VirusBuffMrg _buffMrg;
        private bool _isActive;

        public BiggerVirusBuff(Transform target)
            : base(target)
        {

            VirusPropEnum = VirusPropEnum.Big;
            Target = target;
            _buffMrg = target.GetComponent<VirusBuffMrg>();
            _curBiggerSpeed = 1;
            _isActive = true;
        }

        public override void OnUpdate()
        {
            if (_isActive)
            {
                Vector3 v = new Vector3(Target.localScale.x / _curBiggerSpeed, Target.localScale.y / _curBiggerSpeed, 1);
                Target.localScale = v;
                _curBiggerSpeed += Time.deltaTime * 2;
                if (_curBiggerSpeed >= 2)
                    _curBiggerSpeed = 2;
                v = new Vector3(Target.localScale.x * _curBiggerSpeed, Target.localScale.y * _curBiggerSpeed, 1);
                Target.localScale = v;
            }
            else
            {
                Vector3 v1 = new Vector3(Target.localScale.x / _curBiggerSpeed, Target.localScale.y / _curBiggerSpeed, 1);
                Target.localScale = v1;
                _curBiggerSpeed -= Time.deltaTime * 2;
                if (_curBiggerSpeed <= 1)
                {
                    _curBiggerSpeed = 1;
                    _buffMrg.RemoveBuff(VirusPropEnum.Big);
                }
                v1 = new Vector3(Target.localScale.x * _curBiggerSpeed, Target.localScale.y * _curBiggerSpeed, 1);
                Target.localScale = v1;
            }
        }

        public override void Stop()
        {
            _isActive = false;
        }
      
    }
}
