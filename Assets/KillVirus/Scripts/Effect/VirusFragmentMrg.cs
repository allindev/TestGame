using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VirusFragmentMrg : MonoBehaviour
{

    private List<GameObject> _fragments;

    public void Initi(int index,int count)
    {
        _fragments = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = EffectPools.Instance.Spawn("Fragment");
            obj.transform.SetParent(transform);
            float scale = Random.Range(0.2f, 0.5f);
            obj.transform.localScale = new Vector3(scale, scale, 1);
            Vector3 dir = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.right;
            obj.transform.localPosition = Vector3.zero;
            float d1 = 0.2f;
            obj.transform.DOLocalMove(dir * Random.Range(1.5f, 2f), d1);
            obj.GetComponent<VirusFragmentEffect>().Initi(index);
        }
    }


    private void OnDisable()
    {
        if (_fragments != null && _fragments.Count > 0)
        {
            for (int i = 0; i < _fragments.Count; i++)
            {
                var obj = _fragments[i];
                EffectPools.Instance.DeSpawn(obj);
            }
        }
    }

}
