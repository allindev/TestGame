using System.Collections.Generic;
using UnityEngine;

public class NormalVirusSprite : BaseVirusSprite
{

    [SerializeField] private List<VirusSpriteItem> items;
    [SerializeField] private SpriteRenderer _circle;
    [SerializeField] private SpriteRenderer _ring;
    [SerializeField] private VirusEnum virusEnum;


    public override void Initi(ColorLevel level)
    {
        int index = (int)level;
        if (_ring != null)
            _ring.sprite = VirusSpritesMrg.Instance.GetRingSprite(index);
        if (_circle != null)
            _circle.sprite = VirusSpritesMrg.Instance.GetCircleSprite(index);
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            string virusName = virusEnum.ToString();
            string strName = item.SpriteNames[index];
            Sprite sprite = VirusSpritesMrg.Instance.GetSpriteByName(virusName, strName);
            for (int j = 0; j < item.SpriteRenderers.Count; j++)
            {
                var tt = item.SpriteRenderers[j];
                tt.sprite = sprite;
            }
        }
    }


}
