using Events;
using Tool;
using UnityEngine;

public class VirusBulletDamage : MonoBehaviour
{

    private int _damageValue;
    public void Initi(int damageValue)
    {
        _damageValue = damageValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (VirusPlayerDataAdapter.GetShootCoin())
            {
                EventManager.TriggerEvent(new UIVirusAddLevelCoinEvent(transform.position));
                VirusGameDataAdapter.AddLevelCoin(1); 
            }

            var baseVirus = collision.transform.GetComponent<BaseVirus>();
            if (!baseVirus.IsDeath)
                baseVirus.Injured(_damageValue, true);
            var obj = EffectPools.Instance.Spawn("HitEffect");
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one * 1.5f;
            BulletPools.Instance.DeSpawn(gameObject);
        }
    }
   
  
}
