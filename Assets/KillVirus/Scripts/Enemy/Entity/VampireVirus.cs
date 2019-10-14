using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;


namespace Enemy.Entity
{
    public class VampireVirus : BaseVirus,ICure
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;
        [SerializeField] private Transform _rotateTransform;

        [SerializeField] private float _vampireRadius;

        private float _totalTime;
        private float _runAwayDuration;
        private float _vampireDuration;
        private bool _isVampire;
        private BaseVirus _targetVirus;
        private VampireLine _vampireLine;
       

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Injured(1, false);
            }

            if (!_isVampire)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _runAwayDuration)
                {
                    _totalTime = 0;
                    RunAway();
                }
            }
            else
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _vampireDuration)
                {
                    _isVampire = false;
                    _totalTime = 0;
                    StopVampire();
                    return;
                }
                if (_targetVirus != null && _targetVirus.gameObject.activeSelf)
                {
                    bool b1 = (transform.position - _targetVirus.transform.position).sqrMagnitude < _vampireRadius * _vampireRadius;
                    if (b1)
                    {
                        float delta = Time.deltaTime * 1;
                        float value = StartCure(this, delta);
                        _targetVirus.Injured(value, false);
                        if (_vampireLine != null)
                            _vampireLine.UpdateLine(transform, _targetVirus.transform, value >= delta);
                        if (value < delta)
                        {
                            StopCure(this);
                            return;
                        }
                        _rotateTransform.localEulerAngles += new Vector3(0, 0, 200f * Time.deltaTime);
                        return;
                    }
                }
                StopVampire();
            }
        }

        private void StopVampire()
        {
            if (_vampireLine != null)
            {
                EffectPools.Instance.DeSpawn(_vampireLine.gameObject);
                _vampireLine = null;
            }
            _targetVirus = null;
            StopCure(this);
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

            _runAwayDuration = 3f;
            _vampireDuration = 5f;
            _targetVirus = null;
        }


        protected override void RunAway()
        {
            LayerMask layerMask = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, _vampireRadius, layerMask);
            List<BaseVirus> list = new List<BaseVirus>();
            for (int i = 0; i < coliders.Length; i++)
            {
                var cc = coliders[i].GetComponent<BaseVirus>();
                bool b1 = cc is VampireVirus;
                if (!b1)
                {
                    list.Add(cc);
                }
            }
            if (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);
                if (list[index] != null)
                {
                    _isVampire = true;
                    _targetVirus = list[index];
                    var line = EffectPools.Instance.Spawn("VampireLine");
                    _vampireLine = line.GetComponent<VampireLine>();
                    _vampireLine.UpdateLine(transform, _targetVirus.transform, false);
                }
            }
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("VampireVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            if (_vampireLine != null)
            {
                EffectPools.Instance.DeSpawn(_vampireLine.gameObject);
                _vampireLine = null;
            }
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
            var virusLevel = VirusTool.GetVirusColorLevel("VampireVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            var virusLevel = VirusTool.GetVirusColorLevel("VampireVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
