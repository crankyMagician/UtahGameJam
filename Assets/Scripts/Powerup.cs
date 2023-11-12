using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Powerup", menuName = "ScriptableObjects/Powerup", order = 1)]
public class Powerup : ScriptableObject {
    public Sprite icon;
    public string powerupName;
    public string description;
    public int maxCost;
    public int minCost;
    public int chance;
    public UnityEvent onBuy;
    
    public void FastReload() {
        ProjectileSpawner spawner = PlayerController.Player.GetComponent<ProjectileSpawner>();

        spawner.fireRate /= 1.25f;
    }
}
