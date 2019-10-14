using System;
using System.Globalization;
using DG.Tweening;
using Events;
using Tool;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIMainPanel : BasePanel, IEventListener<UIVirusAddLevelCoinEvent>,
                                          IEventListener<UIVirusTipEvent>
    {

        [Header("RectTransform")]
        [SerializeField] private RectTransform _coinBg;
        [SerializeField] private RectTransform coinParent;

        [Header("Text")]
        [SerializeField] private Text _coinText;
        [SerializeField] private Text warningText;
        [SerializeField] private Text progressText;

        [Header("Image")]
        [SerializeField] private Image warning;
        [SerializeField] private Image warningTitle;
        [SerializeField] private Image progressImage;

        [SerializeField] private CanvasGroup _canvasGroup;


        private float _lastCoin;
        private bool _isLerp;
        private void Awake()
        {
            _isLerp = false;
            _coinText.text = "0";

            warning.gameObject.SetActive(false);
            warningTitle.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isLerp)
            {
                int totalCoin = VirusGameDataAdapter.GetCurLevelCoin();
                _lastCoin = Mathf.Lerp(_lastCoin, totalCoin, Time.deltaTime * 5);
                if (Mathf.Abs(_lastCoin - totalCoin) < 0.1f)
                {
                    _isLerp = false;
                }
                _coinText.text = VirusTool.GetStrByIntger(Mathf.CeilToInt(_lastCoin));
            }
        }

        private void OnEnable()
        {
            EventRegister.EventStartListening<UIVirusAddLevelCoinEvent>(this);
            EventRegister.EventStartListening<UIVirusTipEvent>(this);
        }

        private void OnDisable()
        {
            EventRegister.EventStopListening<UIVirusAddLevelCoinEvent>(this);
            EventRegister.EventStopListening<UIVirusTipEvent>(this);
        }

        private void ShowManyVirus()
        {
            warningTitle.gameObject.SetActive(true);
            warningTitle.enabled = false;
            warningText.text = "大量病毒入侵";
            DOVirtual.DelayedCall(3.0f, () => { warningTitle.gameObject.SetActive(false); });
        }

        private void ShowBigVirus()
        {
            VirusSoundMrg.Instance.PlaySound(VirusSoundType.Waring);
            warningTitle.enabled = true;
            warningText.text = "巨型病毒入侵";
            DOVirtual.Float(0, 31, 3.0f, (t) =>
            {
                int v = (int)t;
                warning.gameObject.SetActive(v % 3 == 0);
                warningTitle.gameObject.SetActive(v % 3 == 0);
            }).SetEase(Ease.Linear);
        }


      


        public void SetLeftVirus(float value)
        {
            float t = value * 100f;
            int v = Mathf.CeilToInt(t);
            progressText.text = String.Format("{0}%", v);
            progressImage.fillAmount = value;
        }


        public override void Active()
        {
            _coinText.text = "0";
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1f, 0.5f);
        }


        public override void UnActive()
        {
            _canvasGroup.DOFade(0f, 0.5f);
        }

      
        public void UpdateCoin(float value)
        {
            _coinText.text = value.ToString(CultureInfo.InvariantCulture);
        }


        public void OnEvent(UIVirusAddLevelCoinEvent eventType)
        {
            var rt = transform as RectTransform;
            Vector2 pos = VirusTool.SceneToUguiPos(rt, eventType.WorldPos);
            var coin = PropPools.Instance.Spawn("Coin");

            coin.transform.SetParent(coinParent);
            var rectTransform = coin.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = pos;
                Vector2 e = _coinBg.anchoredPosition;
                Vector2 s = pos;
                Vector2 dir = Quaternion.Euler(0, 0, -90) * (e - s).normalized;
                float mul = Random.Range(0, 2) == 1 ? 1f : -1f;
                float t1 = Random.Range(0.3f, 0.5f);
                Vector2 mid = Vector2.Lerp(s, e, t1) + mul * dir * Random.Range(1.0f, 2.0f) * 100f;
                float dis = (e - s).magnitude;
                DOVirtual.Float(0, 10, dis / 1000f, (t) =>
                {
                    Vector2 p = VirusTool.GetBesselPoint(s, e, mid, t / 10f);
                    rectTransform.anchoredPosition = p;
                }).OnComplete(() =>
                {
                    PropPools.Instance.DeSpawn(coin);
                    _isLerp = true;
                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.LevelCoin);
                });
            }
        }


        public void OnEvent(UIVirusTipEvent eventType)
        {
            if (WaveVirusDataAdapter.IsShowBoss())
                ShowBigVirus();
            else
                ShowManyVirus();
        }


    }

}
