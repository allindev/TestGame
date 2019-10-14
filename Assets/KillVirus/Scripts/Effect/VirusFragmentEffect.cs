using DG.Tweening;
using UnityEngine;

public class VirusFragmentEffect : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Initi(int index)
    {
        _spriteRenderer.sprite = VirusSpritesMrg.Instance.GetFragmentSprite(index);
        _spriteRenderer.color = Color.white;
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
        _spriteRenderer.DOFade(0, 0.4f);
    }


}
