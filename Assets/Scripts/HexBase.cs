using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBase : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public Turret Turret { get; private set; }

    private MeshRenderer meshRenderer;
    private Material nominalMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        nominalMaterial = meshRenderer.material;
    }

    public void Highlight(Material highlightMaterial)
    {
        meshRenderer.material = highlightMaterial;
    }
    public void Unhighlight()
    {
        meshRenderer.material = nominalMaterial;
    }

    public bool TryConstructTurret(Turret turretPrefab)
    {
        if (IsOccupied)
        {
            return false;
        }
        IsOccupied = true;

        Turret = Instantiate(turretPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Turret.Init(this);

        return true;
    }

    public bool TryDeconstructTurret()
    {
        if (!IsOccupied || Turret.IsDeconstructing)
        {
            return false;
        }

        Turret.Deconstruct();

        return true;
    }

    public void FinishDeconstruct()
    {
        IsOccupied = false;
    }
}
