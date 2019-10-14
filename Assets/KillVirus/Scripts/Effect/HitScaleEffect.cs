using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HitScaleEffect : MonoBehaviour
{

    [SerializeField] private float duration;
    [SerializeField] private List<Sprite> _hitSprites;
    [SerializeField] private SpriteRenderer _hit;
    [SerializeField] private GameObject _star;

    private void OnEnable()
    {
        float x = Random.Range(0.6f, 1f);
        transform.DOScale(new Vector3(0.2f, 0.2f, 1), duration);
        int index = Random.Range(0, _hitSprites.Count);
        _hit.sprite = _hitSprites[index];
        _star.transform.localScale = new Vector3(x, x, 1);
        SetStar();
    }

    private void SetStar()
    {
        float radius = 0.4f;
        float angle = Random.Range(0, 360);
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
        _star.transform.localPosition = dir * radius;
        _star.transform.right = dir;
    }

}