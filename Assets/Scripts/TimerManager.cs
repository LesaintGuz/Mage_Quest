using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class timerCZ
{
    private float m_timeStamp;
    private bool m_isStart;
    private bool m_isEnd;

    private float m_cooldown;

    public delegate void ClockEvent();
    public event ClockEvent m_isFinallyTheEnd;

    public timerCZ(float coolDown)
    {
        m_cooldown = coolDown;
        this.m_isStart = false;
        this.m_isEnd = false;
    }

    public void StartTimer()
    {
        this.m_timeStamp = Time.time + m_cooldown;
        this.m_isStart = true;
        this.m_isEnd = false;
        TimerManager.RegisterTimer(this);
    }

    public void ResetTimer()
    {
        m_timeStamp = Time.time + m_cooldown;
    }

    public void StopTimer()
    {
        m_isStart = false;
        m_isEnd = true;
        TimerManager.UnregisterTimer(this);
    }

    public bool End()
    {
        return m_isEnd;
    }

    public bool IsStart()
    {
        return m_isStart;
    }

    public void Update()
    {
        if (this.m_isStart == true)
        {
            if (this.m_timeStamp <= Time.time)
            {
                this.StopTimer();
                if (m_isFinallyTheEnd != null)
                {
                    m_isFinallyTheEnd();
                }
            }
        }
    }
}

public class TimerManager : SimpleSingle<TimerManager>
{
    private List<timerCZ> timerCZList = new List<timerCZ>();

    public void Update()
    {
        foreach (timerCZ t in timerCZList)
        {
            t.Update();
        }
    }

    public static void RegisterTimer(timerCZ timerToAdd)
    {
        if (!TimerManager.si.timerCZList.Contains(timerToAdd))
            TimerManager.si.timerCZList.Add(timerToAdd);
    }

    public static void UnregisterTimer(timerCZ timerToDel)
    {
        TimerManager.si.timerCZList.Remove(timerToDel);
    }
}

