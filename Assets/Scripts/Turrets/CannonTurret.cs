using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : Turret
{
    // Pitch: -14 to 24.5

    [SerializeField] private Projectile cannonballPrefab;

    [SerializeField] private Transform turningPlate;
    [SerializeField] private Transform barrelPivot;
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform projectileSpawn;

    [SerializeField] private float range;
    [SerializeField] private float cannonballSpeed;
    [SerializeField] private float fireFrequency;

    public override void MainInit()
    {
        
    }

    public override void MainUpdate()
    {
        if (TryFindTarget(out Enemy target))
        {
            Vector3 vector = (target.transform.position - transform.position).normalized;
            Shoot(vector);
            Aim(vector);
        }
        else
        {
            Vector3 vector = (GameManager.I.wave.GetSpawnPoint() - transform.position).normalized;
            Aim(vector);
        }
    }

    private bool TryFindTarget(out Enemy target)
    {
        target = null;

        float closestDistance = float.MaxValue;
        foreach (Enemy enemy in GameManager.I.enemy.Get().Values)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < range && distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy;
            }
        }

        return target != null;
    }


    private void Aim(Vector3 vector)
    {
        float heading = (Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg) + 90f;
        float pitch = Mathf.Sin(vector.y) * Mathf.Rad2Deg;

        turningPlate.localEulerAngles = new Vector3(0, 0, heading);
        barrelPivot.localEulerAngles = new Vector3(0, Mathf.Clamp(pitch, -14, 24.5f), 0);
    }

    private void Shoot(Vector3 vector)
    {
        Projectile bullet = Instantiate(cannonballPrefab, projectileSpawn.position, Quaternion.identity);
        bullet.rb.AddRelativeForce(vector * cannonballSpeed, ForceMode.VelocityChange);
        GameManager.I.projectile.Add(bullet);
    }
}
