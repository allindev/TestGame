using Assets.Tool.Pools;
using UnityEngine;

public class VirusFriend : MonoBehaviour
{

    [SerializeField] private Transform _shootPos;

    private float _shootDuration;
    private float _interval;
    private float _totalTime;
    private bool _isStartShoot;

    private VirusPlayerMove _virusPlayerMove;
    private bool _isFollow;
    private Vector3 _stopPos;


    public void FllowTarget(VirusPlayerMove playerMove)
    {
        _virusPlayerMove = playerMove;
        _isFollow = true;
        _isStartShoot = true;
    }

    public void NotFllowTarget()
    {
        _stopPos = new Vector3(transform.position.x, -10f, 0);
        _virusPlayerMove = null;
        _isFollow = false;
        _isStartShoot = false;
    }


    private void Awake()
    {
        _stopPos = new Vector3(transform.position.x, -10f, 0);
        _shootDuration = 0.1f;
        _interval = 0.35f;
    }

    private void Update()
    {
        if (_isFollow)
        {
            Vector3 endPos = _virusPlayerMove.transform.position - _virusPlayerMove.MoveDirection.normalized * 3;
            transform.position = Vector3.LerpUnclamped(transform.position, endPos, Time.deltaTime * 2f);
        }
        else
        {
            transform.position = Vector3.LerpUnclamped(transform.position, _stopPos, Time.deltaTime * 2);
        }

        if (_isStartShoot)
        {
            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                Shoot();
            }
        }
    }



    private void Shoot()
    {
        int shootNum = VirusPlayerDataAdapter.GetShootNum();
        float originX = 0;
        int v = shootNum % 2;
        int vv = shootNum / 2;
        originX = v == 1 ? -vv * _interval : -(vv - 0.5f) * _interval;
        for (int i = 0; i < shootNum; i++)
        {
            float x = originX + i * _interval + _shootPos.position.x;
            SpawnBullet(_shootPos.position, Vector3.zero, x, true);
        }
    }


    private void SpawnBullet(Vector3 pos, Vector3 euler, float boderX, bool isSector)
    {
        int value = VirusPlayerDataAdapter.GetShootPower();
        var obj = BulletPools.Instance.Spawn("BulletBlue");
        obj.transform.position = pos;
        obj.transform.eulerAngles = euler;
        obj.GetComponent<VirusPlayerBulletMove>().InitiDir(boderX, isSector);
        obj.GetComponent<VirusBulletDamage>().Initi(value);
    }





}
