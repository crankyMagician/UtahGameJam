using System;
using System.Collections;
using System.Collections.Generic;
using J;
using UnityEngine;

[System.Serializable]
public class RandomSoundPlayer
{
	private static System.Random rnd = new System.Random();

	[SerializeField]
	public AudioSource audioSource;

	[SerializeField]
	public List<AudioClip> sounds = new List<AudioClip>();

	public void PlaySound()
	{
		if (audioSource != null && sounds.Count > 0)
		{
			int soundIndex = rnd.Next(0, sounds.Count); // creates a number between 1 and 12
			audioSource.PlayOneShot(sounds[soundIndex]);
		}
	}
};