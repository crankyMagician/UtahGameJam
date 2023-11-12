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

	void Start()
	{
		RepeatedSpawnWave(
				AIParameters.Make(),
				1,
				AIMovementConstantParameters.RandomInitialPosition());

		RepeatedSpawnWave(
				AIParameters.Make(),
				1,
				AIMovementConstantParameters.RandomInitialPosition());

		RepeatedSpawnWave(
				AIParameters.Make(),
				1,
				AIMovementConstantParameters.RandomInitialPosition());
	}

	void RepeatedSpawnWave(
			AIParameters movementParameters,

			///This is a flaot so we can use mobSizeScalar to scale it
			/// and slowly grow if the value is low
			float mobSize,
			Vector3 initialPosition,
			float delayBetweenMobs = 1f,
			float delayBetweenWaves = 10,
			/// Slowly increase the size of the wave
			float mobSizeScaler = 1.2f,
			/// Slowly decrease the time between waves
			float waveTimeScalar = 1f)
	{
		SpawnWave(movementParameters, mobSize, initialPosition, delayBetweenMobs);
		J.Timer.Delay(1, () =>
		{
			if (GameIsActive)
			{
				SpawnWave(movementParameters, mobSize, initialPosition, delayBetweenMobs);
				Timer.Delay(delayBetweenWaves, () =>
				{
					var newPos = AIMovementConstantParameters.RandomInitialPosition();
					RepeatedSpawnWave(
						AIParameters.Make(),
						(mobSize * mobSizeScaler),
						newPos,
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
		float delayBetweenMobs = .5f)
	{
		// Greedy spawn 1 man now
		var bot = Instantiate(botTemplate);
		var ai  = bot.GetComponent<AIMovement>();
		ai.movementParameters = movementParameters;
		bot.transform.position = initialPosition;

		// Spawn the rest with delays
		for (int i = 1; i < (int)waveCount; ++i)
		{
			J.Timer.Delay((float)i * delayBetweenMobs, () => {
				var bot = Instantiate(botTemplate);
				var ai  = bot.GetComponent<AIMovement>();
				ai.movementParameters = movementParameters;
				bot.transform.position = initialPosition;
		});
		}
	}
}
}