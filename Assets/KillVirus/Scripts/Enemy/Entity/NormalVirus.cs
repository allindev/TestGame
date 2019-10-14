using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class NormalVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;

       
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
            _top.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 100);
            _bottom.transform.localEulerAngles += new Vector3(0, 0, -Time.deltaTime * 100);
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("NormalVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            Pools.Despawn(gameObject);
        }


        public override void Injured(float damageValue, bool isEffect)
        {
            if (!IsDeath)
            {
                if (VirusHealth.Value - damageValue <= 0)
                {
                    Death(true, true, true, true);
                    return;
                }
                VirusHealth.Value -= damageValue;
                var virusLevel = VirusTool.GetVirusColorLevel("NormalVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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


        public override float StartCure(ICure iCure,float healthValue)
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
            var virusLevel = VirusTool.GetVirusColorLevel("NormalVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
