using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class CollisionVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;
        [SerializeField] private Transform _rotateTransform;

        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        private float _rotateDuration;
        private float _totalTime;
        private float _totalAngle;
        private bool _isRotate;

       

        private void Update()
        {
            if (!_isRotate)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime >= _rotateDuration)
                {
                    _totalTime -= _rotateDuration;
                    _isRotate = true;
                    _totalAngle = 60;
                }
            }
            else
            {
                RunAway();
            }
        }


        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _isRotate = false;
            _totalTime = 0;
            _rotateDuration = 3;
            _totalAngle = 60;
        }


        protected override VirusMove VirusMove
        {
            get { return _virusMove; }
        }


        protected override void RunAway()
        {
            float dleta = Time.deltaTime * 100;
            if (_totalAngle - dleta <= 0)
            {
                _isRotate = false;
                _rotateTransform.localEulerAngles += new Vector3(0, 0, _totalAngle);
                return;
            }
            _totalAngle -= dleta;
            _rotateTransform.localEulerAngles += new Vector3(0, 0, dleta);
        }


        public override void Death(bool isEffect, bool isDivide , bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("CollisionVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            Pools.Despawn(gameObject);
        }


        public override void Injured(float damageValue,bool isEffect)
        {
            if (VirusHealth.Value - damageValue <= 0)
            {
                Death(true, true, true, true);
                return;
            }
            VirusHealth.Value -= damageValue;
            var virusLevel = VirusTool.GetVirusColorLevel("CollisionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                virusHurtEffect.StartHurtEffect();
        }


        public override float StartCure(ICure icure, float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(icure);
            }
            float vv = base.StartCure(icure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("CollisionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            return vv;
        }


        public override void StopCure(ICure icure)
        {
            _virusHealthAddEffect.StopHealthEffect(icure);
        }


    }
}
