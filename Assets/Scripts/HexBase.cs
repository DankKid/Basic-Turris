using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBase : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    public bool TryPlaceTurret(Turret turretPrefab)
    {
        if (IsOccupied)
        {
            return false;
        }
        IsOccupied = true;

        Instantiate(turretPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

        return true;
    }
}
