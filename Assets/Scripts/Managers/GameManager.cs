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

    // Value Config

    // Public
    public WaveManager wave;
    public ProjectileManager projectile;
    public EnemyManager enemy;
    public ConstructionManager construction;

    // Public NonSerialized
    [NonSerialized] public PlayerManager player;

    // Private

    private void Awake()
    {
        AwakeSingleton();
    }

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 50, 0), Quaternion.identity);
    }
}
