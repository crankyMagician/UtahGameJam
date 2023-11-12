using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {
    [SerializeField] private Button muteButton;
    private AudioSource audioSource;
    private bool isMuted = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        muteButton.onClick.AddListener(ToggleAudio);
        
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        
        UpdateAudio();
    }

    private void ToggleAudio() {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        
        UpdateAudio();
    }
    
    private void UpdateAudio() {
        audioSource.mute = isMuted;
    }
}