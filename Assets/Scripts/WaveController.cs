using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyTypes;
    [SerializeField] private bool debugging;
    [SerializeField] GameObject spawnPoint;

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
        if(debugging && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Enemy enemy = Instantiate(enemyTypes[0], spawnPoint.transform.position, Quaternion.identity);
            enemy.Init(points);
        }
    }
}
