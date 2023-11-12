using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemySwitcher : MonoBehaviour, IWorldSwitcher {
    [SerializeField, ReadOnly] private bool createdInWorld1;

    [SerializeField] private Sprite startWorld1Sprite;
    [SerializeField] private Sprite startWorld2Sprite;
    
    private void Awake() {
        createdInWorld1 = GameManager.Instance.timeManager.IsTimer1Active();
        
        GetComponent<SpriteRenderer>().sprite = createdInWorld1 ? startWorld1Sprite : startWorld2Sprite;
        GetComponent<Animator>().SetTrigger(createdInWorld1 ? "isWorld1" : "isWorld2");
    }

    private void OnEnable() {
        WorldSwitcher.Instance.RegisterSwitcher(this);
    }

    private void OnDestroy() {
        WorldSwitcher.Instance.UnregisterSwitcher(this);
    }

    public void OnSwitchWorld(bool firstWorldActive) {
        gameObject.SetActive(firstWorldActive == createdInWorld1);
    }
}
