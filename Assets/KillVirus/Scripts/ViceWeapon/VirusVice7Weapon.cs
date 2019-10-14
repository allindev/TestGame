using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice7Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private Transform _lefWing;
        [SerializeField] private Transform _righWing;

        [SerializeField] private Transform _shootPos;

        [SerializeField] private GameObject _leftEffect;
        [SerializeField] private GameObject _rightEffect;

        [SerializeField] private float _shootDuration;


        private float _totalTime;

        public override void Initi()
        {
            _totalTime = 0;
        }


        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _totalTime += Time.deltaTime;
            if (_totalTime >= _shootDuration)
            {
                _totalTime -= _shootDuration;
                Emit();
            }
        }


        public override void ReIniti()
        {

        }



        private void Emit()
        {
            _lefWing.DOLocalRotate(new Vector3(0, 0, -15), 0.5f);
            _righWing.DOLocalRotate(new Vector3(0, 0, 15), 0.5f).OnComplete(() =>
            {
                _leftEffect.SetActive(false);
                _rightEffect.SetActive(false);

                var obj = BulletPools.Instance.Spawn("ViceBullet_7");
                obj.GetComponent<ViceBullet7>().Initi(Random.Range(17000f,18000f));
                obj.transform.position = _shootPos.position;
                obj.transform.localScale = Vector3.zero;
            });

            DOVirtual.DelayedCall(0.8f, () =>
            {
                _lefWing.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
                _righWing.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
                {
                    _leftEffect.SetActive(true);
                    _rightEffect.SetActive(true);
                });
            });

        }



       
    }
}
