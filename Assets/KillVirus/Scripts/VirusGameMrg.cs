using System.Collections.Generic;
using DG.Tweening;
using Events;
using KLWStateMachine;
using Tool;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusGameMrg : Singleton<VirusGameMrg>, IEventListener<VirusGameStateEvent>,
                                                     IEventListener<FirstByteClick5DownEvent>

{

    [SerializeField] private UIMrg _uiMrg;
    [SerializeField] private VirusPlayer _virusPlayer;
    [SerializeField] private VirusFriend _virusFriend;
    [SerializeField] private VirusMrg virusMrg;

    public VirusPlayer VirusPlayer { get { return _virusPlayer; } }


    private StateMachine<VirusGameState> _fsm;
    private bool _firstIn;
    private bool _isGetAward;
    private bool _isClickSpace;
    private bool _isSettle;
    private float _gameOverTime;


    private void Awake()
    {
        _fsm = StateMachine<VirusGameState>.Initialize(this, VirusGameState.None);
        _firstIn = true;
        _isSettle = false;
    }

    private void Start()
    {
        VirusGameDataAdapter.Load();
        _fsm.ChangeState(VirusGameState.Load);
        _uiMrg.SettlePanel.UnActive();
        _uiMrg.RestartPanel.UnActive();

        virusMrg.BindView(_uiMrg.MainPanel);
    }
   

    private void OnEnable()
    {
        EventRegister.EventStartListening<VirusGameStateEvent>(this);
        EventRegister.EventStartListening<FirstByteClick5DownEvent>(this);
    }

    private void OnDisable()
    {
        EventRegister.EventStopListening<VirusGameStateEvent>(this);
        EventRegister.EventStopListening<FirstByteClick5DownEvent>(this);
    }
   
   


 

    public void AddFriend()
    {
        _virusFriend.FllowTarget(_virusPlayer.GetComponent<VirusPlayerMove>());
    }

    public void RemoveFriend()
    {
        _virusFriend.NotFllowTarget();
    }

    public void OnEvent(VirusGameStateEvent eventType)
    {
        if (eventType.GameState == VirusGameState.Settle)
        {
            _fsm.ChangeState(VirusGameState.Settle);
        }
        if (eventType.GameState == VirusGameState.GameOver)
        {
            _fsm.ChangeState(VirusGameState.GameOver);
        }
    }

    public void OnEvent(FirstByteClick5DownEvent eventType)
    {
        _isClickSpace = true;
    }
    




    [FSM("Load", FSMActionName.Enter)]
    private void OnLoadingEnter()
    {
        _uiMrg.FadeIn();
        DOVirtual.Float(0, 10, 2.0f, (t) =>
        {
            _uiMrg.TitlePanel.SetLoadingBar(t / 10f);

        }).OnComplete(() => { _fsm.ChangeState(VirusGameState.ShowTitle); });
    }


    [FSM("ShowTitle", FSMActionName.Enter)]
    private void OnShowTitleEnter()
    {
        Sequence sq = DOTween.Sequence();
        if (!_firstIn)
            _uiMrg.FadeIn();
        VirusPlayer.SetPlayerState(false, false);
        _virusPlayer.transform.position = new Vector3(0, -15f, 0);
        sq.Append(VirusPlayer.transform.DOLocalMoveY(-5f, 0.6f));
        sq.AppendInterval(1.0f);
        sq.AppendCallback(() => { _fsm.ChangeState(VirusGameState.Upgrade); });
        _firstIn = false;
        _isGetAward = false;
    }


    [FSM("Upgrade", FSMActionName.Enter)]
    private void OnUpgradeEnter()
    {
        bool isUpdgrade = false;
        int coin = VirusGameDataAdapter.GetTotalCoin();
        int needCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetWeaponLevel());
        while (true)
        {
            if (coin >= needCoin)
            {
                isUpdgrade = true;
                int value = VirusPlayerDataAdapter.GetUpgradeValue();
                VirusGameDataAdapter.MinusTotalCoin(needCoin);
                VirusPlayerDataAdapter.AddWeaponLevel();
                VirusPlayerDataAdapter.AddShootPower(value);
                VirusPlayerDataAdapter.AddShootSpeed();

                bool b1 = VirusPlayerDataAdapter.GetShootNum() <= 8;
                bool b2 = VirusPlayerDataAdapter.UpgradeShoot();
                if (b1 && b2)
                {
                    VirusPlayerDataAdapter.AddShootNum(1);
                }

                coin = VirusGameDataAdapter.GetTotalCoin();
                needCoin = VirusTool.GetUpgradeCoin(VirusPlayerDataAdapter.GetWeaponLevel());
            }
            else
            {
                break;
            }
        }

        string tipStr = ""; 
        if (isUpdgrade)
        {
            _uiMrg.CoinPanel.SetCoinText();
            _virusPlayer.Upgrade();
            VirusSoundMrg.Instance.PlaySound(VirusSoundType.UpgradeGun);
            tipStr = "火力升级";
        }
        float delayTime = 2f;
        int level = VirusTool.UnlockViceWeapon(VirusGameDataAdapter.GetLevel());
        if (level > 0)
        {
            delayTime = 5f;
            _virusPlayer.InitiViceWeapon(level);
            tipStr = "装备升级";
        }
        if (!string.IsNullOrEmpty(tipStr))
        {
            var tip = EffectPools.Instance.Spawn("FloatTip");
            tip.transform.position = _virusPlayer.transform.position;
            tip.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            tip.GetComponent<FastlaneFloatTip>().Float(tipStr, () => { EffectPools.Instance.DeSpawn(tip); });
        }

        DOVirtual.DelayedCall(delayTime, _uiMrg.FadeOut).OnComplete(() =>
        {
            _fsm.ChangeState(VirusGameState.GamePlay);
        });
    }


    [FSM("GamePlay", FSMActionName.Enter)]
    private void OnPlayEnter()
    {
        Debug.LogError("Level:" + VirusGameDataAdapter.GetLevel());
        _uiMrg.MainPanel.Active();
        _virusPlayer.Invincible = false;
        _virusPlayer.SetPlayerState(true, true);
        virusMrg.GameStart();
    }


    [FSM("Settle", FSMActionName.Enter)]
    private void OnSettleEnter()
    {
        ScenePropMrg.Instance.RemoveAll();
        TimerManager.Instance.FinishTimers();
        VirusGameDataAdapter.AddLevel();

        float y = 15f - _virusPlayer.transform.localPosition.y;
        _virusPlayer.SetPlayerState(false, false);
        _virusPlayer.Invincible = true;

        Sequence sq = DOTween.Sequence();
        sq.Append(_virusPlayer.transform.DOMoveY(15f, y / 15f));
        sq.AppendInterval(1.0f);
        sq.AppendCallback(() =>
        {
            _uiMrg.MainPanel.UnActive(); 
        });
        sq.AppendInterval(1.0f);
        sq.AppendCallback(() =>
        {
            VirusSoundMrg.Instance.PlaySound(VirusSoundType.Clear);
            _uiMrg.SettlePanel.Active();
            _uiMrg.SettlePanel.SetCoinText(VirusGameDataAdapter.GetCurLevelCoin());
            _uiMrg.CoinPanel.Active();
        });
        sq.AppendInterval(0.5f);
        sq.AppendCallback(() => { _isSettle = true; });
    }


    [FSM("Settle", FSMActionName.Update)]
    private void OnSettleUpdate()
    {
        if (_isClickSpace && !_isGetAward && _isSettle)
        {
            Sequence sq = DOTween.Sequence();
            _isGetAward = true;
            _isClickSpace = false;
            _isSettle = false;

            if (VirusGameDataAdapter.GetCurLevelCoin() > 0)
            {
                int count = Random.Range(10, 20);
                Vector3 pos = new Vector2(0f, -96f);
                float offset = 360f / count;
                for (int i = 0; i < count; i++)
                {
                    float angle = offset * i - 180;
                    bool isP = angle > -45f && angle < 135;
                    float radius = Random.Range(200f, 250f);
                    Vector2 uiPos = pos + Quaternion.Euler(0, 0, angle) * Vector2.right * radius;
                    EventManager.TriggerEvent(new UIVirusAddTotalCoinEvent(uiPos, isP));
                }
                VirusGameDataAdapter.AddTotalCoin(VirusGameDataAdapter.GetCurLevelCoin());
                VirusGameDataAdapter.ResetLevelCoin();
                sq.AppendCallback(() => { _uiMrg.SettlePanel.UnActive(); });
                sq.AppendInterval(1.5f);
                sq.AppendCallback(() => { _fsm.ChangeState(VirusGameState.ShowTitle); });
                return;
            }
            sq.AppendCallback(() => { _uiMrg.SettlePanel.UnActive(); });
            sq.AppendInterval(0.5f);
            sq.AppendCallback(() => { _fsm.ChangeState(VirusGameState.ShowTitle); });
            
        }
    }


    [FSM("GameOver", FSMActionName.Enter)]
    private void OnGameOverEnter()
    {
        _uiMrg.RestartPanel.Active();
        TimerManager.Instance.FinishTimers();
        var effect = EffectPools.Instance.Spawn("Death");
        effect.transform.position = _virusPlayer.transform.position;
        if (_virusPlayer.CurViceWeapon != null)
            _virusPlayer.CurViceWeapon.ReIniti();
        _virusPlayer.gameObject.SetActive(false);
        _gameOverTime = 5;
    }


    [FSM("GameOver", FSMActionName.Update)]
    private void OnGameOverUpdate()
    {
       
        _gameOverTime -= Time.deltaTime;
        if (_gameOverTime <= 0)
        {
            _gameOverTime = 0;
            _uiMrg.RestartPanel.SetLeftTime(0);
            SceneManager.LoadScene("SelectUI");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isClickSpace = true;
        }
        if (_isClickSpace)
        {
            _isClickSpace = false;
            _uiMrg.RestartPanel.UnActive();
            _virusPlayer.Revive();
            _virusPlayer.gameObject.SetActive(true);
            _virusPlayer.transform.position = new Vector3(0f, -5f, 0f);
            _virusPlayer.SetPlayerState(true, true);
            _fsm.ChangeState(VirusGameState.None);
        }
        _uiMrg.RestartPanel.SetLeftTime(Mathf.CeilToInt(_gameOverTime));

    }


   
}