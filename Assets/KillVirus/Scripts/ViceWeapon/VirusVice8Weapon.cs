using System.Collections;
using UnityEngine;

namespace ViceWeapon
{
    public class VirusVice8Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private VirusVice8MiniCanon _leftMiniCanon;
        [SerializeField] private VirusVice8MiniCanon _rightMiniCanon;

        [SerializeField] private Transform _leftWing;
        [SerializeField] private Transform _rightWing;

        [SerializeField] private float _chargeEnergyDuration;
        [SerializeField] private float _shootDuration;


        private bool _isEnergyFull;
        private bool _isShoot;
        private float _totalTime;

        public override void Initi()
        {
            _isEnergyFull = false;
            _leftWing.transform.localPosition = new Vector3(-0.8f, 0, 0);
            _rightWing.transform.localPosition = new Vector3(0.8f, 0, 0);
            _leftMiniCanon.Initi();
            _rightMiniCanon.Initi();
            _totalTime = 0;
        }

        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            if (!_isEnergyFull)
            {
                _isEnergyFull = true;
                StartCoroutine(FadeOut());
            }
            if (_isShoot)
            {
                _totalTime -= Time.deltaTime;
                if (_totalTime <= 0)
                {
                    StartCoroutine(FadeIn());
                    _isShoot = false;
                }
            }
        }

        public override void ReIniti()
        {
            StopAllCoroutines();
        }


        private IEnumerator FadeOut()
        {
            while (true)
            {
                yield return StartCoroutine(DoDotAlpha());
                _leftMiniCanon.FadeOut();
                _rightMiniCanon.FadeOut();
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet8);
                yield return StartCoroutine(DoMoveX(-1.2f, 1.2f, 0.3f));
                yield return StartCoroutine(DoMoveY(2.5f, 2.5f, 0.6f));
                _totalTime = _shootDuration + 0.6f;
                _isShoot = true;
                yield break;
            }
        }

        private IEnumerator FadeIn()
        {
            _leftMiniCanon.FadeIn();
            _rightMiniCanon.FadeIn();
            while (true)
            {
                yield return new WaitForSeconds(0.6f);
                yield return StartCoroutine(DoMoveY(0, 0, 0.6f));
                yield return StartCoroutine(DoMoveX(-0.8f, 0.8f, 0.3f));
                _isEnergyFull = false;
                yield break;
            }
        }

        private IEnumerator DoMoveY(float leftY, float rightY, float duration)
        {
            Vector3 sleft = _leftWing.localPosition;
            Vector3 sright = _rightWing.localPosition;
            Vector3 left = _leftWing.localPosition;
            Vector3 right = _rightWing.localPosition;

            left = new Vector3(left.x, leftY, left.z);
            right = new Vector3(right.x, rightY, right.z);
            float totalTime = 0;
            while (true)
            {
                totalTime += Time.deltaTime;
                if (totalTime >= duration)
                {
                    _leftWing.localPosition = Vector3.LerpUnclamped(sleft, left, 1);
                    _rightWing.localPosition = Vector3.LerpUnclamped(sright, right, 1);
                    yield break;
                }
                _leftWing.localPosition = Vector3.LerpUnclamped(sleft, left, totalTime / duration);
                _rightWing.localPosition = Vector3.LerpUnclamped(sright, right, totalTime / duration);
                yield return null;
            }
        }

        private IEnumerator DoMoveX(float leftX, float rightX, float duration)
        {
            Vector3 sleft = _leftWing.localPosition;
            Vector3 sright = _rightWing.localPosition;
            Vector3 left = _leftWing.localPosition;
            Vector3 right = _rightWing.localPosition;

            left = new Vector3(leftX, left.y, left.z);
            right = new Vector3(rightX, right.y, right.z);
            float totalTime = 0;
            while (true)
            {
                totalTime += Time.deltaTime;
                if (totalTime >= duration)
                {
                    _leftWing.localPosition = Vector3.LerpUnclamped(sleft,left,1);
                    _rightWing.localPosition = Vector3.LerpUnclamped(sright,right,1);
                    yield break;
                }
                _leftWing.localPosition = Vector3.LerpUnclamped(sleft, left, totalTime / duration);
                _rightWing.localPosition = Vector3.LerpUnclamped(sright, right, totalTime / duration);
                yield return null;
            }
        }

        private IEnumerator DoDotAlpha()
        {
            float totalTime = 0;
            float offset = 100f / _chargeEnergyDuration;
            int index = 1;
            bool isMinus = true;
            while (true)
            {
                totalTime += Time.deltaTime;
                if (totalTime >= _chargeEnergyDuration)
                {
                    yield break;
                }
                float t = totalTime * offset;
                if (t >= index * 10)
                {
                    index++;
                    isMinus = !isMinus;
                }
                float v = isMinus ? (index * 10f - t) / 10f : 1 - (index * 10f - t) / 10f;
                _leftMiniCanon.SetDotAlpha(v);
                _rightMiniCanon.SetDotAlpha(v);
                yield return null;
            }
        }

        
    }
}
