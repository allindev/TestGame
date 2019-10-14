using UnityEngine;

public class VirusAutoDespawn : MonoBehaviour
{

    [SerializeField]
    private float _delayTime;

    private float _totalTime;
    private bool _isUpdate;



    private void OnEnable()
    {
        _totalTime = 0;
        _isUpdate = true;
    }


    private void Update()
    {
        _totalTime += Time.deltaTime;
        if (_totalTime >= _delayTime && _isUpdate)
        {
            EffectPools.Instance.DeSpawn(gameObject);
        }
    }


}