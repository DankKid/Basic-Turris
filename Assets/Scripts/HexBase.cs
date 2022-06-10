using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBase : MonoBehaviour
{
    private GameManager manager;

    public bool IsOccupied { get; private set; } = false;
    public Turret Turret { get; private set; }

    private bool init = false;

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

    public void TryInit(GameManager manager)
    {
        if (!init)
        {
            init = true;

            this.manager = manager;
        }
    }

    public bool TryConstructTurret(Turret turretPrefab)
    {
        if (IsOccupied)
        {
            return false;
        }
        IsOccupied = true;

        Turret = Instantiate(turretPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Turret.Init(manager, this);

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
