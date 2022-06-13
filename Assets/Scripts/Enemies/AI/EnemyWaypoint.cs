using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemyWaypoint : MonoBehaviour
{
    public bool isLinear;
    public bool isRandom;

    public Transform pointA;
    public Transform pointB;

    public List<NextEnemyWaypoint> next;

    #region Space
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    public float space;
    #endregion Space

    public NextEnemyWaypoint SelectNextWaypoint(EnemyType enemyType)
    {
        List<NextEnemyWaypoint> possibleWaypoints = next.FindAll(waypoint => waypoint.enemyTypes.Contains(enemyType));

        float weightSum = 0;
        foreach (NextEnemyWaypoint possibleWaypoint in possibleWaypoints)
        {
            weightSum += possibleWaypoint.weight;
        }

        float selector = Random.Range(0, weightSum);
        NextEnemyWaypoint selected = null;
        weightSum = 0;
        foreach (NextEnemyWaypoint possibleWaypoint in possibleWaypoints)
        {
            bool selectorGreaterOrEqual = selector >= weightSum;
            weightSum += possibleWaypoint.weight;
            bool selectorLesserOrEqual = selector <= weightSum;
            if (selectorGreaterOrEqual && selectorLesserOrEqual)
            {
                selected = possibleWaypoint;
                break;
            }
        }

        if (selected == null)
        {
            throw new Exception($"Cannot select waypoint with enemy type: {enemyType}");
        }

        return selected;
    }

    public Vector3 GetPoint(float randomLerp, float endpointBackoff)
    {
        if (!isLinear)
        {
            return pointA.position;
        }

        if (isRandom)
        {
            randomLerp = Random.Range(0f, 1f);
        }


        Vector3 tempPointA = pointA.position;
        Vector3 tempPointB = pointB.position;
        float length = Vector3.Distance(tempPointA, tempPointB);

        if (length <= endpointBackoff * 2)
        {
            return Vector3.Lerp(tempPointA, tempPointB, 0.5f);
        }
        else
        {
            tempPointA = Vector3.MoveTowards(tempPointA, tempPointB, endpointBackoff);
            tempPointB = Vector3.MoveTowards(tempPointB, tempPointA, endpointBackoff);
            return Vector3.Lerp(tempPointA, tempPointB, randomLerp);
        }
    }
}

[Serializable]
public class NextEnemyWaypoint
{
    #region Space
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    #endregion Space

    public EnemyWaypoint waypoint;
    public bool isEnd;
    public bool isTeleporter;
    public List<EnemyType> enemyTypes;
    public float weight;
}