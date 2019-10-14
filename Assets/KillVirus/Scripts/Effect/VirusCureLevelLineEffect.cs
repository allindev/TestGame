using UnityEngine;

public class VirusCureLevelLineEffect : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public void UpdateLine(Transform startPos, Transform endPos, ColorLevel level)
    {
        Vector3 offset = endPos.position - startPos.position;
        float dis = offset.magnitude;
        transform.position = startPos.position;
        transform.right = offset.normalized;
        transform.localScale = new Vector3(dis / 1.92f, 1, 1);

        int index = (int)level;
        Sprite sprite = VirusSpritesMrg.Instance.GetCureLineSprite(index);
        _spriteRenderer.sprite = sprite;
    }


}