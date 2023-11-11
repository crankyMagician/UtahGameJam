using System;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public TimeManager timeManager = new TimeManager();
    private float totalElapsedTime = 0f;
    private bool isGameActive = true;

    [BoxGroup("Timers")]
    public TextMeshProUGUI Timer1Text;

    [BoxGroup("Timers")]
    public TextMeshProUGUI Timer2Text;

    [BoxGroup("Timers")]
    public TextMeshProUGUI TotalTimeText;
    
    [BoxGroup("Timers")]
    public TextMeshProUGUI GameOverText;
    
    public Button retryButton;
    public Button quitButton;

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
        TotalTimeText.text = "";
        GameOverText.text = "";
        retryButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isGameActive)
        {
            TotalTimeText.text = timeManager.FormatTime(totalElapsedTime);
            GameOverText.text = "Game Over play again?";
            retryButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            return;
        }

        float deltaTime = Time.deltaTime;
        timeManager.UpdateTimers(deltaTime);

        UpdateTimerTexts();
        UpdateTimerColors();
        
        totalElapsedTime += deltaTime;

        CheckTimer(timeManager.Timer1);
        CheckTimer(timeManager.Timer2);
    }

    private void UpdateTimerTexts()
    {
        Timer1Text.text = timeManager.FormatTime(timeManager.Timer1);
        Timer2Text.text = timeManager.FormatTime(timeManager.Timer2);
       
    }

    private void UpdateTimerColors()
    {
        bool isTimer1Active = timeManager.IsTimer1Active();
        Timer1Text.color = isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        Timer2Text.color = !isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
    }
    private void CheckTimer(float timer)
    {
        if (timer <= 0 && isGameActive)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        OnEndGame?.Invoke();
    }

    [Button("Try to switch timers")]
    public void TryToSwitchTimers()
    {
        timeManager.TrySwitchTimer();
    }
    [Button("Add Time to Timer")]
    public void AddTimeToInactiveTimer()
    {
        timeManager.AddTimeToInactiveTimer(30f);
    }
    
    [Button("Restart Game")]
    public void RestartGame()
    {
        isGameActive = true;
        totalElapsedTime = 0f;
        timeManager = new TimeManager();
        
    }

   [Button("Quit Game")]
    public void QuitGame()
    {
        // Save any game data if necessary

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

/*
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

    [BoxGroup("Timers")]
    
    public TextMeshProUGUI Timer1Text; // Reference to the TMP text for Timer1

    [BoxGroup("Timers")]
  
    public TextMeshProUGUI Timer2Text; // Reference to the TMP text for Timer2

    [BoxGroup("Switch Control")]
    [SerializeField]
    private bool isTimer1Active = true;

    [BoxGroup("Switch Control")]
    [SerializeField]
    private float timeSinceLastSwitch = 0f; // Time since the last switch

    [BoxGroup("Switch Control")]
    [SerializeField]
    private const float SwitchCooldown = 30f; // 30 seconds cooldown for switching
    
    [BoxGroup("Time Tracking")]
    [SerializeField, ReadOnly]
    private float totalElapsedTime = 0f; // Total elapsed time
    
    [BoxGroup("Game State")]
    [SerializeField, ReadOnly]
    private bool isGameActive = true; // Flag to check if the game is active
    
    [BoxGroup("Timers")]
    public TextMeshProUGUI TotalTimeText; // Reference to the TMP text for Total Time


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
        if (!isGameActive) return; // Stop updating if the game is not active

        float deltaTime = Time.deltaTime;

        // Update timers
        if (isTimer1Active && Timer1 > 0)
        {
            Timer1 -= deltaTime;
            CheckTimer(Timer1);
        }
        else if (!isTimer1Active && Timer2 > 0)
        {
            Timer2 -= deltaTime;
            CheckTimer(Timer2);
        }

        // Update the total elapsed time
        totalElapsedTime += deltaTime;

        UpdateTimerTexts(); // Update the timer texts every frame
        timeSinceLastSwitch += deltaTime; // Update the time since the last switch
    }



    private bool TrySwitchTimer()
    {
        if (timeSinceLastSwitch < SwitchCooldown)
        {
            Debug.Log($"Switching timers is on cooldown. Please wait {SwitchCooldown - timeSinceLastSwitch:F2} more seconds.");
            return false;
        }

        // Check if the other timer has at least 30 seconds left
        float otherTimer = isTimer1Active ? Timer2 : Timer1;
        if (otherTimer < SwitchCooldown)
        {
            Debug.Log("Cannot switch as the other timer does not have enough time left.");
            return false;
        }

        isTimer1Active = !isTimer1Active;isTimer1Active
        timeSinceLastSwitch = 0f;
        Debug.Log($"Switched timers. Timer1 is now {(isTimer1Active ? "active" : "inactive")}.");
        return true;
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
            if (!isGameActive) return; // Prevent multiple end game triggers

            isGameActive = false; // Mark the game as ended
            OnEndGame?.Invoke();
            Debug.Log($"Game ended. Total elapsed time: {FormatTime(totalElapsedTime)}");
        }
    }

   
    private void UpdateTimerTexts()
    {
        if (!isGameActive)
        {
            Timer1Text.text = "00:00";
            Timer2Text.text = "00:00";
          
        }
        else
        {
            Timer1Text.text = FormatTime(Timer1);
            Timer2Text.text = FormatTime(Timer2);
            TotalTimeText.text = FormatTime(totalElapsedTime); // Update for total time
        }
    
        Timer1Text.color = isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        Timer2Text.color = !isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        // You can also update the color for TotalTimeText if needed
        Debug.Log($"Updated timer texts: Timer1 - {Timer1Text.text}, Timer2 - {Timer2Text.text}, Total Time - {TotalTimeText.text}");
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
    
    public bool IsTimer1Active() {
        return isTimer1Active;
    }
}
*/
