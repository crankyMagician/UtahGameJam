using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Player { get; set; }
    
    [SerializeField] private float speedMultiplier = 1; //This is reserved in case we decide to add a speed multiplier at some point
    private const float speedModifier = 15; //This is used to counteract Time.deltaTime making our numbers really small
    private Vector3 lastMovement =  Vector3.zero;

    private void Awake() {
        if (Player == null) {
            Player = this;
        }
        
        //Move the player to start on the bottom of the screen
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 10));
    }

    private void Update() {
        transform.position += lastMovement * (Time.deltaTime * speedModifier * speedMultiplier); //Order is to reduce vector math
        
        //Clamp position inside of the camera
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.025f, .975f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void UpdateMovement(Vector3 movement) {
        lastMovement = movement;
    }
}