using Assets.Tool.Pools;
using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice1Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private Transform _leftminWing;
        [SerializeField] private Transform _rightminWing;

        [SerializeField] private Transform _leftShootPos;
        [SerializeField] private Transform _rightShootPos;
        [SerializeField] private float _shootDuration;

        private ViceBullet1 _leftBullet;
        private ViceBullet1 _rightBullet;
        private float _totalTime;
        private bool _isLeftShoot;
       

        private void SpawnLeftBullet()
        {
            var obj = BulletPools.Instance.Spawn("ViceBullet_1");
            obj.transform.parent = _leftShootPos;
            obj.transform.localPosition = Vector3.zero;

            float value = Random.Range(5f, 10f) * VirusGameDataAdapter.GetLevel();
            _leftBullet = obj.GetComponent<ViceBullet1>();
            _leftBullet.Initi();
            _leftBullet.Initi(value);
        }

        private void SpawnRightBullet()
        {
            var obj = BulletPools.Instance.Spawn("ViceBullet_1");
            obj.transform.parent = _rightShootPos;
            obj.transform.localPosition = Vector3.zero;

            float value = Random.Range(5f, 10f) * VirusGameDataAdapter.GetLevel();
            _rightBullet = obj.GetComponent<ViceBullet1>();
            _rightBullet.Initi();
            _rightBullet.Initi(value);
        }



        public override void Initi()
        {
            _leftminWing.localEulerAngles = Vector3.zero;
            _rightminWing.localEulerAngles = Vector3.zero;

            _isLeftShoot = true;
            _totalTime = 0;
            if (_leftBullet == null)
                SpawnLeftBullet();
            if (_rightBullet == null)
                SpawnRightBullet();
        }


        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            float delta = Time.deltaTime * 600f;
            _leftminWing.localEulerAngles -= new Vector3(0, 0, delta);
            _rightminWing.localEulerAngles += new Vector3(0, 0, delta);

            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                _isLeftShoot = !_isLeftShoot;
                if (_isLeftShoot)
                {
                    _leftBullet.Emit();
                    SpawnLeftBullet();
                }
                else
                {
                    _rightBullet.Emit();
                    SpawnRightBullet();
                }
            }
        }


        public override void ReIniti()
        {
            Initi();
        }


    }
}
