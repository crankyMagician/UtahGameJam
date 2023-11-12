using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class SpecialController : MonoBehaviour {
    void Start() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, Random.Range(-7,-3));
    }
}