using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath
{
    private readonly List<Vector3> points;

    public EnemyPath(List<Vector3> points)
    {
        this.points = points;
    }

    public Vector3 GetPosition(float time, float speed)
    {
        return Vector3.zero;
    }
}