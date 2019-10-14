using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ViceWeapon
{
    public class VirusVice5MiniCanon : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _pieces;
        [SerializeField] private GameObject _leftWing;
        [SerializeField] private GameObject _rightWing;
        [SerializeField] private Transform _shootPos;
        [SerializeField] private GameObject _shootEffect;

        [SerializeField] private float _shootDuration;


        private Transform _target;
        private VirusVice5Weapon _vice5Weapon;

        private bool _isStartShoot;
        private bool _isFllow;
        private bool _isLerp;
       
        private int _bulletCount;
        private float _totalTime;
        private float _angle;
        private float _lerpDelta;
        private float _radius;


        public bool IsControl { set; get; }

        private void Awake()
        {
            _shootEffect.transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (_isStartShoot)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _shootDuration)
                {
                    Shoot();
                    _totalTime -= _shootDuration;
                }
            }
            if (_isFllow)
            {
                FollowVirus();
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
                        _isStartShoot = false;
                        _isFllow = false;
                        if (IsControl)
                        {
                            _vice5Weapon.BackToDock(this);
                        }
                    }
                    var bullet = BulletPools.Instance.Spawn("ViceBullet_5");
                    bullet.transform.position = _shootPos.position;
                    bullet.transform.up = _shootPos.up;
                    bullet.GetComponent<ViceBullet5>().Initi(Random.Range(460f, 500f));
                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet6);

                    Sequence sq = DOTween.Sequence();
                    sq.Append(_shootEffect.transform.DOScale(Vector3.one, 0.3f));
                    sq.Append(_shootEffect.transform.DOScale(Vector3.zero, 0.3f));
                }
            }
            else
            {
                _isStartShoot = false;
            }
        }

        private void FollowVirus()
        {
            if (_target != null && _target.gameObject.activeSelf)
            {
                Vector3 dir = Quaternion.Euler(0, 0, _angle) * Vector3.left * (_radius + 1.5f);
                Vector3 targetPos = _target.position + dir;
                if (!_isLerp)
                {
                    float delta = Time.deltaTime * 10;
                    Vector3 distance = targetPos - transform.position;
                    Vector3 offsetDir = distance.normalized - transform.right;
                    transform.right = (transform.right * 5f + offsetDir.normalized * 1).normalized;
                    float dd = Vector3.Dot(transform.right, distance);
                    if (dd * dd <= delta * delta && distance.sqrMagnitude <= delta * delta)
                    {
                        _isLerp = true;
                        transform.right = -dir;
                        return;
                    }
                    transform.position += transform.right * delta;
                }
                else
                {
                    _lerpDelta += Time.deltaTime * 4f;
                    if (_lerpDelta >= 1)
                    {
                        _lerpDelta = 1;
                        _isStartShoot = true;
                    }
                    transform.right = Vector3.LerpUnclamped(transform.right, -dir, Time.deltaTime);
                    transform.position = Vector3.LerpUnclamped(transform.position, targetPos, _lerpDelta);
                }
            }
            else
            {
                var virus = VirusMrg.Instance.GetRandomVirus();
                if (virus == null)
                {
                    _isStartShoot = false;
                    _isFllow = false;
                    if (IsControl)
                    {
                        _vice5Weapon.BackToDock(this);
                    }
                }
                else
                {
                    _target = virus.transform;
                    _isLerp = false;
                    _lerpDelta = 0;
                }
            }
        }



        public void Initi(Transform target, float angle)
        {
            _isFllow = true;
            _isStartShoot = false;
            _isLerp = false;

            _target = target;
            _bulletCount = 9;

            _angle = angle;
            _radius = target.GetComponent<VirusMove>().Radius;
            _lerpDelta = 0;

            IsControl = true;
        }

        public void InitiPiece(VirusVice5Weapon vice5Weapon)
        {
            _vice5Weapon = vice5Weapon;
            for (int i = 0; i < _pieces.Count; i++)
            {
                _pieces[i].SetActive(false);
            }
            _leftWing.transform.localEulerAngles = Vector3.zero;
            _rightWing.transform.localEulerAngles = Vector3.zero;

            _isFllow = false;
            _isStartShoot = false;
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
