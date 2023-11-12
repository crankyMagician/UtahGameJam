using System;
using System.Collections;
using System.Collections.Generic;
using J;
using UnityEngine;

namespace J
{

[System.Serializable]
public class RandomSoundPlayer: MonoBehaviour
{
	private static System.Random rnd = new System.Random();

	AudioSource audioSource;

	[SerializeField]
	public List<AudioClip> sounds = new List<AudioClip>();

	public static RandomSoundPlayer GlobalInstance;

	public void Awake()
	{
		if (GlobalInstance == null)
		{
			GlobalInstance = this;
			GlobalInstance.audioSource = gameObject.AddComponent<AudioSource>();
		}
	}

	public void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void PlayRandomSound()
	{
		// if (audioSource != null && sounds.Count > 0)
		{
			int soundIndex = rnd.Next(0, sounds.Count); // creates a number between 1 and 12
			audioSource.PlayOneShot(sounds[soundIndex]);
		}
	}

	public void PlayRandomSoundGlobal()
	{
		if (audioSource != null && sounds.Count > 0)
		{
			int soundIndex = rnd.Next(0, sounds.Count); // creates a number between 1 and 12
			if (sounds[soundIndex] == null)
			{
				Debug.Log("Error null sounds for object: " + gameObject.ToString());
				return;
			}

			GlobalInstance?.audioSource?.PlayOneShot(sounds[soundIndex]);
		}
	}

};

}