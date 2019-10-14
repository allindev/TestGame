using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet4 : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed;
        [SerializeField] private GameObject _tail;

        private Transform _target;
        private Vector3 _moveDir;
        private float _damageValue;
        private bool _isEmit;

        public GameObject Tail { get { return _tail; } }


        public void Initi(float value)
        {
            _damageValue = value;
        }

        public void Emit(Transform target)
        {
            _moveDir = Vector3.up;
            if (target != null)
                _target = target;

            transform.parent = null;
            _isEmit = true;
            _tail.SetActive(true);
        }

        private void Update()
        {
            if (_isEmit)
            {
                if (_target != null && _target.gameObject.activeSelf)
                {
                    Vector3 targetDir = (_target.position - transform.position).normalized;
                    _moveDir = Vector3.LerpUnclamped(_moveDir, targetDir, Time.deltaTime * 2);
                }
                transform.position += _moveSpeed * _moveDir * Time.deltaTime;
                transform.up = _moveDir;
                BorderCheck();
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isEmit)
            {
                if (collision.transform.CompareTag("Virus"))
                {
                    var virus = collision.transform.GetComponent<BaseVirus>();
                    if (!virus.IsDeath)
                    {
                        virus.Injured(_damageValue, false);
                    }
                    CalculateDamage(collision.transform);

                    BulletPools.Instance.DeSpawn(gameObject);
                    var explosion = EffectPools.Instance.Spawn("ViceBullet4Explosion");
                    explosion.transform.position = transform.position;
                    explosion.transform.localScale = new Vector3(2.5f, 2.5f, 1);
                    explosion.transform.localEulerAngles = Vector3.zero;

                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet4Explosion);
                    _isEmit = false;
                }
            }
        }


        private void CalculateDamage(Transform virus)
        {
            LayerMask layer = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, 2f, layer);
            for (int i = 0; i < coliders.Length; i++)
            {
                if (coliders[i].transform != virus)
                {
                    var baseVirus = coliders[i].GetComponent<BaseVirus>();
                    if (!baseVirus.IsDeath)
                    {
                        baseVirus.Injured(_damageValue * 0.5f, false);
                    }
                }
            }
        }


        private void BorderCheck()
        {
            bool b1 = transform.position.x > VirusTool.RightX + 2f;
            bool b2 = transform.position.x < VirusTool.LeftX - 2f;
            bool b3 = transform.position.y > VirusTool.TopY + 2f;
            bool b4 = transform.position.y < VirusTool.BottomY - 2f;
            if (b1 || b2 || b3 || b4)
            {
                BulletPools.Instance.DeSpawn(gameObject);
                _isEmit = false;
            }
        }


    }
}
