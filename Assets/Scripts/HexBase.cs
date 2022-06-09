using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBase : MonoBehaviour
{
    private GameManager manager;

    public bool IsOccupied { get; private set; } = false;

    public void Init(GameManager manager)
    {
        this.manager = manager;
    }

    public bool TryPlaceTurret(Turret turretPrefab)
    {
        if (IsOccupied)
        {
            return false;
        }
        IsOccupied = true;

        Turret turret = Instantiate(turretPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        turret.Init(manager);

        return true;
    }
}
