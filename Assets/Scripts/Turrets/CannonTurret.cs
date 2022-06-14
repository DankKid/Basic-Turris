using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : Turret
{
    // Pitch: -14 to 24.5

    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private Transform turningPlate;
    [SerializeField] private Transform barrelPivot;
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform projectileSpawn;

    [SerializeField] private float range;
    [SerializeField] private float cannonballSpeed;
    [SerializeField] private float fireFrequency;

    [SerializeField] private float recoilDistance;
    [SerializeField] private float recoilScaleFactor;
    [Range(0, 100)] [SerializeField] private float recoilTimePercentOfFirePeriod;

    [SerializeField] private float headingMoveSpeed;
    [SerializeField] private float pitchMoveSpeed;
    [SerializeField] private float headingFireThreshold;
    [SerializeField] private float pitchFireThreshold;

    [SerializeField] private bool smartPredictorEnabled;
    [SerializeField] private float smartPredictorIncrement;
    [SerializeField] private float smartPredictorMinTimeDelta;

    [SerializeField] private bool superAimEnabled;

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
    }

    public override void MainUpdate()
    {
        if (TryFindTarget(out Enemy target))
        {
            bool smartPredictorFailed = true;
            if (smartPredictorEnabled)
            {
                // Could add check until no more decreases

                // Have dictionary of targets and what point to place next shot

                float smartPredictorTime = range / cannonballSpeed;
                for (float t = 0; t <= smartPredictorTime; t += smartPredictorIncrement)
                {
                    if (!target.TryGetFutureTargetPosition(t, out Vector3 positionAtTime))
                    {
                        break;
                    }

                    float distanceAtTime = Vector3.Distance(projectileSpawn.position, positionAtTime);
                    float timeToReach = (distanceAtTime / cannonballSpeed);
                    if (superAimEnabled)
                    {
                        float timeToFire = Mathf.Max(0, (float)(allowedFiringTime - Time.timeAsDouble));
                        //(float headingAtTime, float pitchAtTime) = Aim(positionAtTime);
                        //float timeToMoveHeading = Mathf.Min(Mathf.Abs(Mathf.DeltaAngle(currentHeading, headingAtTime + headingFireThreshold)), Mathf.Abs(Mathf.DeltaAngle(currentHeading, headingAtTime - headingFireThreshold))) / headingMoveSpeed;
                        //float timeToMovePitch = Mathf.Min(Mathf.Abs(Mathf.DeltaAngle(currentPitch, pitchAtTime + pitchFireThreshold)), Mathf.Abs(Mathf.DeltaAngle(currentPitch, pitchAtTime - pitchFireThreshold))) / pitchMoveSpeed;
                        timeToReach += timeToFire; //Mathf.Max(timeToFire, timeToMoveHeading, timeToMovePitch);
                    }
                    float timeDelta = Mathf.Abs(t - timeToReach);
                    if (timeDelta <= smartPredictorMinTimeDelta)
                    {
                        if (distanceAtTime < range)
                        {
                            (targetHeading, targetPitch) = Aim(positionAtTime);
                            smartPredictorFailed = false;
                        }
                        break;
                    }
                }

                if (smartPredictorFailed)
                {
                    target = null;
                }
            }
            else
            {
                float projectileTravelTime = Vector3.Distance(projectileSpawn.position, target.GetTargetPosition()) / cannonballSpeed;
                if (target.TryGetFutureTargetPosition(projectileTravelTime, out Vector3 targetPosition) && Vector3.Distance(projectileSpawn.position, targetPosition) < range)
                {
                    (targetHeading, targetPitch) = Aim(targetPosition);
                }
                else
                {
                    target = null;
                }
            }
        }

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
            float distance = Vector3.Distance(enemy.GetTargetPosition(), projectileSpawn.position);
            if (distance < range && distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy;
            }
        }

        return target != null;
    }


    private (float, float) Aim(Vector3 targetPosition)
    {
        Vector3 vector = (targetPosition - projectileSpawn.position).normalized;
        float heading = (Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg) + 90f;
        float pitch = Mathf.Sin(vector.y) * Mathf.Rad2Deg;
        return (heading, pitch);
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

        Projectile bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        bullet.rb.AddRelativeForce(vector * cannonballSpeed, ForceMode.VelocityChange);
        GameManager.I.projectile.Add(bullet);
    }
}
