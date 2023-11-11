using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Switches sprites when the world switches
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSwitcher : MonoBehaviour, IWorldSwitcher {
    [SerializeField] private Sprite firstWorldSprite;
    [SerializeField] private Sprite secondWorldSprite;
    
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        WorldSwitcher.Instance.RegisterSwitcher(this);
        
        OnSwitchWorld(GameManager.Instance.IsTimer1Active()); //Setup the initial sprite in case we were instantiated at runtime
    }
    
    private void OnDisable() {
        WorldSwitcher.Instance.UnregisterSwitcher(this);
    }

    public void OnSwitchWorld(bool firstWorldActive) {
        spriteRenderer.sprite = firstWorldActive ? firstWorldSprite : secondWorldSprite;
    }
}
