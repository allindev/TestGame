using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tool.Pools;
using DG.Tweening;
using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice4Weapon : BaseVirusViceWeapon
    {
        [SerializeField] private Transform leftShootPos;
        [SerializeField] private Transform rightShootPos;
        [SerializeField] private float _shootDuration;

        private float _totalTime;
        private bool _isleft;
        private ViceBullet4 _leftBullet4;
        private ViceBullet4 _rightBullet4;


        public override void Initi()
        {
            _totalTime = 0;
            _isleft = false;
            if (_leftBullet4 == null)
                _leftBullet4 = SpawnBullet(leftShootPos);
            if (_rightBullet4 == null)
                _rightBullet4 = SpawnBullet(rightShootPos);

            StopAllCoroutines();
        }


        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                _isleft = !_isleft;
                if (_isleft)
                {
                    Transform p = VirusMrg.Instance.GetTargetVirus().transform;
                    if (p != null)
                    {
                        _leftBullet4.Emit(p);
                        _leftBullet4 = null;
                        StartCoroutine(DealyCall(2.0f, () => { _leftBullet4 = SpawnBullet(leftShootPos); }));
                    }
                }
                else
                {
                    Transform p = VirusMrg.Instance.GetTargetVirus().transform;
                    if (p != null)
                    {
                        _rightBullet4.Emit(p);
                        _rightBullet4 = null;
                        StartCoroutine(DealyCall(2.0f, () => { _rightBullet4 = SpawnBullet(rightShootPos); }));
                    }
                }
            }
        }


        public override void ReIniti()
        {
           
        }



        private ViceBullet4 SpawnBullet(Transform parent)
        {
            var obj = BulletPools.Instance.Spawn("ViceBullet_4");
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            var vb = obj.GetComponent<ViceBullet4>();
            vb.Tail.SetActive(false);
            vb.Initi(18000f);
            return vb;
        }

        private IEnumerator DealyCall(float duation, Action callAction)
        {
            while (true)
            {
                yield return new WaitForSeconds(duation);
                callAction.Invoke();
                yield break;
            }
        }


    }
}
