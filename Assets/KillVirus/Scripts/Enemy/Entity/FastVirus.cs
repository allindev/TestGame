using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class FastVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private Transform _virus1;
        [SerializeField] private Transform _virus2;

        private float _duration;
        private float _totalTime;
        private float _speedTime;
        private float _rotateSpeed;
        private float _speedMul;

        private bool _isSpeed;
        

        private void Update()
        {
            RunAway();
            SpeedUp();
        }

        private void SpeedUp()
        {
            if (_isSpeed)
            {
                _speedTime += Time.deltaTime;
                _rotateSpeed += Time.deltaTime * 700f;
                _speedMul += Time.deltaTime;
                _virusMove.SpeedUp(_speedMul);
                if (_speedTime > 1.5f)
                {
                    _isSpeed = false;
                    _speedTime = 0;
                }
                if (_rotateSpeed >= 1000f)
                {
                    _rotateSpeed = 1000f;
                }
                if (_speedMul >= 2f)
                {
                    _speedMul = 2f;
                }
            }
        }


        protected override VirusHealthBar HealthBar
        {
            get { return _virusHealthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return _virusMove; }
        }



        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _duration = Random.Range(6f, 10f);
            _rotateSpeed = 300f;
            _isSpeed = false;
            _speedMul = 1;
        }


        protected override void RunAway()
        {
            _virus1.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * _rotateSpeed);
            _virus2.transform.localEulerAngles += new Vector3(0, 0, -Time.deltaTime * _rotateSpeed);
            if (!_isSpeed)
            {
                _totalTime += Time.deltaTime;
                _rotateSpeed -= Time.deltaTime * 700f;
                _speedMul -= Time.deltaTime;
                if (_totalTime > _duration)
                {
                    _isSpeed = true;
                    _totalTime = 0;
                    _duration = Random.Range(6f, 10f);
                    return;
                }
                if (_rotateSpeed <= 300f)
                {
                    _rotateSpeed = 300f;
                }
                if (_speedMul <= 1f)
                {
                    _speedMul = 1;
                }
                _virusMove.SpeedUp(_speedMul);
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("FastVirus", OriginColorLevel, 2);
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
            var virusLevel = VirusTool.GetVirusColorLevel("FastVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                _virusHurtEffect.StartHurtEffect();
        }


        public override float StartCure(ICure iCure,float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(iCure);
            }
            float vv= base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("FastVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            return vv;
        }


        public override void StopCure(ICure iCure)
        {
            _virusHealthAddEffect.StopHealthEffect(iCure);
        }


    }

      
}
