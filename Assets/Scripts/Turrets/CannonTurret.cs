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
        if (!ShootAtClosestEnemy())
        {
            Aim(GetHeadingToAimAt(GameManager.I.wave.GetSpawnPoint()), 0);
        }
    }

    private bool ShootAtClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        Enemy closest = null;
        foreach (Enemy enemy in GameManager.I.enemy.Get().Values)
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

        Aim(GetHeadingToAimAt(closest.transform.position), GetPitchToAimAt(closest.transform.position));

        /*
        Projectile bullet = Instantiate(cannonballPrefab, projectileSpawn.position, Quaternion.identity);
        bullet.rb.AddRelativeForce((closest.transform.position - transform.position).normalized * cannonballSpeed, ForceMode.VelocityChange);
        GameManager.I.projectile.Add(bullet);
        */

        return true;
    }

    private float GetHeadingToAimAt(Vector3 target)
    {
        Vector3 diffVector = transform.position - target;
        float heading = (Mathf.Atan2(diffVector.x, diffVector.z) * Mathf.Rad2Deg) - 90f;
        return heading;
    }

    private float GetPitchToAimAt(Vector3 target)
    {
        Vector3 diffVector = transform.position - target;
        //float pitch = -Mathf.Atan2(diffVector.x, diffVector.y) * Mathf.Rad2Deg;
        float pitch = -Vector3.Angle(Vector3.left, diffVector);
        return pitch;
    }


    private void Aim(float heading, float pitch)
    {
        turningPlate.localEulerAngles = new Vector3(0, 0, heading);
        barrelPivot.localEulerAngles = new Vector3(0, Mathf.Clamp(pitch, -14, 24.5f), 0);
    }
}
