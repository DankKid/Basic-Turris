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
    public WaveManager waveManager;

    // Public Hidden
    [NonSerialized] public PlayerManager player;
    [NonSerialized] public Dictionary<int, Enemy> enemies;
    [NonSerialized] public Queue<(double, Projectile)> projectiles;

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 50, 0), Quaternion.identity);
        player.Init(this);
        enemies = new();
        projectiles = new();
    }

    private void Update()
    {
        UpdatePlacement();
        UpdateProjectiles();
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
                    hexBase.Init(this);
                    hexBase.TryPlaceTurret(turretPrefabs[0]);
                }
            }
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
