using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour  {
    [SerializeField] private TextMeshProUGUI numText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image icon;
    
    [NonSerialized] public Powerup powerup;

    public int costAmt;
    
    public void Setup(Powerup powerup, int id, int cost) {
        this.powerup = powerup;
        costAmt = cost;
        
        numText.text = id.ToString();
        
        nameText.text = powerup.powerupName;
        descriptionText.text = powerup.description;
        this.cost.text = $"Cost: {cost}";
        
        icon.sprite = powerup.icon;
    }
}
