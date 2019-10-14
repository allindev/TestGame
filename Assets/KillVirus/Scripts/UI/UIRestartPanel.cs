using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRestartPanel : BasePanel
    {

        [SerializeField] private Text _leftTimeText;
        [SerializeField] private GameObject _adBtn;
        [SerializeField] private CanvasGroup canvasGroup;


        public void SetLeftTime(int value)
        {
            _leftTimeText.text = value.ToString();
        }


        public override void Active()
        {
            gameObject.SetActive(true);
            _adBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(1, 0.5f);
        }


        public override void UnActive()
        {
            gameObject.SetActive(false);
            _adBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(0, 0.5f);
        }

    }
}
