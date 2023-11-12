using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    [SerializeField] private float speed = 5;
    [SerializeField] private Vector3 direction;

    [SerializeField] private String targetTag;
    
    private Rigidbody2D rb;

    private float lifespan = 10f;
    private float launchTime = float.MaxValue;

    private bool hasHit = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        //Destroy after lifespan
        if (Time.time - launchTime > lifespan) {
            Destroy(gameObject);
        }
    }

    public void Launch() {
        rb.velocity = direction * speed;
        launchTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(targetTag) && !hasHit) {
            hasHit = true;
            
            Destroy(gameObject);
        }
    }
}
