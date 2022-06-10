using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private bool debugging;
    [SerializeField] private GameObject spawnPoint;

    private List<Transform> points;

    private void Start()
    {
        points = new();

        Transform pointsTransform = GameObject.Find("Points").transform;
        for (int i = 1; i < pointsTransform.childCount; i++)
        {
            points.Add(pointsTransform.GetChild(i).transform);
        }
    }

    private void Update()
    {
        if(debugging && Input.GetKey(KeyCode.Alpha1))
        {
            Enemy enemy = Instantiate(enemyPrefabs[0], spawnPoint.transform.position, Quaternion.identity);
            enemy.Init(points);
            GameManager.I.enemy.Add(enemy);
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.transform.position;
    }
}
