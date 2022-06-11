using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager I { get { return instance; } }
    private void AwakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton

    // Prefabs
    [SerializeField] private PlayerManager playerPrefab;
    [Space]

    // Reference Config
    [SerializeField] private Transform sphere;

    // Value Config

    // Public
    public WaveManager wave;
    public ProjectileManager projectile;
    public EnemyManager enemy;
    public ConstructionManager construction;

    // Public NonSerialized
    [NonSerialized] public PlayerManager player;
    [NonSerialized] public float sphereRadius;
    [NonSerialized] public Vector3 sphereCenter;

    // Private


    // When Adding Things, Make Sure To Set
    /*
     *  Projectiles:
     *      Tag: Projectile
     *      Layer: Ignore Raycast!!!!!!!!!!!
     *      Give an RB
     *          No Gravity
     *          Mass 1
     *          Drag 0
     *          Angular Drag 0
     *          Interpolate: Interpolate
     *          Collision Detection: Continuous
     *      Config Projectile Script
     *      Collider:
     *          Don't use mesh for performance
     *          Make a trigger collider
     *  Turrets:
     *      Tag: Turret
     *      Config Turret Script
     *  Enemies:
     *      Tag: Enemy
     *      Config Enemy Script
     *      Collider:
     *          Make a trigger collider
     *  
    */

    private void Awake()
    {
        AwakeSingleton();
    }

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 30, 0), Quaternion.identity);
        sphereRadius = sphere.transform.localScale.x * 0.0875f;
        sphereCenter = sphere.transform.position;
    }
}
