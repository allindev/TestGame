using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ViceWeapon
{
    public class VirusVice8MiniCanon : MonoBehaviour
    {

        [SerializeField] private Animator leafAnimator;

        [SerializeField] private SpriteRenderer _dotRenderer;
        [SerializeField] private SpriteRenderer _leafRenderer;

        [SerializeField] private Transform leafTransform;
        [SerializeField] private Transform dartTransform;
        [SerializeField] private List<Transform> dartLeafTransform;

        [SerializeField] private float leafRotateSpeed;
        [SerializeField] private float dartRotateSpeed;


        private float _curLeafSpeed;
        private float _curDartSpeed;

        private bool _isFadeOut;

        private void Update()
        {
            if (_isFadeOut)
            {
                _curLeafSpeed += Time.deltaTime * leafRotateSpeed;
                _curDartSpeed += Time.deltaTime * dartRotateSpeed;

                if (_curLeafSpeed >= leafRotateSpeed)
                    _curLeafSpeed = leafRotateSpeed;
                if (_curDartSpeed >= dartRotateSpeed)
                    _curDartSpeed = dartRotateSpeed;

                leafTransform.transform.localEulerAngles += new Vector3(0, 0, _curLeafSpeed * Time.deltaTime);
                dartTransform.transform.localEulerAngles += new Vector3(0, 0, _curDartSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Virus") && _isFadeOut)
            {
                var baseVirus = collision.GetComponent<BaseVirus>();
                if (!baseVirus.IsDeath)
                {
                    baseVirus.Injured(Random.Range(27000f, 30000f), false);
                }
            }
        }

        


        public void Initi()
        {
            for (int i = 0; i < dartLeafTransform.Count; i++)
            {
                dartLeafTransform[i].localScale = Vector3.zero;
                dartLeafTransform[i].localPosition = Vector3.zero;
            }

            leafTransform.gameObject.SetActive(false);
            _leafRenderer.color = new Color(1, 1, 1, 0);
            _isFadeOut = false;
            StopAllCoroutines();
        }

        public void FadeOut()
        {
            _curLeafSpeed = 0;
            _curDartSpeed = 0;
            leafTransform.gameObject.SetActive(true);
            if (leafAnimator.isActiveAndEnabled)
                leafAnimator.SetTrigger("FadeOut");
            StartCoroutine(FadeOut(0.6f));
        }

        public void FadeIn()
        {
            if (leafAnimator.isActiveAndEnabled)
                leafAnimator.SetTrigger("FadeIn");
            StartCoroutine(FadeIn(0.6f));
        }


        private IEnumerator FadeOut(float duration)
        {
            float totalTime = 0;
            List<Vector3> startPos = dartLeafTransform.Select(t => t.localPosition).ToList();
            List<Vector3> endPos = new List<Vector3>();
            for (int i = 0; i < 4; i++)
            {
                endPos.Add(Quaternion.Euler(0, 0, i * 90) * Vector3.right * 1.65f);
            }
            while (true)
            {
                totalTime += Time.deltaTime;
                float t1 = totalTime / duration;
                if (t1 >= 1)
                    t1 = 1;
                for (int i = 0; i < dartLeafTransform.Count; i++)
                {
                    var t = dartLeafTransform[i];
                    t.transform.localPosition = Vector3.LerpUnclamped(startPos[i], endPos[i], t1);
                    float scale = Mathf.LerpUnclamped(0, 3.5f, t1);
                    t.transform.localScale = new Vector3(scale, scale, 1);
                }
                _leafRenderer.color = new Color(1, 1, 1, Mathf.LerpUnclamped(0, 1, t1));
                if (totalTime > duration)
                {
                    _isFadeOut = true;
                    yield break;
                }
                yield return null;
            }
        }

        private IEnumerator FadeIn(float duration)
        {
            float totalTime = 0;
            List<Vector3> startPos = dartLeafTransform.Select(t => t.localPosition).ToList();
            List<Vector3> endPos = new List<Vector3>(4) { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
            while (true)
            {
                totalTime += Time.deltaTime;
                float t1 = totalTime / duration;
                if (t1 >= 1)
                    t1 = 1;
                _curLeafSpeed = Mathf.LerpUnclamped(leafRotateSpeed, 0, t1);
                _curDartSpeed = Mathf.LerpUnclamped(dartRotateSpeed, 0, t1);
                _leafRenderer.color = new Color(1, 1, 1, Mathf.LerpUnclamped(1, 0, t1));
                for (int i = 0; i < dartLeafTransform.Count; i++)
                {
                    var t = dartLeafTransform[i];
                    t.transform.localPosition = Vector3.LerpUnclamped(startPos[i], endPos[i], t1);
                    float scale = Mathf.LerpUnclamped(3.5f, 0, t1);
                    t.transform.localScale = new Vector3(scale, scale, 1);
                }
                if (totalTime > duration)
                {
                    _isFadeOut = false;
                    yield break;
                }
                yield return null;
            }
        }

        public void SetDotAlpha(float alpha)
        {
            _dotRenderer.color = new Color(1, 1, 1, alpha);
        }


    }
}
