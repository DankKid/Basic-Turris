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

    // TODO Recoil
    [SerializeField] private float recoilDistance;
    [SerializeField] private float recoilScaleFactor;
    [Range(0, 100)] [SerializeField] private float recoilTimePercentOfFirePeriod;

    [SerializeField] private float headingMoveSpeed;
    [SerializeField] private float pitchMoveSpeed;
    [SerializeField] private float headingFireThreshold;
    [SerializeField] private float pitchFireThreshold;

    private double allowedFiringTime = 0;
    private double lastFiringTime = -420;

    private float barrelNominalX;
    private Vector3 barrelNominalScale;

    private float currentHeading = 0;
    private float currentPitch = 0;
    private float targetHeading = 0;
    private float targetPitch = 0;

    private float RecoilTime => (recoilTimePercentOfFirePeriod / 100) * (1 / fireFrequency);

    public override void MainInit()
    {
        barrelNominalX = barrel.localPosition.x;
        barrelNominalScale = barrel.localScale;
        // TODO Lerp towards target, go based off distance from and have threshold for shooting at it
    }

    public override void MainUpdate()
    {
        Vector3 direction;
        if (TryFindTarget(out Enemy target))
        {
            float projectileTravelTime = Vector3.Distance(projectileSpawn.position, target.GetTargetPosition()) / cannonballSpeed;
            if (target.TryGetFutureTargetPosition(projectileTravelTime, out Vector3 targetPosition))
            {
                direction = (targetPosition - projectileSpawn.position).normalized;
                Aim(direction);
            }
        }
        // Just aim where it last was aiming, nothing fancy
        /*
        else
        {
            Make it smarter, aim where enemies will come from
            direction = (GameManager.I.wave.GetSpawnPoint() - projectileSpawn.position).normalized;
            Aim(direction);
        }
        */

        float heading = Mathf.MoveTowardsAngle(currentHeading, targetHeading, headingMoveSpeed * Time.deltaTime);
        float pitch = Mathf.MoveTowardsAngle(currentPitch, targetPitch, pitchMoveSpeed * Time.deltaTime);
        SetHeadingAndPitch(heading, pitch);
        bool aimingWithinThresholds = Mathf.Abs(Mathf.DeltaAngle(currentHeading, targetHeading)) < headingFireThreshold && Mathf.Abs(Mathf.DeltaAngle(currentPitch, targetPitch)) < pitchFireThreshold;
        if (target != null && aimingWithinThresholds)
        {
            Shoot(projectileSpawn.forward);
        }

        double timeSinceLastShot = Time.timeAsDouble - lastFiringTime;
        float recoilTime = RecoilTime;
        if (timeSinceLastShot < recoilTime)
        {
            // animate
            float step = (recoilTime - (float)timeSinceLastShot) / recoilTime;
            float scaledStep = Mathf.Sin(step * Mathf.PI);

            Vector3 barrelPosition = barrel.localPosition;
            barrelPosition.x = Mathf.Lerp(barrelNominalX, barrelNominalX + recoilDistance, scaledStep);
            barrel.localPosition = barrelPosition;

            barrel.localScale = Vector3.Lerp(barrelNominalScale, barrelNominalScale * recoilScaleFactor, scaledStep);
        }
        else
        {
            Vector3 barrelPosition = barrel.localPosition;
            barrelPosition.x = barrelNominalX;
            barrel.localPosition = barrelPosition;

            barrel.localScale = barrelNominalScale;
        }
    }

    private bool TryFindTarget(out Enemy target)
    {
        target = null;

        float closestDistance = float.MaxValue;
        foreach (Enemy enemy in GameManager.I.enemy.Get().Values)
        {
            float distance = Vector3.Distance(enemy.transform.position, projectileSpawn.position);
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
        targetHeading = (Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg) + 90f;
        targetPitch = Mathf.Sin(vector.y) * Mathf.Rad2Deg;
    }

    private void SetHeadingAndPitch(float heading, float pitch)
    {
        currentHeading = heading;
        currentPitch = pitch;

        turningPlate.localEulerAngles = new Vector3(0, 0, heading);
        barrelPivot.localEulerAngles = new Vector3(0, Mathf.Clamp(Mathf.DeltaAngle(0, pitch), -14, 24.5f), 0);
    }

    private void Shoot(Vector3 vector)
    {
        double time = Time.timeAsDouble;
        if (time >= allowedFiringTime)
        {
            // TODO NO CARRYOVER
            allowedFiringTime = time + (1 / fireFrequency);
            lastFiringTime = time;
        }
        else
        {
            return;
        }

        Projectile bullet = Instantiate(cannonballPrefab, projectileSpawn.position, Quaternion.identity);
        bullet.rb.AddRelativeForce(vector * cannonballSpeed, ForceMode.VelocityChange);
        GameManager.I.projectile.Add(bullet);
    }
}
