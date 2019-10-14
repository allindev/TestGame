using Assets.Tool.Pools;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class TrackingVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private List<VirusFadeAction> _fadeTransform;


        private float _trackDuration;
        private float _wanderDuration;
        private float _totalTime;
        private bool _isTracking;

        private void Update()
        {
            if (!_isTracking)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _wanderDuration)
                {
                    _totalTime = 0;
                    _trackDuration = Random.Range(10f, 20f);
                    RunAway();
                    _isTracking = true;
                }
            }
            else
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _trackDuration)
                {
                    _virusMove.IsTracking = false;
                    _totalTime = 0;
                    _wanderDuration = Random.Range(3f, 4f);
                    for (int i = 0; i < _fadeTransform.Count; i++)
                    {
                        int index = i;
                        DOVirtual.DelayedCall(0.05f * index, () => { _fadeTransform[index].FadeIn(0.3f); });
                    }
                    _isTracking = false;
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
            _virusMove.IsTracking = true;
            for (int i = 0; i < _fadeTransform.Count; i++)
            {
                int index = i;
                DOVirtual.DelayedCall(0.05f * index, () => { _fadeTransform[index].FadeOut(0.3f); });
            }
        }

        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _isTracking = false;
            _totalTime = 0;
            _wanderDuration = Random.Range(3f, 4f);
            foreach (VirusFadeAction t in _fadeTransform)
            {
                t.Initi();
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("TrackingVirus", OriginColorLevel, 2);
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
            var virusLevel = VirusTool.GetVirusColorLevel("TrackingVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            float vv=base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("TrackingVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
