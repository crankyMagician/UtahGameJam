using UnityEngine;
using System;
using NaughtyAttributes;
using TMPro; // For TextMeshPro

/// <summary>
/// Manages game state including timers and world switching.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float Timer1 { get; private set; } = 120f;
    public float Timer2 { get; private set; } = 120f;
    private bool isTimer1Active = true;

    public TextMeshProUGUI Timer1Text; // Reference to the TMP text for Timer1
    public TextMeshProUGUI Timer2Text; // Reference to the TMP text for Timer2

    public static event Action OnEndGame;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created.");
        }
        else
        {
            Debug.Log("Duplicate GameManager instance found, destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //subscribe to events
        Debug.Log("GameManager started.");
    }

    private void Update()
    {
        if (isTimer1Active && Timer1 > 0)
        {
            Timer1 -= Time.deltaTime;
            CheckTimer(Timer1);
        }
        else if (!isTimer1Active && Timer2 > 0)
        {
            Timer2 -= Time.deltaTime;
            CheckTimer(Timer2);
        }

        UpdateTimerTexts(); // Update the timer texts every frame
    }

    [Button("Switch Timers")]
    private void SwitchTimers()
    {
        isTimer1Active = !isTimer1Active;
        Debug.Log($"Switched timers. Timer1 is now {(isTimer1Active ? "active" : "inactive")}.");
    }

    public void AddTimeToInactiveTimer(float timeToAdd)
    {
        if (isTimer1Active)
        {
            Timer2 += timeToAdd;
            Debug.Log($"Added {timeToAdd} seconds to Timer2.");
        }
        else
        {
            Timer1 += timeToAdd;
            Debug.Log($"Added {timeToAdd} seconds to Timer1.");
        }
    }

    private void CheckTimer(float timer)
    {
        if (timer <= 0)
        {
            OnEndGame?.Invoke();
            Debug.Log("Timer ran out, triggering end game event.");
        }
    }

    private void UpdateTimerTexts()
    {
        Timer1Text.text = FormatTime(Timer1);
        Timer2Text.text = FormatTime(Timer2);
        Timer1Text.color = isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        Timer2Text.color = !isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        Debug.Log($"Updated timer texts: Timer1 - {Timer1Text.text}, Timer2 - {Timer2Text.text}");
    }

    /// <summary>
    /// Formats the given time in minutes and seconds.
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds.</param>
    /// <returns>A string formatted as minutes:seconds.</returns>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    private void OnDestroy()
    {
        //unsubscribe from events
        Debug.Log("GameManager instance destroyed.");
    }
}
