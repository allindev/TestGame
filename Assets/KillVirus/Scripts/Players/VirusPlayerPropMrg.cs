using System;
using Events;
using Tool;
using UnityEngine;

public class VirusPlayerPropMrg : MonoBehaviour,IEventListener<VirusPropAddEvent>

{

    [SerializeField] private VirusPlayerShootEffect _shootEffect;

    private VirusPlayer _player;

    private void Awake()
    {
        _player = transform.GetComponent<VirusPlayer>();
    }

    private void OnEnable()
    {
        EventRegister.EventStartListening<VirusPropAddEvent>(this);
    }

    private void OnDisable()
    {
        EventRegister.EventStopListening<VirusPropAddEvent>(this);
    }

    public void OnEvent(VirusPropAddEvent eventType)
    {
        switch (eventType.PropEnum)
        {
            case VirusPropEnum.Big:
                AddBigProp(eventType.Duration);
                break;
            case VirusPropEnum.Active:
                AddActiveProp(eventType.Duration);
                break;
            case VirusPropEnum.Weaken:
                AddWeakenProp(eventType.Duration);
                break;
            case VirusPropEnum.ReinforceShootSpeed:
                AddShootNumProp(eventType.Duration);
                break;
            case VirusPropEnum.ReinforceShootPower:
                AddShootPowerProp(eventType.Duration);
                break;
            case VirusPropEnum.CallFriend:
                AddCallFriendProp(eventType.Duration);
                break;
            case VirusPropEnum.LimitMove:
                AddLimitMoveProp(eventType.Duration);
                break;
            case VirusPropEnum.ShootCoin:
                AddShootCoinProp(eventType.Duration);
                break;
            case VirusPropEnum.ShootRepulse:
                AddShootRepulseProp(eventType.Duration);
                break;
        }
    }


    private void AddBigProp(float duration)
    {
        string str = VirusPropEnum.Big.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.Big, t, duration);
        };
        Action callAction = () =>
        {
            VirusMrg.Instance.RecoverBiggerVirus();
            _player.RemovePropItem(VirusPropEnum.Big);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusMrg.Instance.StartBiggerVirus();
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.Big); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddActiveProp(float duration)
    {
        string str = VirusPropEnum.Active.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.Active, t, duration);
        };
        Action callAction = () =>
        {
            VirusMrg.Instance.UnActiveVirus();
            _player.RemovePropItem(VirusPropEnum.Active);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusMrg.Instance.ActiveVirus();
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.Active); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddWeakenProp(float duration)
    {
        string str = VirusPropEnum.Weaken.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.Weaken, t, duration);
        };
        Action callAction = () =>
        {
            VirusMrg.Instance.NotWeakenVirus();
            _player.RemovePropItem(VirusPropEnum.Weaken);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusMrg.Instance.WeakenVirus();
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.Weaken); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddShootNumProp(float duration)
    {
        string str = VirusPropEnum.ReinforceShootSpeed.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.ReinforceShootSpeed, t, duration);
        };
        Action callAction = () =>
        {
            VirusPlayerDataAdapter.MulHalfShootNum();
            _player.RemovePropItem(VirusPropEnum.ReinforceShootSpeed);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusPlayerDataAdapter.MulShootNum(2);
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.ReinforceShootSpeed); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddShootPowerProp(float duration)
    {
        string str = VirusPropEnum.ReinforceShootPower.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.ReinforceShootPower, t, duration);
        };
        Action callAction = () =>
        {
            VirusPlayerDataAdapter.MulHalfShootPower();
            _player.RemovePropItem(VirusPropEnum.ReinforceShootPower);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusPlayerDataAdapter.MulShootPower(2);
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.ReinforceShootPower); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddLimitMoveProp(float duration)
    {
        string str = VirusPropEnum.LimitMove.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.LimitMove, t, duration);
        };
        Action callAction = () =>
        {
            _player.Recover();
            _player.RemovePropItem(VirusPropEnum.LimitMove);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            _player.Decelerate();
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.LimitMove); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddCallFriendProp(float duration)
    {
        string str = VirusPropEnum.CallFriend.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.CallFriend, t, duration);
        };
        Action callAction = () =>
        {
            VirusGameMrg.Instance.RemoveFriend();
            _player.RemovePropItem(VirusPropEnum.CallFriend);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusGameMrg.Instance.AddFriend();
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.CallFriend); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddShootCoinProp(float duration)
    {
        string str = VirusPropEnum.ShootCoin.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.ShootCoin, t, duration);
        };
        Action callAction = () =>
        {
            _shootEffect.InitiShootEffect(ShootEffectEnum.Normal);
            VirusPlayerDataAdapter.SetIsShootCoin(false);
            _player.RemovePropItem(VirusPropEnum.ShootCoin);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusPlayerDataAdapter.SetIsShootCoin(true);
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () =>
        {
            _shootEffect.InitiShootEffect(ShootEffectEnum.Coin);
            _player.AddPropItem(VirusPropEnum.ShootCoin);
        });
        TimerManager.Instance.AddTimer(str, timer);
    }

    private void AddShootRepulseProp(float duration)
    {
        string str = VirusPropEnum.ShootRepulse.ToString();
        Action<float> updateAction = t =>
        {
            _player.UpdatePropItem(VirusPropEnum.ShootRepulse, t, duration);
        };
        Action callAction = () =>
        {
            VirusPlayerDataAdapter.SetIsRepulse(false);
            _player.RemovePropItem(VirusPropEnum.ShootRepulse);
            TimerManager.Instance.RemoveTimer(str);
        };
        Action initiAction = () =>
        {
            VirusPlayerDataAdapter.SetIsRepulse(true);
        };

        Timer timer = new Timer(duration, updateAction, callAction, initiAction, () => { _player.AddPropItem(VirusPropEnum.ShootRepulse); });
        TimerManager.Instance.AddTimer(str, timer);
    }

    


}