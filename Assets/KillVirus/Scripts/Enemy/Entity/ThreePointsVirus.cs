using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class ThreePointsVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private List<VirusScaleAction> _scaleActions;



        private void Update()
        {
            RunAway();
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
            for (int i = 0; i < _scaleActions.Count; i++)
            {
                var item = _scaleActions[i];
                item.OnUpdate();
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("ThreePointsVirus", OriginColorLevel, 3);
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
            var virusLevel = VirusTool.GetVirusColorLevel("ThreePointsVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                _virusHurtEffect.StartHurtEffect();
        }


        public override float StartCure(ICure iCure, float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(iCure);
            }
            else
            {
                _virusHealthAddEffect.StopHealthEffect(iCure);
            }
            float vv = base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("ThreePointsVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
