using System.Collections.Generic;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class ExplosiveVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private List<VirusStretchAction> _stretchTransforms;

        private bool _isStretch;
        private bool _isDeath;


        private void OnEnable()
        {
            _isStretch = true;
            _isDeath = false;
        }

        private void Update()
        {
            if (_isStretch)
            {
                RunAway();
            }
        }

        private void DelayDeath()
        {
            _virusMove.IsUpdate = false;
            _isStretch = false;
            _isDeath = true;
            float scale = transform.localScale.x;
            float scaleX;
            DOVirtual.Float(0, 100f, 2f, t =>
            {
                int t1 = (int)t;
                if (t1 <= 50)
                {
                    scaleX = t1 / 500f;
                    float x = scale + scaleX;
                    Vector3 s1 = t1 % 5 == 0 ? new Vector3(x, x, 1) : new Vector3(x - 0.03f, x - 0.03f, 1);
                    transform.localScale = s1;
                }
                else
                {
                    scaleX = t1 / 500f;
                    float x = scale + scaleX;
                    Vector3 s1 = t1 % 2 == 0 ? new Vector3(x, x, 1) : new Vector3(x - 0.03f, x - 0.03f, 1);
                    transform.localScale = s1;
                }

            }).OnComplete(() =>
            {
                var obj = EffectPools.Instance.Spawn("ExplosiveEffect");
                obj.transform.position = transform.position;
                Pools.Despawn(gameObject);
            });
        }


        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return _virusMove; }
        }

        protected override void RunAway()
        {
            for (int i = 0; i < _stretchTransforms.Count; i++)
            {
                var item = _stretchTransforms[i];
                item.OnUpdate(Time.deltaTime);
            }
        }

        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (SplitLevel > SplitLevel.Level1)
            {
                if (isDivide)
                    Divide("ExplosiveVirus", OriginColorLevel, 2);
                base.Death(isEffect, isDivide, isCoin, isProp);
                Pools.Despawn(gameObject);
            }
            else
            {
                base.Death(false, false, false, false);
                DelayDeath();
            }
        }


        public override void Injured(float damageValue,bool isEffect)
        {
            if (!_isDeath)
            {
                if (VirusHealth.Value - damageValue <= 0)
                {
                    Death(true, true, true, true);
                    return;
                }
                VirusHealth.Value -= damageValue;
                var virusLevel = VirusTool.GetVirusColorLevel("ExplosiveVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
                if (virusLevel != CurColorLevel)
                {
                    CurColorLevel = virusLevel;
                    VirusSprite.Initi(CurColorLevel);
                    transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
                }
                if (isEffect)
                    _virusHurtEffect.StartHurtEffect();
            }
        }

        public override float StartCure(ICure iCure, float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(iCure);
            }
            float vv = base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("ExplosiveVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
