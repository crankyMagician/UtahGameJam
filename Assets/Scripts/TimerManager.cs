using System;
using UnityEngine;


[Serializable]
public class TimeManager
{
    public float Timer1 { get; private set; } = 120f;
    public float Timer2 { get; private set; } = 0f;
  

    private bool isTimer1Active = true;
    public float timeSinceLastSwitch = 0f;
    public const float SwitchCooldown = 15f;

    public void UpdateTimers(float deltaTime)
    {
        try
        {
            if (isTimer1Active && Timer1 > 0)
                Timer1 -= deltaTime;
            else if (!isTimer1Active && Timer2 > 0) Timer2 -= deltaTime;

            timeSinceLastSwitch += deltaTime;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in UpdateTimers: " + ex.Message);
        }
    }

    public void SetTimerOne(float timeInSeconds)
    {
        try
        {
            Timer1 = timeInSeconds;
            Debug.Log($"Timer1 set to {timeInSeconds}");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in SetTimerOneTo: " + ex.Message);
        }
    }

    public bool TrySwitchTimer()
    {
        try
        {
            if (timeSinceLastSwitch < SwitchCooldown)
            {
                Debug.Log("Switch timer failed due to cooldown");
                return false;
            }

            var otherTimer = isTimer1Active ? Timer2 : Timer1;
            if (otherTimer < SwitchCooldown)
            {
                Debug.Log("Switch timer failed due to other timer's cooldown");
                return false;
            }

            isTimer1Active = !isTimer1Active;
            timeSinceLastSwitch = 0f;
            Debug.Log($"Timer switched. Active timer: {(isTimer1Active ? "Timer1" : "Timer2")}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in TrySwitchTimer: " + ex.Message);
            return false;
        }
    }

    public void AddTimeToInactiveTimer(float timeToAdd)
    {
        try
        {
            if (isTimer1Active)
                Timer2 += timeToAdd;
            else
                Timer1 += timeToAdd;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in AddTimeToInactiveTimer: " + ex.Message);
        }
    }

    public void RemoveTimeFromActiveTimer(float timeToAdd)
    {
        try
        {
            if (!isTimer1Active)
                Timer2 -= timeToAdd;
            else
                Timer1 -= timeToAdd;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in AddTimeToInactiveTimer: " + ex.Message);
        }
    }
    
    public string FormatTime(float timeInSeconds)
    {
        try
        {
            var minutes = Mathf.FloorToInt(timeInSeconds / 60);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60);
            return $"{minutes:00}:{seconds:00}";
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in FormatTime: " + ex.Message);
            return "Error";
        }
    }

    public bool IsTimer1Active()
    {
        try
        {
            return isTimer1Active;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in IsTimer1Active: " + ex.Message);
            return false; // Default or error state
        }
    }
}