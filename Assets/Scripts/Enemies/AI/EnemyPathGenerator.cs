using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPathGenerator : MonoBehaviour
{
    [SerializeField] private List<FirstEnemyWaypoint> firstWaypoints;

    [SerializeField] private bool debugEndpointBackoff;
    [SerializeField] private float randomLerpOverride;
 
    // TODO Implement teleporters
    public EnemyPath Generate(EnemyType enemyType, float endpointBackoff)
    {
        float randomLerp = Random.Range(0f, 1f);
        if (debugEndpointBackoff)
        {
            randomLerp = randomLerpOverride;
        }

        List<EnemyPathSegment> segments = new();

        FirstEnemyWaypoint first = SelectNextFirstWaypoint(enemyType);
        Vector3 start = first.GetPoint(randomLerp, endpointBackoff);
        NextEnemyWaypoint selected;

        while (true)
        {
            selected = first.SelectNextWaypoint(enemyType);

            if (!selected.isEnd)
            {
                Vector3 end = selected.waypoint.GetPoint(randomLerp, endpointBackoff);
                segments.Add(new EnemyPathSegment(start, end));
                start = end;
            }
            else
            {
                break;
            }
        }

        return new EnemyPath(segments);
    }

    public FirstEnemyWaypoint SelectNextFirstWaypoint(EnemyType enemyType)
    {
        List<FirstEnemyWaypoint> possibleFirstWaypoints = firstWaypoints.FindAll(
            firstWaypoint => firstWaypoint.next.Exists(
                waypoint => waypoint.enemyTypes.Contains(enemyType
        )));

        float weightSum = 0;
        foreach (FirstEnemyWaypoint firstWaypoint in possibleFirstWaypoints)
        {
            weightSum += firstWaypoint.weight;
        }

        float selector = Random.Range(0, weightSum);
        FirstEnemyWaypoint selected = null;
        weightSum = 0;
        foreach (FirstEnemyWaypoint firstWaypoint in possibleFirstWaypoints)
        {
            bool selectorGreaterOrEqual = selector >= weightSum;
            weightSum += firstWaypoint.weight;
            bool selectorLesserOrEqual = selector <= weightSum;
            if (selectorGreaterOrEqual && selectorLesserOrEqual)
            {
                selected = firstWaypoint;
                break;
            }
        }

        if (selected == null)
        {
            throw new Exception($"Cannot select first waypoint with enemy type: {enemyType}");
        }

        return selected;
    }

    /*
    public NextEnemyWaypoint FindPath(List<EnemyWaypoint> waypoints, EnemyType enemyType)
    {
        List<NextEnemyWaypoint> possiblePaths = new();
        float weightSum = 0;

        foreach (EnemyWaypoint path in paths)
        {
            foreach (NextEnemyWaypoint nextPath in path.next)
            {
                if (nextPath.enemyTypes.Contains(enemyType))
                {
                    possiblePaths.Add(nextPath);
                    weightSum += nextPath.weight;
                }
            }
        }

        float select
    }
    */


    //public bool TryGet
}
