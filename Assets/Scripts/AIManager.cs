using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace J {
    public class AIManager : MonoBehaviour {
        /// need a way to check if the game is still running
        bool GameIsOver = false;

        public GameObject botTemplate;

        void Start() {
            RepeatedSpawnWave(5, new Vector3(0, 5, 0));
        }

        void RepeatedSpawnWave(
            int mobSize,
            Vector3 initialPosition,
            float delayBetweenMobs = 1f,
            float delayBetweenWaves = 10,
            /// Slowly increase the size of the wave
            float mobSizeScaler = 1.2f,
            /// Slowly decrease the time between waves
            float waveTimeScalar = .9f) {
            SpawnWave(mobSize, initialPosition, delayBetweenMobs);
            J.Timer.Delay(delayBetweenWaves, () => {
                if (!GameIsOver) {
                    SpawnWave(mobSize, initialPosition, delayBetweenMobs);
                    Timer.Delay(delayBetweenWaves, () => {
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
            float delayBetweenMobs = .5f) {
            // Greedy spawn 1 man now
            var bot = Instantiate(botTemplate);
            bot.transform.position = initialPosition;

            // Spawn the rest with delays
            for (int i = 1; i < waveCount; ++i) {
                J.Timer.Delay((float)i * delayBetweenMobs, () => {
                    var bot = Instantiate(botTemplate);
                    bot.transform.position = initialPosition;
                });
            }
        }
    }
}