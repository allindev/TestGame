using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class DartVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private Transform _rotateTransform;


        private float _rotateSpeed;
        private float _duration;
        private float _runAwayDuration;
        private float _totalTime;
        private bool _isRunAway;
        private bool _isSpawn;

        private void Update()
        {
            _rotateTransform.localEulerAngles += new Vector3(0, 0, _rotateSpeed * Time.deltaTime);
            if (!_isRunAway)
            {
                _rotateSpeed -= Time.deltaTime * 500f;
                if (_rotateSpeed <= 100)
                {
                    _rotateSpeed = 100;
                }
                _totalTime += Time.deltaTime;
                if (_totalTime > _duration)
                {
                    _isRunAway = true;
                    _totalTime = 0;
                    _isSpawn = true;

                    _rotateTransform.DOScale(new Vector3(0.8f, 0.8f, 1), 1.8f);
                }
            }
            else
            {
                _rotateSpeed += Time.deltaTime * 500f;
                if (_rotateSpeed >= 1000f)
                {
                    if (_isSpawn)
                    {
                        RunAway();
                        _isSpawn = false;
                        _rotateTransform.DOScale(new Vector3(1, 1, 1), 1.8f);
                    }
                    _totalTime += Time.deltaTime;
                    if (_totalTime >= _runAwayDuration)
                    {
                        _isRunAway = false;
                    }
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

        protected override void RunAway()
        {
            var move = transform.GetComponent<VirusMove>();
            for (int i = 0; i < 3; i++)
            {
                int level = (int)(SplitLevel - 1);
                SplitLevel splitLevel = level < 0 ? SplitLevel.Level1 : (SplitLevel)level;
                float angle = Random.Range(i * 120, (i + 1) * 120);
                VirusData data = new VirusData();
                data.VirusColorLevel = VirusTool.GetColorLevel(CurColorLevel);
                data.SplitLevel = splitLevel;
                data.MoveSpeed = move.OriginSpeed;
                data.MoveDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                int t = VirusGameDataAdapter.GetLevel();
                data.HealthValue = VirusTool.GetVirusHealthByColorLevel("DartVirus", t, data.VirusColorLevel);
                VirusMrg.Instance.SpawnVirus("DartVirus", data, transform.position, true);
            }
        }


        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _rotateSpeed = 100;
            _totalTime = 0;
            _isRunAway = false;

            _duration = Random.Range(4f, 10f);
            _runAwayDuration = Random.Range(1f, 3f);
            _rotateTransform.localScale = Vector3.one;
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("DartVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            Pools.Despawn(gameObject);
        }


        public override void Injured(float damageValue, bool isEffect)
        {
            if (VirusHealth.Value - damageValue <= 0)
            {
                Death(true, true, true, true);
                return;
            }
            VirusHealth.Value -= damageValue;
            var virusLevel = VirusTool.GetVirusColorLevel("DartVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                virusHurtEffect.StartHurtEffect();
        }


        public override float StartCure(ICure iCure, float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(iCure);
            }
            float vv = base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("DartVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
