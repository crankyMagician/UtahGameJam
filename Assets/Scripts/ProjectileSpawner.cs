using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private Projectile activeProjectile = null;
    
    private float summonCooldown = 0.5f;
    private float lastSummonTime = float.MinValue;
    
    public void SummonProjectile([CanBeNull] Transform parent) {
        if (activeProjectile != null)
            return;
        if(Time.time - lastSummonTime < summonCooldown) //Sick strat- hold onto your bullet and you can fire two in a row if you time it right
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
    }
}
