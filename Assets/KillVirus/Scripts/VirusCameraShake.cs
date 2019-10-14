
using DG.Tweening;
using UnityEngine;

public class VirusCameraShake : Singleton<VirusCameraShake>
{

    private bool _isCanShake;
    private void Awake()
    {
        _isCanShake = true;
    }

    public void Shake()
    {
        if (_isCanShake)
        {
            _isCanShake = false;
            Vector2 r = Random.insideUnitCircle * 0.5f;
            transform.DOShakePosition(0.2f, new Vector3(r.x, r.y)).OnComplete(() =>
            {
                _isCanShake = true;
            });
        }
    }
   

}