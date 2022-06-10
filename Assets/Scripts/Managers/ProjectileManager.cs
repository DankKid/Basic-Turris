using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileManager : MonoBehaviour
{
    // Prefabs

    // Reference Config

    // Value Config

    // Public

    // Public NonSerialized
    [NonSerialized] public Queue<(double, Projectile)> projectiles;

    // Private

    private void Awake()
    {
        projectiles = new();
    }

    private void Update()
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

    public void Add(Projectile projectile)
    {
        projectiles.Enqueue((Time.timeAsDouble, projectile));
    }
}
