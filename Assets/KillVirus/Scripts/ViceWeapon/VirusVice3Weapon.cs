using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice3Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private GameObject _lefMiniWing;
        [SerializeField] private GameObject _rightMiniWing;
        [SerializeField] private ViceBullet3 _viceBullet3;
        [SerializeField] private float _shootDuration;

        private float _totalTime;

        private int _num;
        private bool _isActive;

        public override void Initi()
        {
            _isActive = true;
            _lefMiniWing.SetActive(true);
            _rightMiniWing.SetActive(true);

            _totalTime = 0;
            _viceBullet3.Initi(Random.Range(1600, 2000));
        }


        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _num++;
            if (_num > 5)
            {
                _num = 0;
                _isActive = !_isActive;
                _lefMiniWing.SetActive(_isActive);
                _rightMiniWing.SetActive(_isActive);
            }

            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                _viceBullet3.Show();
            }
        }


        public override void ReIniti()
        {

        }

    }
}
