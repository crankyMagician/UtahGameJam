using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementController : MonoBehaviour {
    [SerializeField] private float speedMultiplier = 1; //This is reserved in case we decide to add a speed multiplier at some point
    private const float speedModifier = 25; //This is used to counteract Time.deltaTime making our numbers really small
    private Vector3 lastMovement =  Vector3.zero;

    private void Awake() {
        
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
