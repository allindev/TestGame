using DG.Tweening;
using UnityEngine;

public class VirusFadeAction : MonoBehaviour
{

    [SerializeField] private Vector3 outPos;
    [SerializeField] private Vector3 inPos;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isEnablePos;
    [SerializeField] private bool _isEnableAlpha;

    public void Initi()
    {
        if (_isEnableAlpha)
            _spriteRenderer.color = Color.white;
        if (_isEnablePos)
            transform.localPosition = inPos;
    }

    public void FadeIn(float time)
    {
        if (_isEnableAlpha)
            _spriteRenderer.DOFade(1f, time).SetEase(Ease.Linear);
        if (_isEnablePos)
            transform.DOLocalMove(inPos, time).SetEase(Ease.Linear);
    }

    public void FadeOut(float time)
    {
        if (_isEnableAlpha)
            _spriteRenderer.DOFade(0f, time).SetEase(Ease.Linear);
        if (_isEnablePos)
            transform.DOLocalMove(outPos, time).SetEase(Ease.Linear);
    }


    [ContextMenu("SetOutPos")]
    protected void SetOutPos()
    {
        outPos = transform.localPosition + transform.up * 0.2f;
    }

    [ContextMenu("SetInPos")]
    protected void SetInPos()
    {
        inPos = transform.localPosition;
    }



}