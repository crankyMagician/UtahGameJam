using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitcher : MonoBehaviour {
    public static WorldSwitcher Instance { get; private set; }

    private List<IWorldSwitcher> switchers = new();
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Attempt to change the player's world
    /// </summary>
    public void TrySwitchWorld() {
        //Check if it is allowed
        //GameManager.Instance.SwitchTimers();
        
        //If it is, switch the world
        SwitchWorld();
    }

    private void SwitchWorld() {
        foreach (IWorldSwitcher switcher in switchers) {
            switcher.OnSwitchWorld(GameManager.Instance.IsTimer1Active());
        }
    }
    
    public void RegisterSwitcher(IWorldSwitcher worldSwitcher) {
        switchers.Add(worldSwitcher);
    }
    
    public void UnregisterSwitcher(IWorldSwitcher worldSwitcher) {
        switchers.Remove(worldSwitcher);
    }
}
