using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ViceWeapon
{
    public class VirusVice6MiniCanon : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _pieces;
        [SerializeField] private GameObject _leftWing;
        [SerializeField] private GameObject _rightWing;
        [SerializeField] private Transform _shootPos;
        [SerializeField] private GameObject _shootEffect;

        [SerializeField] private float _shootDuration;

        private Transform _target;
        private Transform _fllower;
        private VirusVice6Weapon _vice6Weapon;

        private bool _isLerp;
        private int _bulletCount;
        private float _totalTime;

        public bool IsControl { set; get; }

        private void Awake()
        {
            _shootEffect.transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (_isLerp)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _shootDuration)
                {
                    _totalTime -= _shootDuration;
                    Shoot();
                }
                if (IsControl)
                {
                    transform.position = Vector3.LerpUnclamped(transform.position, _fllower.position, Time.deltaTime * 6);
                }
                if (_target != null && _target.gameObject.activeSelf)
                {
                    Vector3 targetDir = _target.position - transform.position;
                    transform.right = Vector3.LerpUnclamped(transform.right, targetDir.normalized, Time.deltaTime * 20);
                }
                else
                {
                    var virus = VirusMrg.Instance.GetRandomVirus();
                    if (virus == null)
                    {
                        _isLerp = false;
                        if (IsControl)
                        {
                            _vice6Weapon.BackToDock(this);
                        }
                    }
                    else
                    {
                        _target = virus.transform;
                    }
                }
            }
        }



        private void Shoot()
        {
            if (_target != null && _target.gameObject.activeSelf)
            {
                if (_bulletCount > 0)
                {
                    _bulletCount--;
                    if (_bulletCount <= 6 && _bulletCount > 3)
                    {
                        _pieces[0].SetActive(false);
                    }
                    else if (_bulletCount <= 3 && _bulletCount >= 1)
                    {
                        _pieces[1].SetActive(false);
                    }
                    else if (_bulletCount == 0)
                    {
                        _pieces[2].SetActive(false);
                        if (IsControl)
                        {
                            _vice6Weapon.BackToDock(this);
                        }
                        _isLerp = false;
                    }
                    var bullet = BulletPools.Instance.Spawn("ViceBullet_6");
                    bullet.transform.position = _shootPos.position;
                    bullet.transform.up = _shootPos.up;
                    bullet.GetComponent<ViceBullet6>().Initi(Random.Range(1100f, 1500f));
                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet6);

                    Sequence sq = DOTween.Sequence();
                    sq.Append(_shootEffect.transform.DOScale(Vector3.one, 0.3f));
                    sq.Append(_shootEffect.transform.DOScale(Vector3.zero, 0.3f));
                }
            }
        }


        public void InitiPiece(VirusVice6Weapon vice6Weapon)
        {
            _vice6Weapon = vice6Weapon;
            for (int i = 0; i < _pieces.Count; i++)
            {
                _pieces[i].SetActive(false);
            }
            _leftWing.transform.localEulerAngles = Vector3.zero;
            _rightWing.transform.localEulerAngles = Vector3.zero;

            _isLerp = false;
            _totalTime = 0;
            _bulletCount = 0;
        }

        public void Initi(Transform target, Transform fllower)
        {
            _fllower = fllower;
            _target = target;

            _bulletCount = 9;
            _isLerp = true;
            IsControl = true;
        }



        public void OpenWings()
        {
            DOVirtual.Float(0, 45f, 0.5f, (t) =>
            {
                _leftWing.transform.localEulerAngles = new Vector3(0, 0, -t);
                _rightWing.transform.localEulerAngles = new Vector3(0, 0, t);
            });
        }

        public void TurnOffWings()
        {
            DOVirtual.Float(45f, 0, 0.5f, (t) =>
            {
                _leftWing.transform.localEulerAngles = new Vector3(0, 0, -t);
                _rightWing.transform.localEulerAngles = new Vector3(0, 0, t);
            });
        }

        public void ChargeEnergy(int index)
        {
            _pieces[index].SetActive(true);
        }


    }
}
