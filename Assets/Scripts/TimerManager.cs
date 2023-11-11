using System;
using UnityEngine;

[Serializable]
public class TimeManager
{
    public float Timer1 { get; private set; } = 120f;
    public float Timer2 { get; private set; } = 120f;

    
    private bool isTimer1Active = true;
    private float timeSinceLastSwitch = 0f;
    private const float SwitchCooldown = 30f;

    public void UpdateTimers(float deltaTime)
    {
        if (isTimer1Active && Timer1 > 0)
        {
            Timer1 -= deltaTime;
        }
        else if (!isTimer1Active && Timer2 > 0)
        {
            Timer2 -= deltaTime;
        }

        timeSinceLastSwitch += deltaTime;
    }

    public bool TrySwitchTimer()
    {
        if (timeSinceLastSwitch < SwitchCooldown)
        {
            return false;
        }

        float otherTimer = isTimer1Active ? Timer2 : Timer1;
        if (otherTimer < SwitchCooldown)
        {
            return false;
        }

        isTimer1Active = !isTimer1Active;
        timeSinceLastSwitch = 0f;
        return true;
    }

    public void AddTimeToInactiveTimer(float timeToAdd)
    {
        if (isTimer1Active)
        {
            Timer2 += timeToAdd;
        }
        else
        {
            Timer1 += timeToAdd;
        }
    }

    public string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
    
    // New method to get the active timer state
    public bool IsTimer1Active()
    {
        return isTimer1Active;
    }
}