using UnityEngine;

public class VirusPlayerBulletMove : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _threshold;

   
    private bool _isSector;
    private float _totalTime;
    private float _borderX;
    private float _startX;

    public void InitiDir(float borderX, bool isSector)
    {
        _startX = transform.position.x;
        _borderX = borderX;
        _isSector = isSector;
        if (_isSector)
        {
            _totalTime = 0;
        }
    }


    private void Update()
    {
        if (_isSector)
        {
            _totalTime += Time.deltaTime;
            if (_totalTime >= 0.15f)
            {
                _totalTime = 0.15f;
                _isSector = false;
            }
            float x = Mathf.Lerp(_startX, _borderX, _totalTime / 0.15f);
            transform.position = new Vector3(x, transform.position.y, 0);
        }
        transform.position += Vector3.up * _speed * Time.deltaTime;
        if (transform.position.y > _threshold)
        {
            BulletPools.Instance.DeSpawn(gameObject);
        }
    }




}