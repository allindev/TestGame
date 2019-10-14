using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    private List<Timer> _timers;
    private Dictionary<string, Timer> _timerDict;


    private void Awake()
    {
        _timers = new List<Timer>();
        _timerDict = new Dictionary<string, Timer>();
    }

    private void Update()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].OnUpdate(Time.deltaTime);
        }
    }

    private IEnumerator FinishAllTimer()
    {
        while (true)
        {
            if (_timers.Count > 0)
                _timers[0].FinishTimer();
            else
                yield break;
            yield return null;
        }
    }



    public void FinishTimers()
    {
        StartCoroutine(FinishAllTimer());
    }

    public void AddTimer(string str, Timer timer)
    {
        if (_timerDict.ContainsKey(str))
        {
            _timerDict[str].LeftTime = _timerDict[str].Duration;
        }
        else
        {
            _timerDict.Add(str, timer);
            _timers.Add(timer);
            timer.OnAwake();
        }
    }

    public void RemoveTimer(string str)
    {
        var timer = _timerDict[str];
        if (timer != null)
        {
            _timers.Remove(timer);
            _timerDict.Remove(str);
        }
    }


}