using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Prefabs
    [SerializeField] private PlayerManager playerPrefab;
    [SerializeField] private List<Turret> turretPrefabs;

    // Config
    [SerializeField] private float reachDistance;

    // Public
    [NonSerialized] public PlayerManager player;

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 50, 0), Quaternion.identity);
    }

    private void Update()
    {
        UpdatePlacement();
    }

    private void UpdatePlacement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, reachDistance))
            {
                if (hit.transform.TryGetComponent(out HexBase hexBase))
                {
                    hexBase.TryPlaceTurret(turretPrefabs[0]);
                }
            }
        }
    }
}
