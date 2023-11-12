using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(ProjectileSpawner))]
public class InputBehaviour : MonoBehaviour {
    private PlayerController controller;
    private ProjectileSpawner projectileSpawner;
    
    private void Awake() {
        //We know this is always valid because of RequireComponent
        controller = GetComponent<PlayerController>();
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }
       
    void Update() {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        controller.UpdateMovement(movement);
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            projectileSpawner.fireRate = GameManager.Instance.timeManager.FireRate;
            
            projectileSpawner.SummonProjectile(transform);
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            projectileSpawner.LaunchProjectile();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            WorldSwitcher.Instance.TrySwitchWorld();
        }
    }
}