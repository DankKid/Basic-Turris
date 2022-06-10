using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : Turret
{
    // Pitch: -14 to 24.5

    [SerializeField] private Projectile cannonballPrefab;

    [SerializeField] private Transform turningPlate;
    [SerializeField] private Transform barrelPivot;
    [SerializeField] private Transform projectileSpawn;

    [SerializeField] private float range;
    [SerializeField] private float cannonballSpeed;

    public override void MainInit()
    {
        
    }

    public override void MainUpdate()
    {
        if (!ShootAtClosestEnemy())
        {
            Aim(GetHeadingToAimAt(manager.waveManager.GetSpawnPoint()), 0);
        }
    }

    private bool ShootAtClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        Enemy closest = null;
        foreach (Enemy enemy in manager.enemies.Values)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < range && distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        if (closest == null)
        {
            return false;
        }

        Projectile bullet = Instantiate(cannonballPrefab, projectileSpawn.position, Quaternion.identity);
        bullet.rb.AddRelativeForce((closest.transform.position - transform.position).normalized * cannonballSpeed, ForceMode.VelocityChange);
        manager.AddProjectile(bullet);

        Aim(GetHeadingToAimAt(closest.transform.position), 0);

        return true;
    }

    private float GetHeadingToAimAt(Vector3 target)
    {
        Vector3 diffVector = transform.position - target;
        float heading = Mathf.Atan2(diffVector.x, diffVector.z) * Mathf.Rad2Deg;
        return heading;
    }


    private void Aim(float heading, float pitch)
    {
        turningPlate.localEulerAngles = new Vector3(0, 0, heading - 90f);
        barrelPivot.localEulerAngles = new Vector3(0, Mathf.Clamp(pitch, -14, 24.5f), 0);
    }
}
