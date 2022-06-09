using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : Turret
{
    // Pitch: -14 to 24.5

    private GameManager manager;

    [SerializeField] private Transform turningPlate;
    [SerializeField] private Transform barrelPivot;
    [SerializeField] private Transform projectileSpawn;

    public void Init(GameManager manager)
    {
        this.manager = manager;
    }


    private void Aim(float heading, float pitch)
    {
        turningPlate.eulerAngles = new Vector3(0, 0, heading);
        barrelPivot.eulerAngles = new Vector3(0, Mathf.Clamp(pitch, -14, 24.5f), 0);
    }
}
