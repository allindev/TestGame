using System.Collections.Generic;
using UnityEngine;

public class BubbleEffectMrg : MonoBehaviour
{

    private List<GameObject> _bubbleObjects;

    public void Initi(int sortIndex, SplitLevel splitLevel, ColorLevel colorLevel)
    {
        DetroyBubbles();
        if (IsSpawn(splitLevel))
        {
            _bubbleObjects = new List<GameObject>();
            int count = Random.Range(3, 6);
            for (int i = 0; i < count; i++)
            {
                SpawnBubble(sortIndex, colorLevel);
            }
        }
    }

    public void SetBubbleSprite(ColorLevel colorLevel)
    {
        if (_bubbleObjects != null && _bubbleObjects.Count > 0)
        {
            for (int i = 0; i < _bubbleObjects.Count; i++)
            {
                var bubble = _bubbleObjects[i];
                bubble.GetComponent<BubbleEffect>().SetSprite(colorLevel);
            }
        }
    }

    public void DetroyBubbles()
    {
        if (_bubbleObjects != null)
        {
            for (int i = 0; i < _bubbleObjects.Count; i++)
            {
                EffectPools.Instance.DeSpawn(_bubbleObjects[i]);
            }
            _bubbleObjects.Clear();
        }
    }
   


    private void SpawnBubble(int sortIndex, ColorLevel colorLevel)
    {
        int level = (int)colorLevel;
        var bubble = EffectPools.Instance.Spawn("BubbleEffect");
        float radius = Random.Range(1f, 1.3f);
        float angle = Random.Range(0, 2 * Mathf.PI);

        bubble.transform.SetParent(transform);
        bubble.transform.localPosition = new Vector2(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius);
        bubble.GetComponent<BubbleEffect>().Initi(0.1f, 1.2f, level, sortIndex);
        _bubbleObjects.Add(bubble);
    }


    private bool IsSpawn(SplitLevel splitLevel)
    {
        switch (splitLevel)
        {
            case SplitLevel.Level1:
                return Random.Range(0, 100f) > 80f;
            case SplitLevel.Level2:
                return Random.Range(0, 100f) > 20f;
            case SplitLevel.Level3:
                return Random.Range(0, 100f) > 15f;
            case SplitLevel.Level4:
                return Random.Range(0, 100f) > 10f;
            case SplitLevel.Level5:
                return Random.Range(0, 100f) > 5f;
        }
        return false;
    }

  



}