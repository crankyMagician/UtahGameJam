using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialsSpawner : MonoBehaviour {
    [SerializeField] private GameObject[] specials;
    
    void Start() {
        StartCoroutine(SpawnSpecial());    
    }

    private IEnumerator SpawnSpecial() {
        yield return new WaitForSeconds(Random.Range(10,40));
        
        //Instantiate a random special at the top of the screen
        Instantiate(specials[Random.Range(0, specials.Length)], new Vector3(Random.Range(-8, 8), 5), Quaternion.identity);
    }
}
