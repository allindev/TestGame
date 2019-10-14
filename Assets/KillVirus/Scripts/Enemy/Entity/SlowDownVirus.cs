using System;
using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class SlowDownVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;
        [SerializeField] private Transform _rotateTransform;
        [SerializeField] private float _slowDownRadius;

        private VirusForceLineEffect _forceLine;

        private void Update()
        {
            RunAway();
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
            _rotateTransform.localEulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
            bool b1 = VirusGameMrg.Instance.VirusPlayer != null && VirusGameMrg.Instance.VirusPlayer.gameObject.activeSelf;
            if (b1)
            {
                Transform slowTransform = VirusGameMrg.Instance.VirusPlayer.SlowTransform;
                Vector3 offset = transform.position - slowTransform.position - new Vector3(0, 0.85f, 0);
                bool b = offset.sqrMagnitude < _slowDownRadius * _slowDownRadius;
                if (b)
                {
                    if (_forceLine == null)
                    {
                        var line = EffectPools.Instance.Spawn("ForceLine");
                        _forceLine = line.GetComponent<VirusForceLineEffect>();
                        _forceLine.UpdateLine(transform, slowTransform);
                    }
                    else
                    {
                        _forceLine.UpdateLine(transform, slowTransform);
                    }
                    VirusGameMrg.Instance.VirusPlayer.AddDecelerate(this);
                }
                else
                {
                    if (_forceLine != null)
                    {
                        Pools.Despawn(_forceLine.gameObject);
                        _forceLine = null;
                    }
                    VirusGameMrg.Instance.VirusPlayer.RemoveDecelerate(this);
                }
            }
            else
            {
                if (_forceLine != null)
                {
                    Pools.Despawn(_forceLine.gameObject);
                    _forceLine = null;
                }
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("SlowDownVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            if (_forceLine != null)
            {
                Pools.Despawn(_forceLine.gameObject);
                _forceLine = null;
            }
            VirusGameMrg.Instance.VirusPlayer.RemoveDecelerate(this);
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
            var virusLevel = VirusTool.GetVirusColorLevel("SlowDownVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            float vv= base.StartCure(iCure,healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("SlowDownVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
