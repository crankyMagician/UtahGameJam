using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class InputBehaviour : MonoBehaviour {
    MovementController controller;
    
    private void Awake() {
        //We know this is always valid because of RequireComponent
        controller = GetComponent<MovementController>();
    }
       
    void Update() {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        controller.UpdateMovement(movement);
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            WorldSwitcher.Instance.TrySwitchWorld();
        }
    }
}