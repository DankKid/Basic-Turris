using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<SpawnableEnemy> enemyPrefabs;
    [SerializeField] private bool debugging;
    [SerializeField] private EnemyPathGenerator pathGenerator;

    private void Start()
    {

    }

    private void Update()
    {
        if(debugging && Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("SPAWNDASDSADAS");
            EnemyPath path = pathGenerator.Generate(EnemyType.BoomBoxRoboBuddy, enemyPrefabs[0].endpointBackoff);
            if (path.TryGetPosition(0, 0, out Vector3 position, out _))
            {
                print("SPAWN");
                Enemy enemy = Instantiate(enemyPrefabs[0].prefab, position, Quaternion.identity);
                enemy.Init(path);
                GameManager.I.enemy.Add(enemy);
            }
        }
    }
}

[Serializable]
public class SpawnableEnemy
{
    public Enemy prefab;
    public float endpointBackoff;
}