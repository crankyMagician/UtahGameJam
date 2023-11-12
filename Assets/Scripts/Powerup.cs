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
        GameManager.Instance.timeManager.FireRate /= 1.25f;
    }

    public void GainShot() {
        GameManager.Instance.timeManager.AmtOfShots++;
    }

    public void MovementSpeed() {
        GameManager.Instance.timeManager.speedMultiplier *= 1.25f;
    }
}
