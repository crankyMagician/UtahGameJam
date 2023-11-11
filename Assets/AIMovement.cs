using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Timer: MonoBehaviour
{
    public static bool Delay(float second, Action action)
    {
        if (Instance)
        {
            Instance.AddDelay(second, action);
            return true;
        }
        return false;
    }

    static Timer Instance;

    private List<TimerInfo> timers;

    private class TimerInfo
    {
        public float delay;
        public Action action;

        public TimerInfo(float d, Action a)
        {
            delay = d;
            action = a;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void AddDelay(float seconds, Action action)
    {
        timers.Add(new TimerInfo(seconds, action));
    }

    private void Update()
    {
        bool executed = false;
        foreach (var timer in timers)
        {
            timer.delay -= Time.deltaTime;
            if (timer.delay <= 0)
            {
                timer.action?.Invoke();
                executed = true;
            }
        }

        if (executed){
            timers = timers.Where( x => x.delay > 0 ).ToList();
        }
    }
}

public class AIMovement : MonoBehaviour
{
	public delegate void OnEnemyReachedBottomDelegate(AIMovement movement);

	static OnEnemyReachedBottomDelegate OnEnemyReachedBottom;
	OnEnemyReachedBottomDelegate OnReachedBottom;

	float yStartLocation = 5; //5 is arbitrarily chose n
	float yTargetLocation = 0;

	/// Amount of time it takes for this to reach the bottom
	float timeToBottom = 10;
	float lifetime = 0;

    /// Denotes the furthest point leftwards they can be on the side of the screen
    /// When the enemy hits this point it will attemp to start moving in the other
    /// Direction
    /// we can change this to be smaller/larger then the screen to change the timing of this
    float xLowerBound;

    /// Denotes the furthest point rightwards they can be on the side of the screen
    float xUpperBound;

	private void Awake()
	{
		yTargetLocation = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 10)).y;
		yTargetLocation -= 1;
	}

	private void Start()
	{
		lifetime = 0;
	}

	private void Update()
	{
		float alpha = lifetime / timeToBottom;
		float tweenAlpha = EasingFunctions.InOutBounce(alpha);

		lifetime += Time.deltaTime;
		Vector3 pos = transform.position;
		pos.y = Mathf.Lerp(
				yStartLocation,
				yTargetLocation,
				tweenAlpha);

		transform.position = pos;

		///Check if we are close
		if (Mathf.Abs(transform.position.y - yTargetLocation) < .005)
		{
			OnEnemyReachedBottom?.Invoke(this);
			OnReachedBottom?.Invoke(this);
			DestroyBot();
		}
	}


	///Should we destroy here ??
	private void DestroyBot()
	{
		Destroy(gameObject);
	}
}


public class BotManager : MonoBehaviour
{
    /// need a way to check if the game is still running
    bool GameIsOver = false;

    GameObject botTemplate;

    void RepeatedSpawnWave(
            int mobSize,
            Vector3 initialPosition,
            float delayBetweenMobs = .5f,
            float delayBetweenWaves = 5,

            /// Slowly increase the size of the wave
            float mobSizeScaler = 1.2f,

            /// Slowly decrease the time between waves
            float waveTimeScalar = .9f)
    {
        SpawnWave(mobSize, initialPosition, delayBetweenMobs);
        Timer.Delay(delayBetweenWaves, () =>
        {
           if (!GameIsOver)
           {
                SpawnWave(mobSize, initialPosition, delayBetweenMobs);
                Timer.Delay(delayBetweenWaves, () =>
                {
                    RepeatedSpawnWave(
                            (int)(mobSize * mobSizeScaler),
                            initialPosition,
                            delayBetweenMobs,
                            delayBetweenWaves * waveTimeScalar,
                            mobSizeScaler,
                            waveTimeScalar);
                });
           }
        });
    }

    void SpawnWave(
            int waveCount,
            Vector3 initialPosition,
            float delayBetweenMobs = .5f)
    {
        // Greedy spawn 1 man now
        var bot = Instantiate(botTemplate);
        bot.transform.position = initialPosition;

        // Spawn the rest with delays
        for (int i = 1; i < waveCount; ++i)
        {
            Timer.Delay((float)i * delayBetweenMobs, () =>
            {
                var bot = Instantiate(botTemplate);
                bot.transform.position = initialPosition;
            });
        }
    }
}
