using System;
using DG.Tweening;
using UnityEngine;

public class VirusPlayerShootEffect : MonoBehaviour
{

    [SerializeField] private GameObject _shootCoin;
    [SerializeField] private GameObject _shootCircle;

    [SerializeField] private GameObject _reviveObj;

    private ShootEffectEnum _shootEffectEnum;
    private int _num;
    private bool _isShoot;

    public bool IsShoot
    {
        set
        {
            _isShoot = value;
            gameObject.SetActive(_isShoot);
        } 
        get { return _isShoot; }
    }


    private void Awake()
    {
        _reviveObj.SetActive(false);
    }


    public void OnUpdate()
    {
        if (_shootEffectEnum == ShootEffectEnum.Normal)
        {
            _num++;
            if (_num > 5)
            {
                _num -= 5;
            }
            _shootCircle.transform.localScale = new Vector3(_num / 4f, _num / 4f, 1);
        } 
    }


    public void InitiShootEffect(ShootEffectEnum shootEffectEnum)
    {
        _shootEffectEnum = shootEffectEnum;
        if (_shootEffectEnum == ShootEffectEnum.Normal)
        {
            _num = 0;
            _shootCoin.SetActive(false);
            _shootCircle.SetActive(true);
        }
        else if (_shootEffectEnum == ShootEffectEnum.Coin)
        {
            _shootCoin.SetActive(true);
            _shootCircle.SetActive(false);
        }
    }


    public void Flash(Action callAction)
    {
        _reviveObj.SetActive(true);
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(3.0f);
        sq.AppendCallback(() => { _reviveObj.SetActive(false); });
        sq.AppendCallback(callAction.Invoke);
    }



}