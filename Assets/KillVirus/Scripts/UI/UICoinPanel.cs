using DG.Tweening;
using Events;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICoinPanel : BasePanel,IEventListener<UIVirusAddTotalCoinEvent>
    {

        [SerializeField] private RectTransform _coinBg;
        [SerializeField] private RectTransform coinParent;
        [SerializeField] private Text _coinText;


        private float _totalTime;
        private float _lastCoin;
        private bool _isLerp;
        private bool _isSet;
        private int _num;
       
        private void Awake()
        {
            _coinText.text = "0";
        }

        private void Update()
        {
            if (_isLerp)
            {
                int totalCoin = VirusGameDataAdapter.GetTotalCoin();
                _lastCoin = Mathf.Lerp(_lastCoin, totalCoin, Time.deltaTime*5);
                if (Mathf.Abs(_lastCoin - totalCoin) < 0.1f)
                {
                    _isLerp = false;
                    _isSet = false;
                }
                _coinText.text = VirusTool.GetStrByIntger(Mathf.RoundToInt(_lastCoin));
                _totalTime -= Time.deltaTime;
                if (_totalTime <= 0 && _num > 0)
                {
                    _num--;
                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.TotalCoin);
                    _totalTime = 0.15f;
                }
            }
        }

        private void OnEnable()
        {
            EventRegister.EventStartListening<UIVirusAddTotalCoinEvent>(this);
        }

        private void OnDisable()
        {
            EventRegister.EventStopListening<UIVirusAddTotalCoinEvent>(this);
        }



        public void SetCoinText()
        {
            string str = VirusTool.GetStrByIntger(VirusGameDataAdapter.GetTotalCoin());
            _coinText.text = str;
        }


        public override void Active()
        {
            _coinBg.anchoredPosition = new Vector2(-460f, 1100f);
            _coinBg.DOAnchorPosY(900f, 0.5f);
        }


        public override void UnActive()
        {
            _coinBg.DOAnchorPosY(1100, 0.5f); 
        }


        public void OnEvent(UIVirusAddTotalCoinEvent eventType)
        {
            Vector2 pos = eventType.UiPos;
            var coin = PropPools.Instance.Spawn("Coin");

            coin.transform.SetParent(coinParent);
            var rectTransform = coin.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = pos;
                Vector2 e = _coinBg.anchoredPosition;
                Vector2 s = pos;
                Vector2 dir = Quaternion.Euler(0, 0, -90) * (e - s).normalized;
                float mul = eventType.IsPositive ? 1f : -1f;
                float t1 = Random.Range(-0.5f, 0.5f);
                Vector2 mid = Vector2.Lerp(s, e, t1) + mul * dir * Random.Range(1.0f, 2.0f) * 200f;
                float dis = (e - s).magnitude;
                DOVirtual.Float(0, 10, dis / 1500f, (t) =>
                {
                    Vector2 p = VirusTool.GetBesselPoint(s, e, mid, t / 10f);
                    rectTransform.anchoredPosition = p;
                }).OnComplete(() =>
                {
                    PropPools.Instance.DeSpawn(coin);
                    _isLerp = true;
                    if (!_isSet)
                    {
                        _isSet = true;
                        _totalTime = 0.15f;
                        _num = 5;
                        VirusSoundMrg.Instance.PlaySound(VirusSoundType.TotalCoin);
                    }
                });
            }
        }


    }
}
