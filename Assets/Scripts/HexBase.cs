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
        
        if(GameManager.I.player.Coins >= turretPrefab.constructionCost)
        {
            IsOccupied = true;

            Turret = Instantiate(turretPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Turret.Init(this);
            GameManager.I.player.Coins -= turretPrefab.constructionCost;

            return true;
        }

        return false;
    }

    public bool TryDeconstructTurret()
    {
        if (!IsOccupied || Turret.IsDeconstructing)
        {
            return false;
        }
        
        GameManager.I.player.Coins += Turret.deconstructionRefund;
        Turret.Deconstruct();

        return true;
    }

    public void FinishDeconstruct()
    {
        IsOccupied = false;
    }
}
