using System;
using System.Runtime.InteropServices;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;


namespace Enemy.Entity
{
    public class ExpansionVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private Transform rotateTransform;


        private float _runAwayDuration;
        private float _totalTime;
        private float _scale;

        private void Update()
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _runAwayDuration)
            {
                _totalTime -= _runAwayDuration;
                RunAway();
            }
            rotateTransform.localEulerAngles += new Vector3(0, 0, 200 * Time.deltaTime);
        }

        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return virusMove; }
        }


        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _totalTime = 0;

            _runAwayDuration = 3;
            _scale = 1;
        }


        protected override void RunAway()
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(DOVirtual.Float(1f, 2f, 1.0f, (t) =>
            {
                float t1 = transform.localScale.x / _scale;
                transform.localScale = new Vector3(t1, t1, 1);
                _scale = t;
                float t2 = transform.localScale.x * _scale;
                transform.localScale = new Vector3(t2, t2, 1);
            }).SetEase(Ease.OutBack));

            sq.Append(DOVirtual.Float(2f, 1f, 1.0f, (t) =>
            {
                float t1 = transform.localScale.x / _scale;
                transform.localScale = new Vector3(t1, t1, 1);
                _scale = t;
                float t2 = transform.localScale.x * _scale;
                transform.localScale = new Vector3(t2, t2, 1);
            }).SetEase(Ease.OutBounce));
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("ExpansionVirus", OriginColorLevel, 2);
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
            var virusLevel = VirusTool.GetVirusColorLevel("ExpansionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            var virusLevel = VirusTool.GetVirusColorLevel("ExpansionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
