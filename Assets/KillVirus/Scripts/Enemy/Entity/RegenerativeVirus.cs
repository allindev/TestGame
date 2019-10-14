using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class RegenerativeVirus : BaseVirus,ICure
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;
        [SerializeField] private List<GameObject> _rotateObjects; 


        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return _virusMove; }
        }

        private float _totalTime;
        private bool _ishurt;
        private void Update()
        {
            RunAway();
            if (_ishurt)
            {
                _totalTime -= Time.deltaTime;
                if (_totalTime < 0)
                {
                    _ishurt = false;
                    if (Mathf.Abs(VirusHealth.Value - TotalHealth) > 0.1f)
                        _virusHealthAddEffect.StartHealthEffect(this);
                }
            }
            else
            {
                StartCure(this, Time.deltaTime * 10);
            }
        }



        protected override void RunAway()
        {
            for (int i = 0; i < _rotateObjects.Count; i++)
            {
                var item = _rotateObjects[i];
                item.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 80);
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("RegenerativeVirus", OriginColorLevel, 2);
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
            var virusLevel = VirusTool.GetVirusColorLevel("RegenerativeVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                _virusHurtEffect.StartHurtEffect();
            _virusHealthAddEffect.StopHealthEffect(this);
            _totalTime = 0.5f;
            _ishurt = true;
        }


        public override float StartCure(ICure iCure,float healthValue)
        {
            float vv = base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("RegenerativeVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (Mathf.Abs(VirusHealth.Value - TotalHealth) < 0.1f)
            {
                _virusHealthAddEffect.StopHealthEffect(this);
            }
            return vv;
        }


        public override void StopCure(ICure iCure)
        {
            _virusHealthAddEffect.StopHealthEffect(iCure);
        }



    }
}
