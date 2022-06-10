using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    // Prefabs

    // Reference Config

    // Value Config

    // Public

    // Public NonSerialized

    // Private
    private Dictionary<int, Enemy> enemies;

    private void Awake()
    {
        enemies = new();
    }

    public void Add(Enemy enemy)
    {
        enemies.Add(enemy.gameObject.GetInstanceID(), enemy);
    }
    public void Remove(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject.GetInstanceID());
    }
    public Dictionary<int, Enemy> Get()
    {
        return enemies;
    }
}
