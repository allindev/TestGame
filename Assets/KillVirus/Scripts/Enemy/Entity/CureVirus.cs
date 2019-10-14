using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class CureVirus : BaseVirus,ICure
    {

        [SerializeField] private VirusHealthBar _virusHealthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private Transform _rotateTransform;
        [SerializeField] private float _cureRadius;

        private List<BaseVirus> _beCuredVirus;
        private List<VirusCureLevelLineEffect> _lines;
        private float _duration;
        private float _totalTime;
        private float _cureDuration;
        private float _cureTotalTime;
        private bool _isCure;

       
        private void Awake()
        {
            _lines = new List<VirusCureLevelLineEffect>();
            Initi();
        }

        private void Update()
        {
            RunAway();
            if (!_isCure)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _duration)
                {
                    _totalTime -= _duration;
                    _isCure = true;
                    var layer = 1 << LayerMask.NameToLayer("Virus");
                    var coliders = Physics2D.OverlapCircleAll(transform.position, _cureRadius, layer);
                    for (int i = 0; i < coliders.Length; i++)
                    {
                        var beCured = coliders[i].transform.GetComponent<BaseVirus>();
                        bool b1 = beCured is CureVirus;
                        if (!b1)
                        {
                            _beCuredVirus.Add(beCured);
                            var line = EffectPools.Instance.Spawn("Line");
                            var virusLine = line.GetComponent<VirusCureLevelLineEffect>();
                            virusLine.UpdateLine(transform, beCured.transform, beCured.CurColorLevel);
                            _lines.Add(virusLine);
                        }
                    }
                }
            }
            else
            {
                _cureTotalTime += Time.deltaTime;
                if (_cureTotalTime >= _cureDuration)
                {
                    foreach (var item in _beCuredVirus)
                    {
                        item.StopCure(this);
                    }
                    _beCuredVirus.Clear();
                    _isCure = false;
                    _cureTotalTime = 0;
                    _cureDuration = Random.Range(2f, 3f);
                    DespawnAllLine();
                    return;
                }
                for (int i = 0; i < _beCuredVirus.Count; i++)
                {
                    var beCuredItem = _beCuredVirus[i];
                    var lineItem = _lines[i];
                    if (beCuredItem != null && beCuredItem.gameObject.activeSelf)
                    {
                        float dis = (transform.position - beCuredItem.transform.position).sqrMagnitude;
                        bool b = (dis <= _cureRadius * _cureRadius);
                        if (lineItem != null && b)
                        {
                            beCuredItem.StartCure(this, 10 * Time.deltaTime);
                            lineItem.UpdateLine(transform, beCuredItem.transform, beCuredItem.CurColorLevel);
                        }
                        else
                        {
                            DespawnLine(i);
                            beCuredItem.StopCure(this);
                        }
                    }
                    else
                    {
                        DespawnLine(i);
                    }
                }
            }
        }

        private void OnEnable()
        {
            Initi();
        }


        private void Initi()
        {
            _duration = Random.Range(1f, 3f);
            _cureDuration = Random.Range(2f, 3f);

            _cureTotalTime = 0;
            _totalTime = 0;
            _isCure = false;
            _beCuredVirus = new List<BaseVirus>();
        }

        private void DespawnAllLine()
        {
            if (_lines != null)
            {
                for (int i = 0; i < _lines.Count; i++)
                {
                    var itemLine = _lines[i];
                    if (itemLine != null && itemLine.gameObject != null && itemLine.gameObject.activeSelf)
                    {
                        EffectPools.Instance.DeSpawn(itemLine.gameObject);
                    }
                }
                _lines.Clear();
            }
        }

        private void DespawnLine(int index)
        {
            var itemLine = _lines[index];
            if (itemLine != null && itemLine.gameObject != null && itemLine.gameObject.activeSelf)
            {
                EffectPools.Instance.DeSpawn(itemLine.gameObject);
                _lines[index] = null;
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
            _rotateTransform.localEulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
        }

        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("CureVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            for (int i = 0; i < _beCuredVirus.Count; i++)
            {
                var item = _beCuredVirus[i];
                item.StopCure(this);
            }
            DespawnAllLine();
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
            var virusLevel = VirusTool.GetVirusColorLevel("CureVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            if (isEffect)
                virusHurtEffect.StartHurtEffect();
        }

        public override void StopCure(ICure icure)
        {
            
        }


    }
}
