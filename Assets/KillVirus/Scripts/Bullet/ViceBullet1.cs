using DG.Tweening;
using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet1 : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed;

        private bool _isEmit;
        private bool _isReady;
        private bool _isReverse;
        private float _damageValue;
        private int _num;

        private Vector3 _origin;
        private Vector3 _end;


        public void Initi(float damageValue)
        {
            _damageValue = damageValue;
        }

        public void Initi()
        {
            _isEmit = false;
            transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            transform.DOScale(new Vector3(0.25f, 0.25f, 1f), 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _isReady = true;
            });
            _isReverse = false;
            _origin = new Vector3(0.25f, 0.25f, 1f);
            _end = new Vector3(0.28f, 0.28f, 1);
        }

        public void Emit()
        {
            _isEmit = true;
            _isReady = false;
            transform.parent = null;
        }


        private void Update()
        {
            if (_isReady)
            {
                _num++;
                if (_num >= 5)
                {
                    _num = 0;
                    _isReverse = !_isReverse;
                    if (_isReverse)
                    {
                        _origin = new Vector3(0.28f, 0.28f, 1);
                        _end = new Vector3(0.25f, 0.25f, 1f);
                    }
                    else
                    {
                        _origin = new Vector3(0.25f, 0.25f, 1f);
                        _end = new Vector3(0.28f, 0.28f, 1);
                    }
                }
                transform.localScale = Vector3.LerpUnclamped(_origin, _end, _num * 1.0f / 5f);
            }
            if (_isEmit)
            {
                float delta = Time.deltaTime * 1;
                if (transform.localScale.x + delta > 0.55f)
                    transform.localScale = new Vector3(0.55f, 0.55f, 1);
                else
                    transform.localScale += new Vector3(delta, delta, 1);

                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                if (transform.position.y > 12f)
                {
                    BulletPools.Instance.DeSpawn(gameObject);
                }
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Virus") && _isEmit)
            {
                var basevirus = collision.GetComponent<BaseVirus>();
                if (basevirus.VirusHealth.Value >= _damageValue)
                {
                    if (!basevirus.IsDeath)
                    {
                        basevirus.Injured(_damageValue, false);
                    }
                    BulletPools.Instance.DeSpawn(gameObject);
                }
                else
                {
                    _damageValue -= basevirus.VirusHealth.Value;
                    if (!basevirus.IsDeath)
                    {
                        basevirus.Injured(basevirus.VirusHealth.Value, false);
                    }
                }
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet1Explosion);
            }
        }

       

    }
}
