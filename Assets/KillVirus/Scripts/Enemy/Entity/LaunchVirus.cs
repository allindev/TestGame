using System;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class LaunchVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private Transform _lanuch;
        [SerializeField] private Transform _virus;


        private float _totalTime;
        private float _lanuchDuration;

        private void Update()
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _lanuchDuration)
            {
                RunAway();
                _totalTime -= _lanuchDuration;
            }

            _lanuch.right = Vector3.LerpUnclamped(_lanuch.right, virusMove.MoveDirection, Time.deltaTime * 10);
            _virus.right = Vector3.LerpUnclamped(_virus.right, virusMove.MoveDirection, Time.deltaTime * 10);
        }

        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return virusMove; }
        }



        protected override void RunAway()
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(_lanuch.DOScale(new Vector3(1.3f, 1, 1), 0.5f).SetEase(Ease.OutCubic));
            sq.AppendInterval(0.2f);
            sq.Append(_lanuch.DOScale(new Vector3(1f, 1, 1), 0.5f).SetEase(Ease.InCubic));
        }


        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _totalTime = 0;
            _lanuchDuration = 5;
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("LaunchVirus", OriginColorLevel, 2);
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
            var virusLevel = VirusTool.GetVirusColorLevel("LaunchVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            var virusLevel = VirusTool.GetVirusColorLevel("LaunchVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
