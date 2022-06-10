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
    [SerializeField] private Material hexBaseHighlightMaterial;
    [SerializeField] private float reachDistance;

    // Public
    public WaveManager waveManager;

    // Public Hidden
    [NonSerialized] public PlayerManager player;
    [NonSerialized] public Dictionary<int, Enemy> enemies;
    [NonSerialized] public Queue<(double, Projectile)> projectiles;

    private HexBase highlightedHexBase = null;

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 50, 0), Quaternion.identity);
        player.Init(this);
        enemies = new();
        projectiles = new();
    }

    private void Update()
    {
        UpdateConstruction();
        UpdateProjectiles();
    }

    private void UpdateConstruction()
    {
        HexBase hexBase = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance))
        {
            if (hit.transform.TryGetComponent(out hexBase))
            {
                hexBase.TryInit(this);

                if (Input.GetMouseButtonDown(1))
                {
                    hexBase.TryConstructTurret(turretPrefabs[0]);
                }
                else if (Input.GetMouseButtonDown(2))
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
                    if (Input.GetMouseButtonDown(2))
                    {
                        hexBase.TryDeconstructTurret();
                    }
                }
            }
        }

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

    private void UpdateProjectiles()
    {
        while (projectiles.Count > 0 && (Time.timeAsDouble - projectiles.Peek().Item1 > 2f))
        {
            Projectile bullet = projectiles.Dequeue().Item2;
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
        }
    }

    public void AddProjectile(Projectile projectile)
    {
        projectiles.Enqueue((Time.timeAsDouble, projectile));
    }
}
