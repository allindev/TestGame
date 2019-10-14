using UnityEngine;

public class VirusAddLevelEffect : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Initi(ColorLevel colorLevel)
    {
        int index = (int)colorLevel;
        _spriteRenderer.sprite = VirusSpritesMrg.Instance.GetCureAddSprite(index);
    }

}