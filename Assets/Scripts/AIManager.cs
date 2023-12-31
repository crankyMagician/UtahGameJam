using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace J {

public class AIManager : MonoBehaviour
{
	/// need a way to check if the game is still running
	bool GameIsActive => GameManager.Instance.isGameActive;

	public GameObject botTemplate;
	public GameObject flyPrefab;
	public GameObject crabPrefab;
	
	void Start()
	{
		StartGame();
		GameManager.OnGameEnd += () => { J.Timer.ClearTimers(); };
		GameManager.OnGameRestart += () => { StartGame(); };
	}

	void StartGame()
	{
		RepeatedSpawnWave(
				AIParameters.Make(),
				2,
				AIMovementConstantParameters.RandomInitialPosition(),
				flyPrefab);

		RepeatedSpawnWave(
				AIParameters.Make(),
				1.5f,
				AIMovementConstantParameters.RandomInitialPosition(),
				crabPrefab);

		RepeatedSpawnWave(
				AIParameters.Make(),
				1,
				AIMovementConstantParameters.RandomInitialPosition(),
				botTemplate);
	}

	void RepeatedSpawnWave(
			AIParameters movementParameters,

			///This is a flaot so we can use mobSizeScalar to scale it
			/// and slowly grow if the value is low
			float mobSize,
			Vector3 initialPosition,
			GameObject prefab,
			float delayBetweenMobs = 1.2f,
			float delayBetweenWaves = 10,
			/// Slowly increase the size of the wave
			float mobSizeScaler = 1.2f,
			/// Slowly decrease the time between waves
			float waveTimeScalar = 1f)
	{
		SpawnWave(movementParameters, mobSize, initialPosition, prefab, delayBetweenMobs);
		J.Timer.Delay(1, () =>
		{
			if (GameIsActive)
			{
				SpawnWave(movementParameters, mobSize, initialPosition, prefab, delayBetweenMobs);
				Timer.Delay(delayBetweenWaves, () =>
				{
					var newPos = AIMovementConstantParameters.RandomInitialPosition();
					RepeatedSpawnWave(
						AIParameters.Make(),
						(mobSize * mobSizeScaler),
						newPos,
						prefab,
						delayBetweenMobs,
						delayBetweenWaves * waveTimeScalar,
						mobSizeScaler,
						waveTimeScalar);
				});
			}
		});
	}

	void SpawnWave(
		AIParameters movementParameters,
		float waveCount,
		Vector3 initialPosition,
		GameObject prefab,
		float delayBetweenMobs = .5f)
	{
		// Greedy spawn 1 man now
		var bot = Instantiate(prefab);
		var ai  = bot.GetComponent<AIMovement>();
		ai.movementParameters = movementParameters;
		bot.transform.position = initialPosition;

		// Spawn the rest with delays
		for (int i = 1; i < (int)waveCount; ++i)
		{
			J.Timer.Delay((float)i * delayBetweenMobs, () => {
				var bot = Instantiate(prefab);
				var ai  = bot.GetComponent<AIMovement>();
				ai.movementParameters = movementParameters;
				bot.transform.position = initialPosition;
		});
		}
	}
}
}