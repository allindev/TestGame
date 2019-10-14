using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ViceWeapon
{
    public class VirusVice6Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private List<VirusVice6MiniCanon> _miniCanons;
        [SerializeField] private List<Transform> _dockPoss;
        [SerializeField] private List<Transform> readyPoss;
        [SerializeField] private Transform _leftWing;
        [SerializeField] private Transform _rightWing;

        [SerializeField] private float _chargeEnergyDuration;


        private bool _isEnergyFull;
        private float _totalTime;
        private int _miniIndex;
        private int _curPiece;
        private int _backCount;


        public override void Initi()
        {
            for (int i = 0; i < _miniCanons.Count; i++)
            {
                _miniCanons[i].InitiPiece(this);
                _miniCanons[i].transform.SetParent(i < 2 ? _leftWing : _rightWing);
                _miniCanons[i].transform.localPosition = _dockPoss[i].localPosition;
                _miniCanons[i].transform.localEulerAngles = _dockPoss[i].localEulerAngles;
            }
            _isEnergyFull = false;
            _miniIndex = 0;
            _curPiece = -1;
            _totalTime = 0;
        }

        public override void OnUpdate()
        {
            if(!IsUpdate)
                return;
            if (!_isEnergyFull)
            {
                _totalTime += Time.deltaTime;
                if (_totalTime > _chargeEnergyDuration)
                {
                    _curPiece++;
                    _miniCanons[_miniIndex].ChargeEnergy(_curPiece);
                    if (_curPiece >= 2)
                    {
                        _miniIndex++;
                        _curPiece = -1;
                    }
                    _totalTime -= _chargeEnergyDuration;
                    if (_miniIndex == _miniCanons.Count)
                    {
                        Launch();

                        _isEnergyFull = true;
                        _backCount = 0;
                        _miniIndex = 0;
                        _curPiece = -1;
                    }
                }
            }
        }

        public void BackToDock(VirusVice6MiniCanon canon)
        {
            StartCoroutine(CanonBackToDock(canon));
        }

        public override void ReIniti()
        {
            for (int i = 0; i < _miniCanons.Count; i++)
            {
                var canon = _miniCanons[i];
                canon.IsControl = false;
            }
            StopAllCoroutines();
        }



        private void Launch()
        {
            for (int i = 0; i < _miniCanons.Count; i++)
            {
                var canon = _miniCanons[i];
                Vector3 euler = readyPoss[i].localEulerAngles;
                canon.transform.DOLocalMove(readyPoss[i].localPosition, 0.5f);
                canon.transform.DOLocalRotate(euler, 0.5f).OnComplete(() =>
                {
                    canon.transform.parent = null;
                });
                canon.OpenWings();
            }
            StartCoroutine(DelayCall(0.5f,SeekVirus));
        }

        private void SeekVirus()
        {
            var targetVirus = VirusMrg.Instance.GetRandomVirus();
            for (int i = 0; i < _miniCanons.Count; i++)
            {
                var canon = _miniCanons[i];
                Transform target = targetVirus != null ? targetVirus.transform : null;
                canon.Initi(target, readyPoss[i]);
            }
        }


        private IEnumerator DelayCall(float duration,Action callAction)
        {
            yield return new WaitForSeconds(duration);
            callAction.Invoke();
        }

        private IEnumerator CanonBackToDock(VirusVice6MiniCanon canon)
        {
            int index = _miniCanons.IndexOf(canon);
            bool isLerp = false;
            float lerpDelta = 0;
            while (true)
            {
                Vector3 targetPos = _dockPoss[index].position;
                if (!isLerp)
                {
                    float delta = Time.deltaTime * 10;
                    Vector3 distance = targetPos - canon.transform.position;
                    Vector3 offsetDir = distance.normalized - canon.transform.right;
                    canon.transform.right = (canon.transform.right * 5f + offsetDir * 2).normalized;
                    float dd = Vector3.Dot(canon.transform.right, distance);
                    if (dd * dd < delta * delta && distance.sqrMagnitude < delta * delta)
                    {
                        isLerp = true;
                        canon.transform.right = _dockPoss[index].right;
                        canon.TurnOffWings();
                    }
                    canon.transform.position += canon.transform.right * delta;
                }
                else
                {
                    lerpDelta += Time.deltaTime;
                    if (lerpDelta >= 1)
                    {
                        lerpDelta = 1;
                        canon.transform.parent = index >= 2 ? _rightWing : _leftWing;
                    }
                    canon.transform.right = Vector3.LerpUnclamped(canon.transform.right, _dockPoss[index].right, Time.deltaTime);
                    canon.transform.position = Vector3.LerpUnclamped(canon.transform.position, targetPos, lerpDelta);
                    if (lerpDelta >= 1)
                    {
                        _backCount++;
                        if (_backCount == 4)
                        {
                            _isEnergyFull = false;
                            Initi();
                        }
                        yield break;
                    }
                }
                yield return null;
            }
        }


    }
}
