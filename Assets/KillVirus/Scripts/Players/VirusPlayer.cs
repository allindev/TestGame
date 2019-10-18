using System;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using Tool;
using UI;
using UnityEngine;
using ViceWeapon;

public class VirusPlayer : MonoBehaviour
{

    [SerializeField] private VirusPlayerShoot _playerShoot;
    [SerializeField] private VirusPlayerMove _playerMove;
    [SerializeField] private UIVirusPlayerView _playerView;
    [SerializeField] private VirusPlayerShootEffect _shootEffect;
    [SerializeField] private VirusPlayerPropCheck _propCheck;
    [SerializeField] private AudioSource _shootAudioSource;

    [SerializeField] private Transform _slowTransform;
    [SerializeField] private List<GameObject> _weaponList;
    [SerializeField] private GameObject _upgradeEffect;
   

    public Transform SlowTransform { get { return _slowTransform; } }

    public BaseVirusViceWeapon CurViceWeapon { get { return _curViceWeapon; } }

    public bool Invincible
    {
        set { _invincible = value; }
        get { return _invincible; }
    }


    private bool _invincible;
    private BaseVirusViceWeapon _curViceWeapon;
    private List<BaseVirus> _forceViruses; 
    private void Awake()
    {
        _upgradeEffect.SetActive(false);
        _playerShoot.OnAwake();
        _playerMove.OnAwake();
        _playerView.OnAwake();
    }

    private void Start()
    {
        _invincible = false;
        VirusPlayerDataAdapter.Load();
        _shootAudioSource.Stop();
        _shootEffect.InitiShootEffect(ShootEffectEnum.Normal);
        foreach (GameObject t in _weaponList)
        {
            t.SetActive(false);
        }
        InitiViceWeapon(0);
        _forceViruses = new List<BaseVirus>();
    }

    private void Update()
    {
        _playerShoot.OnUpdate();
        _playerMove.OnUpdate();
        _shootEffect.OnUpdate();
        if (_curViceWeapon != null)
        {
            _curViceWeapon.OnUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Virus"))
        {
            if (!_invincible)
            {
                EventManager.TriggerEvent(new VirusGameStateEvent(VirusGameState.GameOver));
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.PlayerDeath);
            }
        }
    }



    public void Revive()
    {
        _forceViruses = new List<BaseVirus>();
        _invincible = true;
        _shootEffect.Flash(() => { _invincible = false; });
    }

    public void Upgrade()
    {
        _upgradeEffect.SetActive(true);
        DOVirtual.DelayedCall(0.75f, () => { _upgradeEffect.SetActive(false); });
    }

    public void SetPlayerState(bool isShoot, bool isControl)
    {
        _playerShoot.IsStartShoot = isShoot;
        _playerMove.OnAwake();
        _playerMove.IsControl = isControl;
        _propCheck.IsControl = isControl;
        _shootAudioSource.enabled = isShoot;
        if (isShoot)
        {
            _shootAudioSource.Play();
        }
       
        _shootEffect.IsShoot = isShoot;
        if (_curViceWeapon != null)
        {
            _curViceWeapon.IsUpdate = isShoot;
            _curViceWeapon.Initi();
        }
    }

    public void InitiViceWeapon(int index)
    {
        if (index >= _weaponList.Count) return;

        if (index >= 0)
        {
            var preIndex = VirusPlayerDataAdapter.GetCurWeaponLevel();
            var weapon = _weaponList[index];
            if (weapon != null)
            {
                weapon.SetActive(true);
                _curViceWeapon = weapon.GetComponent<BaseVirusViceWeapon>();
                if (_curViceWeapon != null)
                {
                    _curViceWeapon.Initi();
                }
            }
            if (index > 0)
            {
                 _weaponList[index].GetComponent<ViceWeaponEffect>().Ready();
                Sequence sq = DOTween.Sequence();
                sq.AppendCallback(() =>
                {
                    _weaponList[preIndex].GetComponent<ViceWeaponEffect>().FadeIn();
                });
                sq.AppendInterval(1.0f);
                sq.AppendCallback(() =>
                {
                    _weaponList[index].GetComponent<ViceWeaponEffect>().FadeOut();
                    VirusSoundMrg.Instance.PlaySound(VirusSoundType.Unlock);
                });
                sq.AppendInterval(1.0f);
                sq.AppendCallback(() => { _weaponList[preIndex].SetActive(false); });
            }

            VirusPlayerDataAdapter.SetCurWeaponLevel(index);
        }
    }

    public void AddDecelerate(BaseVirus baseVirus)
    {
        if (!_forceViruses.Contains(baseVirus))
        {
            _playerMove.Decelerate();
            _forceViruses.Add(baseVirus);
        }
    }

    public void RemoveDecelerate(BaseVirus baseVirus)
    {
        if (_forceViruses.Contains(baseVirus))
            _forceViruses.Remove(baseVirus);
        if (_forceViruses.Count == 0)
            _playerMove.Recover();
    }

    public void Recover()
    {
        _playerMove.Recover();
    }

    public void Decelerate()
    {
        _playerMove.Decelerate();
    }
   


    public void AddPropItem(VirusPropEnum propEnum)
    {
        _playerView.Add(propEnum);
    }

    public void RemovePropItem(VirusPropEnum propEnum)
    {
        _playerView.Remove(propEnum);
    }

    public void UpdatePropItem(VirusPropEnum propEnum, float curTime, float totalTime)
    {
        _playerView.UpdatePropItem(propEnum, curTime / totalTime);
    }


}