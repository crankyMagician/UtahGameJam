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
        if (!GameManager.Instance.TryToSwitchTimers())
            return;
        
        //If it is, switch the world
        SwitchWorld();
    }

    private void SwitchWorld() {
        for (var i = 0; i < switchers.Count; i++) {
            IWorldSwitcher switcher = switchers[i];
            
            if(switcher == null)
                UnregisterSwitcher(null);
            else 
                switcher.OnSwitchWorld(GameManager.Instance.timeManager.IsTimer1Active());
        }
    }
    
    public void RegisterSwitcher(IWorldSwitcher worldSwitcher) {
        switchers.Add(worldSwitcher);
    }
    
    public void UnregisterSwitcher(IWorldSwitcher worldSwitcher) {
        if(switchers.Contains(worldSwitcher))
            switchers.Remove(worldSwitcher);
    }
}
