using System;

public class Timer
{

    public float Duration;

    public float LeftTime;


    private Action<float> _updateAction;

    private Action _destroyAction;

    private Action _awakeAction;

    private Action _enbaleAction;

    private bool _isPause;



    public Timer(float duration, Action<float> updateAction = null, Action destroyAction = null, Action awakeAction = null, Action enbaleAction = null)
    {

        LeftTime = duration;

        Duration = duration;

        if (enbaleAction != null)
        {
            enbaleAction.Invoke();
        }

        _awakeAction = awakeAction;

        _updateAction = updateAction;

        _destroyAction = destroyAction;

        _isPause = false;

    }


    public void OnAwake()
    {
        if (_awakeAction != null)
            _awakeAction.Invoke();
    }

    public void OnUpdate(float deltaTime)
    {

        LeftTime -= deltaTime;
        if (LeftTime <= 0)
        {
            if (_destroyAction != null)
                _destroyAction.Invoke();
        }
        else
        {
            if (_updateAction != null && !_isPause)
                _updateAction.Invoke(LeftTime);
        }
    }

    public void FinishTimer()
    {
        if (_destroyAction != null)
            _destroyAction.Invoke();
    }

    public void SetTimerTrick(bool b)
    {
        _isPause = b;
    }


}
