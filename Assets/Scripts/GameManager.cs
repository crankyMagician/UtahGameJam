using UnityEngine;
using System;
using TMPro; // For the Action event

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Subscribe to the event here
        // Example: SomeClass.OnSwitchEvent += SwitchTimers;
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

    private void SwitchTimers()
    {
        isTimer1Active = !isTimer1Active;
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

    private void CheckTimer(float timer)
    {
        if (timer <= 0)
        {
            OnEndGame?.Invoke();
        }
    }

    private void UpdateTimerTexts()
    {
        // Formatting and updating the text
        Timer1Text.text = $"{Timer1:F1}";
        Timer2Text.text = $"{Timer2:F1}";

        // Setting the color and transparency
        Timer1Text.color = isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f); // Active: Yellow, Inactive: Yellow and transparent
        Timer2Text.color = !isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event here, if any
    }
}