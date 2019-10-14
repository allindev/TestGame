using DG.Tweening;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour
{

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            EffectPools.Instance.DeSpawn(gameObject);
        });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.LogError("sss");
        }
    }

}