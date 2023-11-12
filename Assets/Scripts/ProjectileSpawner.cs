using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileSpawner : MonoBehaviour {
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private Projectile activeProjectile = null;
    
    [SerializeField, ReadOnly] public float fireRate = 0.5f;
    [SerializeField, ReadOnly] private float lastSummonTime = float.MinValue;

    public int amount = 0;
    
    public void SummonProjectile([CanBeNull] Transform parent, bool bypass = false) {
        if (activeProjectile != null)
            return;
        if(Time.time - lastSummonTime < fireRate || bypass) //Sick strat- hold onto your bullet and you can fire two in a row if you time it right
            return;
        
        lastSummonTime = Time.time;
        activeProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        
        if(parent != null)
            activeProjectile.transform.SetParent(parent);
    }
    
    public void LaunchProjectile() {
        if (activeProjectile == null)
            return;
        
        activeProjectile.transform.SetParent(null);
        activeProjectile.Launch();

        activeProjectile = null;

        StartCoroutine(ShootShots(amount));
    }

    private IEnumerator ShootShots(int amt) {
        if (amt <= 0)
            yield break;

        yield return new WaitForSeconds(0.1f);
        
        Projectile proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        proj.Launch();
        
        StartCoroutine(ShootShots(amt - 1));
    }
}
