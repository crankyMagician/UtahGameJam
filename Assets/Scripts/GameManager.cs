using System;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TimeManager timeManager = new();
    private float totalElapsedTime = 0f;
    private bool isGameActive = true;

    [BoxGroup("Timers")] public TextMeshProUGUI Timer1Text;

    [BoxGroup("Timers")] public TextMeshProUGUI Timer2Text;

    [BoxGroup("Timers")] public TextMeshProUGUI TotalTimeText;

    [BoxGroup("Timers")] public TextMeshProUGUI GameOverText;

    public Button retryButton;
    public Button quitButton;

    public static event Action OnEndGame;

    private void Awake()
    {
        try
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("GameManager instance created");
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate GameManager instance detected and destroyed");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Awake: " + ex.Message);
        }
    }

    private void Start()
    {
        try
        {
            TotalTimeText.text = "";
            GameOverText.text = "";
            retryButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
            Debug.Log("GameManager started");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Start: " + ex.Message);
        }
    }

    private void Update()
    {
        try
        {
            if (!isGameActive)
            {
                TotalTimeText.text = timeManager.FormatTime(totalElapsedTime);
                GameOverText.text = "Game Over play again?";
                retryButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
                return;
            }

            var deltaTime = Time.deltaTime;
            timeManager.UpdateTimers(deltaTime);

            UpdateTimerTexts();
            UpdateTimerColors();

            totalElapsedTime += deltaTime;

            CheckTimer(timeManager.Timer1);
            CheckTimer(timeManager.Timer2);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Update: " + ex.Message);
        }
    }

    private void UpdateTimerTexts()
    {
        try
        {
            Timer1Text.text = timeManager.FormatTime(timeManager.Timer1);
            Timer2Text.text = timeManager.FormatTime(timeManager.Timer2);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in UpdateTimerTexts: " + ex.Message);
        }
    }

    private void UpdateTimerColors()
    {
        try
        {
            var isTimer1Active = timeManager.IsTimer1Active();
            Timer1Text.color = isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
            Timer2Text.color = !isTimer1Active ? Color.yellow : new Color(1, 1, 0, 0.5f);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in UpdateTimerColors: " + ex.Message);
        }
    }

    private void CheckTimer(float timer)
    {
        try
        {
            if (timer <= 0 && isGameActive) EndGame();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in CheckTimer: " + ex.Message);
        }
    }

    private void EndGame()
    {
        try
        {
            isGameActive = false;
            OnEndGame?.Invoke();
            Debug.Log("Game ended");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in EndGame: " + ex.Message);
        }
    }


    [Button("Try to switch timers")]
    public void TryToSwitchTimers()
    {
        try
        {
            timeManager.TrySwitchTimer();
            Debug.Log("Attempted to switch timers");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in TryToSwitchTimers: " + ex.Message);
        }
    }

    [Button("Add Time to Timer")]
    public void AddTimeToInactiveTimer()
    {
        try
        {
            timeManager.AddTimeToInactiveTimer(30f);
            Debug.Log("Added time to inactive timer");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in AddTimeToInactiveTimer: " + ex.Message);
        }
    }

    [Button("Restart Game")]
    public void RestartGame()
    {
        try
        {
            isGameActive = true;
            totalElapsedTime = 0f;
            timeManager = new TimeManager();
            Debug.Log("Game restarted");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in RestartGame: " + ex.Message);
        }
    }

    [Button("Quit Game")]
    public void QuitGame()
    {
        try
        {
            // Save any game data if necessary

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            Debug.Log("Quit game triggered");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in QuitGame: " + ex.Message);
        }
    }
}