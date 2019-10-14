using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITittlePanel : BasePanel
    {
        [Header("RectTransform")]
        [SerializeField] private RectTransform _rotateBg;
        [SerializeField] private RectTransform _titleBg;
        [SerializeField] private RectTransform _coinBg;

        [Header("Image")]
        [SerializeField] private Image lineImage;
        [SerializeField] private Image topLineImage;
        [SerializeField] private Image bottonLineImage;
        [SerializeField] private Image _progressBar;

        [Header("GameObject")]
        [SerializeField] private GameObject _progressBg;
        [SerializeField] private GameObject topCircle;
        [SerializeField] private GameObject bottomCircle;
        [SerializeField] private GameObject iconObj;
        [SerializeField] private GameObject tipObj;

        [Header("Text")]
        [SerializeField] private Text _progressNum;



        private bool _isRotate;
        private bool _isLeft;
        private void Awake()
        {
            iconObj.SetActive(false);
            topCircle.SetActive(false);
            bottomCircle.SetActive(false);
            SetObjsState(false);
            _titleBg.anchoredPosition = new Vector2(0, 1200);
            _titleBg.localScale = Vector3.zero;

            _isRotate = false;
            _isLeft = true;

            _progressNum.text = "0%";
            _progressBar.fillAmount = 0;
        }

        private void Update()
        {
            if (_isRotate)
            {
                if (_isLeft)
                {
                    _rotateBg.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 10);
                    if (_rotateBg.localEulerAngles.z > 15 && _rotateBg.localEulerAngles.z < 90)
                        _isLeft = false;
                }
                else
                {
                    _rotateBg.localEulerAngles -= new Vector3(0, 0, Time.deltaTime * 10);
                    if (_rotateBg.localEulerAngles.z < 345f && _rotateBg.localEulerAngles.z > 270f)
                        _isLeft = true;
                }
            }
        }

        private void SetObjsState(bool active)
        {
            lineImage.gameObject.SetActive(active);
            topLineImage.gameObject.SetActive(active);
            bottonLineImage.gameObject.SetActive(active);
        }




        public override void Active()
        {
            SetObjsState(true);
            _rotateBg.gameObject.SetActive(true);
            lineImage.fillAmount = 0;
            topLineImage.fillAmount = 0;
            bottonLineImage.fillAmount = 0;

            _titleBg.DOAnchorPos(new Vector2(0, 500f), 1f);
            _coinBg.anchoredPosition = new Vector2(-460f, 1100);
            _coinBg.DOAnchorPosY(900f, 0.8f);
            Sequence sq = DOTween.Sequence();
            sq.Append(DOVirtual.Float(0, 1, 1, (t) => { _titleBg.localScale = new Vector3(t, t, t); }).OnComplete(() =>
            {
                iconObj.SetActive(true);
            }));
            sq.Append(DOVirtual.Float(0, 1, 0.5f, (t) => { lineImage.fillAmount = t; }));
            sq.Append(DOVirtual.Float(0, 1, 0.3f, (t) =>
            {
                topLineImage.fillAmount = t;
                bottonLineImage.fillAmount = t;
            })).OnComplete(() =>
            {
                topCircle.SetActive(true);
                bottomCircle.SetActive(true);
                topLineImage.gameObject.SetActive(false);
                bottonLineImage.gameObject.SetActive(false);

                _isRotate = true;
            });
        }

        public override void UnActive()
        {
           
            _rotateBg.gameObject.SetActive(false);
            iconObj.SetActive(false);
            topCircle.SetActive(false);
            bottomCircle.SetActive(false);
            SetObjsState(false);
            _isRotate = false;

            _titleBg.DOAnchorPosY(1200, 1f).SetEase(Ease.InOutBack);
            _titleBg.transform.DOScale(Vector3.zero, 1.0f);
            _coinBg.DOAnchorPosY(1100f, 0.8f);
        }

        public void SetLoadingBar(float value)
        {
            _progressBar.fillAmount = value;
            _progressNum.text = String.Format("{0}%", (int)(value * 100));
            if (value >= 1)
            {
                _progressBg.SetActive(false);
                tipObj.SetActive(false);
            }
        }

      
    }
}
