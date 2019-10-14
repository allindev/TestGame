using System.Collections.Generic;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class DefendingVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private List<VirusFadeAction> _fadeViruss;



        private bool _invincible;
        private bool _isFaded;
        private float _invincibleDuration;
        private float _noinvincibleDuration;
        private float _totalTime;

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
            if (_invincible)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _invincibleDuration && !_isFaded)
                {
                    _isFaded = true;
                    _totalTime = 0;
                    _noinvincibleDuration = Random.Range(5f, 10f);
                    _virusHealthBar.gameObject.SetActive(true);
                    foreach (VirusFadeAction t in _fadeViruss)
                    {
                        t.FadeOut(0.5f);
                    }
                    DOVirtual.DelayedCall(0.5f, () => { _invincible = false; _isFaded = false; });
                }
            }
            else
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _noinvincibleDuration && !_isFaded)
                {
                    _isFaded = true;
                    _totalTime = 0;
                    _virusHealthBar.gameObject.SetActive(false);
                    _invincibleDuration = Random.Range(2f, 4f);
                    foreach (VirusFadeAction t in _fadeViruss)
                    {
                        t.FadeIn(0.5f);
                    }
                    DOVirtual.DelayedCall(0.5f, () => { _invincible = true; _isFaded = false; });
                }
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("DefendingVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            Pools.Despawn(gameObject);
        }


        public override void Born(VirusData virusData)
        {
            base.Born(virusData);

            _invincible = true;
            _isFaded = false;
            _invincibleDuration = Random.Range(2f, 4f);
            _noinvincibleDuration = Random.Range(5f, 10f);
            _totalTime = 0;
            _virusHealthBar.gameObject.SetActive(false);
            foreach (VirusFadeAction t in _fadeViruss)
            {
                t.Initi();
            }
        }


        public override void Injured(float damageValue, bool isEffect)
        {
            if (!_invincible)
            {
                if (VirusHealth.Value - damageValue <= 0)
                {
                    Death(true, true, true, true);
                    return;
                }
                VirusHealth.Value -= damageValue;
                var virusLevel = VirusTool.GetVirusColorLevel("DefendingVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
                if (virusLevel != CurColorLevel)
                {
                    CurColorLevel = virusLevel;
                    VirusSprite.Initi(CurColorLevel);
                    transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
                }
                if (isEffect)
                    virusHurtEffect.StartHurtEffect();
            }
        }


        public override float StartCure(ICure iCure, float healthValue)
        {
            if (Mathf.Abs(TotalHealth - VirusHealth.Value) > 0.1f)
            {
                _virusHealthAddEffect.StartHealthEffect(iCure);
            }
            float vv= base.StartCure(iCure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("DefendingVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
