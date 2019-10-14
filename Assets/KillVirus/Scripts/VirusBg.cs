using System.Collections.Generic;
using UnityEngine;

public class VirusBg : MonoBehaviour
{

    [SerializeField] private List<Sprite> _bgSprites;

    private SpriteRenderer _spriteRenderer;
    private int _count;
    private void Awake()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _count = _bgSprites.Count;
    }

    private void Start()
    {
        Initi(Random.Range(0, _count));
    }

    public void Initi(int index)
    {
        _spriteRenderer.sprite = _bgSprites[index];
    }

}
