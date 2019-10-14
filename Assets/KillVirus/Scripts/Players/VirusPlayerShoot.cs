using UnityEngine;

public class VirusPlayerShoot : MonoBehaviour
{

    [SerializeField] private Transform _shootPos;

    private float _shootDuration;
    private float _interval;
    private float _totalTime;
    private bool _isStartShoot;


    public bool IsStartShoot
    {
        set { _isStartShoot = value; } 
        get { return _isStartShoot; }
    }

    public void OnAwake()
    {
        _shootDuration = 0.1f;
        _interval = 0.35f;
    }

    public void OnUpdate()
    {
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
        string bulletName = "BulletBlue";
        bool coin = VirusPlayerDataAdapter.GetShootCoin();
        bool power = VirusPlayerDataAdapter.GetPower();
        if (coin && power)
            bulletName = "BulletCoinPower";
        if (coin && !power)
            bulletName = "BulletCoin";
        if (!coin && power)
            bulletName = "BulletPower";

        int damage = VirusPlayerDataAdapter.GetShootPower();
        var obj = BulletPools.Instance.Spawn(bulletName);
        obj.transform.position = pos;
        obj.transform.eulerAngles = euler;
        obj.GetComponent<VirusPlayerBulletMove>().InitiDir(boderX, isSector);
        obj.GetComponent<VirusBulletDamage>().Initi(damage);
    }

   


}