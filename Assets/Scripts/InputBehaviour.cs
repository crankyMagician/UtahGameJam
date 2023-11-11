using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class InputBehaviour : MonoBehaviour {
    MovementController controller;

    private void Awake() {
        controller = GetComponent<MovementController>(); //We know this is always valid because of RequireComponent
    }
       
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        controller.UpdateMovement(movement);
    }
}