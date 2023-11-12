using System;
using System.Collections;
using System.Collections.Generic;
using J;
using UnityEngine;

[System.Serializable]
public class MusicPlayer: MonoBehaviour
{
	private static System.Random rnd = new System.Random();
	public AudioSource audioSource;

	[SerializeField]
	public AudioClip music;

	public void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = music;
		audioSource.Play();
	}
};