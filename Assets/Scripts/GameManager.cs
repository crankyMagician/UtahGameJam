using System;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

   public 
    CanvasGroup canvasGroup;// = GetComponent<CanvasGroup>();
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
            // Initialize UI elements to be fully transparent at the start
            InitializeCanvasGroup();

            // Other initializations for your game
            TotalTimeText.text = "";
            GameOverText.text = "";
            retryButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            // Fade in all UI elements
            FadeInUI();

            Debug.Log("GameManager started");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Start: " + ex.Message);
        }
    }

    private void InitializeCanvasGroup()
    {
     
        if (canvasGroup == null)
        {
            // If not, add a CanvasGroup component and set the initial alpha to 0
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    private void FadeInUI()
    {
      
        canvasGroup.DOFade(1, 0.5f); // Fades in over 0.5 seconds
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

            UpdateTimerColors();
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

            // Set active timer to red with tween, inactive to transparent yellow
            if (isTimer1Active)
            {
                Timer1Text.DOColor(Color.red, 0.5f); // Active timer 1
                Timer2Text.color = new Color(1, 1, 0, 0.5f); // Inactive timer 2
            }
            else
            {
                Timer2Text.DOColor(Color.red, 0.5f); // Active timer 2
                Timer1Text.color = new Color(1, 1, 0, 0.5f); // Inactive timer 1
            }
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

            // Tweening the GameOverText
            GameOverText.transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in EndGame: " + ex.Message);
        }
    }

  


    [Button("Try to switch timers")]
    public bool TryToSwitchTimers()
    {
        try
        {
            return timeManager.TrySwitchTimer();
            Debug.Log("Attempted to switch timers");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in TryToSwitchTimers: " + ex.Message);
        }

        return false;
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
        // Fade out UI elements
        FadeOutUI(() =>
        {
            // Actual restart logic
            isGameActive = true;
            totalElapsedTime = 0f;
            timeManager = new TimeManager();
            Debug.Log("Game restarted");
        });
    }

    public void QuitGame()
    {
        // Fade out UI elements
        FadeOutUI(() =>
        {
            // Quitting logic
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            Debug.Log("Quit game triggered");
        });
    }

    private void FadeOutUI(Action onComplete)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.DOFade(0, 0.5f).OnComplete(() => onComplete?.Invoke());
    }
}