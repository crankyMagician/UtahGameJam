using System;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UI.ProceduralImage;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public delegate void OnGameStateChangeDelegate();

    /// This should be owned by the class but easy mode
    public static OnGameStateChangeDelegate OnGameRestart;
    public static OnGameStateChangeDelegate OnGameEnd;

    public TimeManager timeManager = new();
    private float totalElapsedTime = 0f;
    public bool isGameActive = true;

    [BoxGroup("Timers")] public TextMeshProUGUI Timer1Text;

    [BoxGroup("Timers")] public TextMeshProUGUI Timer2Text;

    [BoxGroup("Timers")] public TextMeshProUGUI TotalTimeText;

    [BoxGroup("Timers")] public TextMeshProUGUI GameOverText;

    public ProceduralImage proceduralImage;


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
            InitializeUI();
            SetupButtonListeners();
            FadeInUI();

            Debug.Log("GameManager started");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Start: " + ex.Message);
        }
    }


    private void InitializeUI()
    {
        // Initialize UI elements to be fully transparent at the start
        InitializeCanvasGroup();

        // Other initial UI setup
        TotalTimeText.text = "";
        GameOverText.text = "";
        retryButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void SetupButtonListeners()
    {
        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
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
                HandleGameInactiveState();
                return;
            }

            var deltaTime = Time.deltaTime;
            timeManager.UpdateTimers(deltaTime);

            UpdateTimerTexts();
            UpdateTimerColors();

            totalElapsedTime += deltaTime;

            CheckTimer(timeManager.IsTimer1Active() ? timeManager.Timer1 : timeManager.Timer2); //Only check the active timer, its ok if the other timer is <= 0

            UpdateProceduralImageFill(); // Add this line
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in Update: " + ex.Message);
        }
    }
    private void HandleGameInactiveState()
    {
        TotalTimeText.text = timeManager.FormatTime(totalElapsedTime);
        GameOverText.text = "Game Over play again?";
        retryButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        Timer1Text.text = "";
        Timer2Text.text = "";
    }

    private void UpdateProceduralImageFill()
    {
        float fillAmount = CalculateFillAmount();
        if (proceduralImage != null)
            proceduralImage.SetFillAmount(fillAmount);
    }

    private float CalculateFillAmount()
    {
        return Mathf.Clamp01(timeManager.timeSinceLastSwitch / TimeManager.SwitchCooldown);
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

            /// These names are confusing AF
            OnEndGame?.Invoke();
            OnGameEnd?.Invoke();
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
            timeManager.AddTimeToInactiveTimer(10f);
            Debug.Log("Added time to inactive timer");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in AddTimeToInactiveTimer: " + ex.Message);
        }
    }

    public void RemoveTimeFromActiveTimer(int amt = 10)
    {
        try
        {
            timeManager.RemoveTimeFromActiveTimer(amt);
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
            // Other initial UI setup
            TotalTimeText.text = "";
            GameOverText.text = "";
            retryButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            Debug.Log("Game restarted");

            OnGameRestart?.Invoke();
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

    [Button("Debug End Game")]
    public void DebugEndGame()
    {
        try
        {
            // Set Timer 1 to 1 second
            timeManager.SetTimerOne(1f);

            Debug.Log("Debug End Game: Timer 1 set to 1 second");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in DebugEndGame: " + ex.Message);
        }
    }
}