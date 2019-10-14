using System.Collections.Generic;
using UnityEngine;

public class SpecialVirusSprite:BaseVirusSprite
{

    [SerializeField] private List<SpriteRenderer> _circleRenderers;
    [SerializeField] private SpriteRenderer _ring;

    public override void Initi(ColorLevel level)
    {
        int index = (int)level;
        if (_ring != null)
            _ring.sprite = VirusSpritesMrg.Instance.GetRingSprite(index);

        var circleSprite = VirusSpritesMrg.Instance.GetCircleSprite(index);
        for (int i = 0; i < _circleRenderers.Count; i++)
        {
            var item = _circleRenderers[i];
            item.sprite = circleSprite;
        }
    }
}