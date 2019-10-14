using Assets.Tool.Pools;
using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice2Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private Transform _leftShootPos;
        [SerializeField] private Transform _rightShootPos;
        [SerializeField] private float _shootDuration;

        private float _totalTime;
        private bool _isLeft;

        public override void Initi()
        {
            _totalTime = 0;
            _isLeft = true;
        }

        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                _isLeft = !_isLeft;
                SpawnBullet(_isLeft);
            }
        }

        public override void ReIniti()
        {

        }


        private void SpawnBullet(bool isLeft)
        {
            var obj = BulletPools.Instance.Spawn("ViceBullet_2");
            obj.transform.localPosition = isLeft ? _leftShootPos.position : _rightShootPos.position;

            var bullet = obj.GetComponent<ViceBullet2>();
            bullet.Emit(Quaternion.Euler(0, 0, isLeft ? 5 : -5) * Vector3.up);
            bullet.Initi(Random.Range(150, 300));
        }


       
    }

}
