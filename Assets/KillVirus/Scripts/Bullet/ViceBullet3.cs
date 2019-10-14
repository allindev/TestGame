using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet3 : MonoBehaviour
    {

        [SerializeField] private Transform leftPos;
        [SerializeField] private Transform rightPos;

        private float _damageValue;
        public void Initi(float value)
        {
            _damageValue = value;
        }

        public void Show()
        {
            VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet3);
            transform.localScale = new Vector3(1.25f, 1.25f, 1);
            StopAllCoroutines();
            StartCoroutine(DelayShow(0.1f));
            CalculateDamage();
        }



        private void CalculateDamage()
        {
            LayerMask layer = 1 << LayerMask.NameToLayer("Virus");
            List<RaycastHit2D> hit2Ds = new List<RaycastHit2D>();
            List<Vector3> pp = new List<Vector3>();
            List<Transform> tts = new List<Transform>();
            pp.Add(leftPos.position); 
            pp.Add((leftPos.position + rightPos.position) / 2f); 
            pp.Add(leftPos.position);
            for (int i = 0; i < pp.Count; i++)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(pp[i], Vector2.up, 15, layer);
                hit2Ds.AddRange(hits);
            }

            for (int i = 0; i < hit2Ds.Count; i++)
            {
                var item = hit2Ds[i];
                if (!tts.Contains(item.transform))
                {
                    tts.Add(item.transform);
                    if (item.transform != null)
                    {
                        var virus = item.transform.GetComponent<BaseVirus>();
                        if (!virus.IsDeath)
                        {
                            virus.Injured(_damageValue, false);
                        }
                        var effect = EffectPools.Instance.Spawn("ViceBullet3Explosion");
                        effect.transform.position = item.point;
                    }
                }
            }
        }

        private IEnumerator DelayShow(float duration)
        {
            while (true)
            {
                yield return new WaitForSeconds(duration);
                yield return StartCoroutine(Show(0.4f));
                yield break;
            }
        }

        private IEnumerator Show(float duration)
        {
            float totalTime = 0;
            while (true)
            {
                totalTime += Time.deltaTime;
                float t1 = totalTime / duration;
                if (t1 >= 1)
                    t1 = 1;
                transform.localScale = new Vector3(Mathf.LerpUnclamped(1.25f, 0, t1), 1.25f, 1);
                if (totalTime >= duration)
                    yield break;
                yield return null;
            }
        }



    }
}
