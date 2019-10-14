using System.Collections.Generic;
using Assets.Tool.Pools;
using UnityEngine;

namespace Enemy.Entity
{
    public class AdsorptionVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove virusMove;
        [SerializeField] private VirusHurtEffect virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private Transform _rotateTransform;
        [SerializeField] private List<Transform> _corners;

        private float _totalTime;
        private float _duration;
        private Dictionary<int, bool> _cornerCache;
        private Dictionary<VirusMove, int> _transformCache;
        private List<VirusMove> _virusList;


        private void Update()
        {
            RemoveEmptyCorner();
            RunAway();
            _totalTime += Time.deltaTime;
            if (_totalTime >= _duration)
            {
                _totalTime -= _duration;
                FillCorner();
            }
            for (int i = 0; i < _virusList.Count; i++)
            {
                var t = _virusList[i];
                if (_transformCache.ContainsKey(t))
                {
                    int index = _transformCache[t];
                    Vector3 dir = (_corners[index].position - transform.position);
                    t.transform.position = _corners[index].position + dir * (t.Radius / 2f + 0.1f);
                }
            }
        }



        private void FillCorner()
        {
            float scale = transform.localScale.x;
            var layer = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, virusMove.Radius * scale, layer);
            for (int i = 0; i < coliders.Length; i++)
            {
                var c = coliders[i];
                int index = GetIndex();
                if (index < 0)
                    return;

                var vm = c.GetComponent<VirusMove>();
                bool b1 = !_cornerCache[index];
                bool b2 = c.transform != transform;
                bool b3 = !_virusList.Contains(vm);
                bool b4 = !(c.transform.GetComponent<BaseVirus>() is AdsorptionVirus);
                if (b1 && b2 && b3 && b4 && vm.IsUpdate)
                {
                    vm.IsUpdate = false;
                    Vector3 dir = (_corners[index].position - transform.position);
                    c.transform.position = _corners[index].position + dir * (vm.Radius / 2f + 0.1f);
                    _cornerCache[index] = true;
                    _virusList.Add(vm);
                    _transformCache.Add(vm, index);
                    InitiMaxBorder();
                }
            }
        }

        private void RemoveEmptyCorner()
        {
            List<VirusMove> mm = new List<VirusMove>();
            for (int i = 0; i < _virusList.Count; i++)
            {
                var m = _virusList[i];
                bool b1 = !m.gameObject.activeSelf;
                if (b1)
                {
                    if (_transformCache.ContainsKey(m))
                    {
                        int index = _transformCache[m];
                        _transformCache.Remove(m);
                        _cornerCache[index] = false;
                        mm.Add(m);
                    }
                }
            }
            for (int i = 0; i < mm.Count; i++)
            {
                _virusList.Remove(mm[i]);
            }
            mm.Clear();
        }

        private void InitiMaxBorder()
        {
            float max = 0;
            for (int i = 0; i < _virusList.Count; i++)
            {
                var v = _virusList[i];
                if (v.Radius > max)
                {
                    max = v.Radius;
                }
            }
            virusMove.BorderRadius = virusMove.Radius + max * 2 + 0.2f;
        }

        private int GetIndex()
        {
            for (int i = 0; i < 5; i++)
            {
                if (!_cornerCache[i])
                {
                    return i;
                }
            }
            return -1;
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
            _duration = 0.1f;

            _cornerCache = new Dictionary<int, bool>();
            _cornerCache.Add(0, false);
            _cornerCache.Add(1, false);
            _cornerCache.Add(2, false);
            _cornerCache.Add(3, false);
            _cornerCache.Add(4, false);

            _virusList = new List<VirusMove>();
            _transformCache = new Dictionary<VirusMove, int>();
        }


        protected override void RunAway()
        {
            _rotateTransform.localEulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
        }


        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("AdsorptionVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            for (int i = 0; i < _virusList.Count; i++)
            {
                var vm = _virusList[i];
                if (vm != null && vm.gameObject.activeSelf)
                {
                    vm.IsUpdate = true;
                }
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
            var virusLevel = VirusTool.GetVirusColorLevel("AdsorptionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            float v = base.StartCure(icure, healthValue);
            var virusLevel = VirusTool.GetVirusColorLevel("AdsorptionVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
            if (virusLevel != CurColorLevel)
            {
                CurColorLevel = virusLevel;
                VirusSprite.Initi(CurColorLevel);
                transform.GetComponent<BubbleEffectMrg>().SetBubbleSprite(CurColorLevel);
            }
            return v;
        }


        public override void StopCure(ICure iCure)
        {
            _virusHealthAddEffect.StopHealthEffect(iCure);
        }


    }
}
