using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemySwitcher : MonoBehaviour, IWorldSwitcher {
    [SerializeField, ReadOnly] private bool createdInWorld1;
    
    private void Awake() {
        createdInWorld1 = GameManager.Instance.timeManager.IsTimer1Active();
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
