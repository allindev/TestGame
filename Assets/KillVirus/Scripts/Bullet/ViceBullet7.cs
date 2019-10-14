using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet7 : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackRadius;

        private float _damageValue;
        public void Initi(float damageValue)
        {
            _damageValue = damageValue;
        }

        private void Update()
        {
            float delta = Time.deltaTime * 10;
            if (transform.localScale.x + delta >= 1f)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale += new Vector3(delta, delta, 0);
            }

            transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
            if (transform.position.y > 13)
            {
                BulletPools.Instance.DeSpawn(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Virus"))
            {
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet7Explosion);
                var effect = EffectPools.Instance.Spawn("ViceBullet7Explosion");
                effect.transform.position = transform.position;
                CalculateDamage();
                BulletPools.Instance.DeSpawn(gameObject);
            }
        }

        private void CalculateDamage()
        {
            LayerMask layer = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, _attackRadius, layer);
            for (int i = 0; i < coliders.Length; i++)
            {
                var item = coliders[i];
                var virus = item.GetComponent<BaseVirus>();
                if (virus != null && !virus.IsDeath)
                {
                    virus.Injured(_damageValue, false);
                }
                var vm = item.GetComponent<VirusMove>();
                if (vm != null)
                {
                    vm.Palsy();
                }
            }
        }

    }
}
