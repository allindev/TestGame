using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettlePanel : BasePanel
    {

        [SerializeField] private List<Sprite> _coinSheets;
        [SerializeField] private GameObject _resultBg;
        [SerializeField] private Image coinImage;
        [SerializeField] private Text _coinText;
        [SerializeField] private Text _tipText;
        [SerializeField] private Button getAwardBtn;

        private List<string> _tipStrList;

        private int _index;
        private int _count;
        private bool _isActive;
        private void Awake()
        {
            _tipStrList = new List<string>();
            TextAsset str = Resources.Load<TextAsset>("Tip");
            var list = str.text.Split('/');
            for (int i = 0; i < list.Length; i++)
            {
                string tipstr = Regex.Replace(list[i], @"[\n\r]", "");
                _tipStrList.Add(tipstr);
            }
            _tipStrList.RemoveAt(list.Length - 1);

            getAwardBtn.onClick.AddListener(() =>
            {
                EventManager.TriggerEvent(new FirstByteClick5DownEvent());
            });
        }

        private void Update()
        {
            if (_isActive)
            {
                _count++;
                if (_count > 4)
                {
                    _count = 0;
                    _index++;
                    if (_index >= _coinSheets.Count)
                        _index = 0;

                    coinImage.sprite = _coinSheets[_index];
                    coinImage.SetNativeSize();
                }
            }
        }


        public void SetCoinText(int coin)
        {
            _coinText.text = VirusTool.GetStrByIntger(coin);
        }

        public override void Active()
        {
            _resultBg.transform.localScale = Vector3.zero;
            _resultBg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            gameObject.SetActive(true);
            _index = 0;
            _count = 0;
            _isActive = true;
            coinImage.sprite = _coinSheets[_index];
            int i = UnityEngine.Random.Range(0, _tipStrList.Count);
            _tipText.text = _tipStrList[i];
            _coinText.text = VirusGameDataAdapter.GetCurLevelCoin().ToString();
        }

        public override void UnActive()
        {
            _isActive = false;
            _resultBg.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
      


    }
}
