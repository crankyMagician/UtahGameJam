using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupManager : MonoBehaviour {
    [SerializeField] private List<Powerup> powerups;
    [SerializeField] private Transform showContainer;

    [SerializeField] private ShopItem shopItemPrefab;
    
    //The amount of powerups in queue
    private int amtOfPowerups = 0;
    private Coroutine shopCoroutine = null;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Powerup")) {
            AddPowerup();
            Destroy(other.gameObject);
        }
    }

    private void AddPowerup() {
        amtOfPowerups++;
        TryStartShop();
    }

    private void TryStartShop() {
        if (amtOfPowerups <= 0)
            return;
        if (shopCoroutine != null)
            return;
        
        shopCoroutine = StartCoroutine(ShowShop());
    }
    
    //A LOT OF THIS CODE IS JANK
    //NO TIME TO DO BETTER SO OH WELL
    
    private IEnumerator ShowShop() {
        ClearActiveShop();
        amtOfPowerups--;
        
        List<Powerup> chosenPowerups = new();

        while (chosenPowerups.Count != 2) {
            int rand = Random.Range(0, powerups.Count);
            Powerup powerup = powerups[rand];
            
            if (chosenPowerups.Contains(powerup))
                continue;
            
            int chance = Random.Range(0, 100);
            if (chance > powerup.chance && powerup.chance != 0)
                continue;
            
            chosenPowerups.Add(powerup);
        }

        int buy1Cost = 0;
        int buy2Cost = 0;
        
        int index = 0;
        foreach (Powerup powerup in chosenPowerups) {
            index++;
            int costAmt = Random.Range(powerup.minCost, powerup.maxCost);
            
            if(index == 1)
                buy1Cost = costAmt;
            else
                buy2Cost = costAmt;
            
            Instantiate(shopItemPrefab, showContainer).Setup(powerup, index, costAmt);
        }
        
        while (true) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Powerup powerup = chosenPowerups[0];
                powerup.onBuy?.Invoke();
                
                GameManager.Instance.RemoveTimeFromActiveTimer(buy1Cost);
                
                break;
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Powerup powerup = chosenPowerups[1];
                powerup.onBuy?.Invoke();
                
                GameManager.Instance.RemoveTimeFromActiveTimer(buy2Cost);
                
                break;
            }
            
            yield return null;
        }
        
        ClearActiveShop();
        shopCoroutine = null;
        TryStartShop();
    }

    private void ClearActiveShop() {
        for (int i = showContainer.childCount - 1; i >= 0; i--) {
            Destroy(showContainer.GetChild(i).gameObject);
        }
    }
}
