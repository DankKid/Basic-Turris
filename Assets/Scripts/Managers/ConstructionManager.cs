using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    // Prefabs
    [SerializeField] private List<Turret> turretPrefabs;

    // Reference Config
    [SerializeField] private Material hexBaseHighlightMaterial;

    // Value Config
    [SerializeField] private float reachDistance;

    // Public

    // Public NonSerialized

    // Private
    private HexBase highlightedHexBase = null;

    private void Update()
    {
        HexBase hexBase = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance) && GameManager.I.player.isBuilding && !GameManager.I.player.isPaused)
        {
            
                if (hit.transform.TryGetComponent(out hexBase))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hexBase.TryConstructTurret(turretPrefabs[0]);
                    Debug.Log("BUILD");
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        hexBase.TryDeconstructTurret();
                    }
                }
                else
                {
                    Turret turret = hit.transform.GetComponentInParent<Turret>();
                    if (turret != null)
                    {
                        hexBase = turret.HexBase;
                        if (Input.GetMouseButtonDown(1))
                        {
                            hexBase.TryDeconstructTurret();
                        }
                    }
                }
            
            
        }

        UpdateHexBaseHighlights(hexBase);
    }

    private void UpdateHexBaseHighlights(HexBase hexBase)
    {
        if (hexBase == null)
        {
            if (highlightedHexBase != null)
            {
                highlightedHexBase.Unhighlight();
                highlightedHexBase = null;
            }
        }
        else
        {
            if (highlightedHexBase != null && highlightedHexBase != hexBase)
            {
                highlightedHexBase.Unhighlight();
            }
            highlightedHexBase = hexBase;
            hexBase.Highlight(hexBaseHighlightMaterial);
        }
    }
}
