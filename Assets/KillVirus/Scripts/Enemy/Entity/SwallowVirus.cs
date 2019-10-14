using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemy.Entity
{
    public class SwallowVirus : BaseVirus
    {

        [SerializeField] private VirusHealthBar _healthBar;
        [SerializeField] private VirusMove _virusMove;
        [SerializeField] private VirusHurtEffect _virusHurtEffect;
        [SerializeField] private VirusCureEffect _virusHealthAddEffect;

        [SerializeField] private VirusPingPongAction _top;
        [SerializeField] private VirusPingPongAction _bottom;
        

        private float _totalTime;
        private float _duration;
        private bool _isRunAway;
        private bool _invincible;

        private void Update()
        {
            if (!_isRunAway)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _duration)
                {
                    _totalTime -= _duration;
                    _isRunAway = true;
                    RunAway();
                }
            }
            _top.OnUpdate();
            _bottom.OnUpdate();
        }

        private void Produce()
        {
            VirusData data = new VirusData();
            data.SplitLevel = SplitLevel;
            data.VirusColorLevel = CurColorLevel;
            data.HealthValue = VirusTool.GetVirusHealthByColorLevel("SwallowVirus", VirusGameDataAdapter.GetLevel(), CurColorLevel);
            data.MoveSpeed = 4;
            data.MoveDirection = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector3.up;

            float scale = VirusTool.GetScaleByLevel(data.SplitLevel);
            var newVirus = VirusMrg.Instance.SpawnVirus("SwallowVirus", data, transform.position, true);
            newVirus.transform.localScale = Vector3.zero;
            newVirus.transform.DOScale(new Vector3(scale, scale, 1), 0.5f).OnComplete(() =>
            {
                _isRunAway = false;
            });
        }


        protected override VirusHealthBar HealthBar
        {
            get { return _healthBar; }
        }

        protected override VirusMove VirusMove
        {
            get { return _virusMove; }
        }

        protected override void RunAway()
        {
            var layer = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, _virusMove.Radius, layer);
            int index = -1;
            float minDis = _virusMove.Radius * _virusMove.Radius;
            for (int i = 0; i < coliders.Length; i++)
            {
                float dis = (coliders[i].transform.position - transform.position).sqrMagnitude;
                bool b1 = coliders[i].transform != transform;
                bool b2 = (coliders[i].GetComponent<BaseVirus>() is SwallowVirus);
                if (dis < minDis && b1 && b2)
                {
                    index = i;
                    minDis = dis;
                }
            }
            if (index > -1)
            {
                var move = coliders[index].GetComponent<VirusMove>();
                float dis = (coliders[index].transform.position - transform.position).magnitude;
                dis += move.Radius;
                var bv = coliders[index].GetComponent<BaseVirus>();
                bool b1 = dis <= _virusMove.Radius;
                bool b2 = bv.SplitLevel < SplitLevel;
                bool b3 = bv.VirusHealth.Value < VirusHealth.Value;
                if (b1 && b2 && b3)
                {
                    _invincible = true;
                    _virusMove.IsUpdate = false;
                    move.IsUpdate = false;
                    _top.Initi(new Vector3(0, -0.6f, 0), Vector3.zero, 0.2f);
                    _bottom.Initi(new Vector3(0, 0.48f, 0), Vector3.zero, 0.2f);
                    Transform t1 = coliders[index].transform;
                    DOVirtual.Float(0, 540, 0.8f, t => { t1.localEulerAngles = new Vector3(0, 0, t); });
                    t1.DOScale(Vector3.zero, 1f);
                    DOVirtual.DelayedCall(3.0f, () =>
                    {
                        _virusMove.IsUpdate = true;
                        _invincible = false;

                        bv.Death(false, false, false, false);
                        VirusMrg.Instance.MinusVirusCount(bv);

                        _top.Initi(new Vector3(0, -0.4f, 0), Vector3.zero, 0.5f);
                        _bottom.Initi(new Vector3(0, 0.32f, 0), Vector3.zero, 0.5f);

                        Produce();
                    });
                    return;
                }
                _isRunAway = false;
            }
            else
            {
                _isRunAway = false;
            }
        }

        public override void Death(bool isEffect, bool isDivide, bool isCoin, bool isProp)
        {
            if (isDivide)
                Divide("SwallowVirus", OriginColorLevel, 2);
            base.Death(isEffect, isDivide, isCoin, isProp);
            Pools.Despawn(gameObject);
        }

        public override void Born(VirusData virusData)
        {
            base.Born(virusData);
            _top.Initi(new Vector3(0, -0.4f, 0), Vector3.zero, 0.5f);
            _bottom.Initi(new Vector3(0, 0.32f, 0), Vector3.zero, 0.5f);
            _duration = 0.1f;

            _invincible = false;
        }

        public override void Injured(float damageValue,bool isEffect)
        {
            if (!_invincible)
            {
                if (VirusHealth.Value - damageValue <= 0)
                {
                    Death(true, true, true, true);
                    return;
                }
                VirusHealth.Value -= damageValue;
                var virusLevel = VirusTool.GetVirusColorLevel("SwallowVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
            var virusLevel = VirusTool.GetVirusColorLevel("SwallowVirus", VirusGameDataAdapter.GetLevel(), VirusHealth.Value);
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
