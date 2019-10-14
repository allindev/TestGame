using System.Collections.Generic;
using UnityEngine;

public class VirusSpriteSetter : MonoBehaviour
{

    [SerializeField] private List<SpriteRenderer> _spriteRenderers;
    [SerializeField] private Canvas _healthCanvas;


    public int SortIndex { set; get; }

    public void Set(int index)
    {
        _healthCanvas.sortingOrder = index;
        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            var item = _spriteRenderers[i];
            item.sortingOrder = index;
        }
        SortIndex = index;
    }


}
