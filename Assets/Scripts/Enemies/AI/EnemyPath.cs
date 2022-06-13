using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath
{
    private readonly List<EnemyPathSegment> segments;

    public EnemyPath(List<EnemyPathSegment> segments)
    {
        this.segments = segments;
    }

    // Detect teleports in enemy code by looking for large position changes
    // Alternative: Look for end not matching the next segment's start
    public bool TryGetPosition(float time, float speed, out Vector3 position, out Vector3 endpoint)
    {
        float accumulatedTime = 0;
        foreach (EnemyPathSegment segment in segments)
        {
            bool timeGreaterOrEqual = time >= accumulatedTime;
            float lowerBound = accumulatedTime;
            accumulatedTime += segment.GetDistance() / speed;
            float upperBound = accumulatedTime;
            bool timeLesserOrEqual = time <= accumulatedTime;
            if (timeGreaterOrEqual && timeLesserOrEqual)
            {
                float step = Mathf.InverseLerp(lowerBound, upperBound, time);
                position = segment.Lerp(step);
                endpoint = segment.end;
                return true;
            }
        }

        position = Vector3.zero;
        endpoint = Vector3.zero;
        return false;
    }
}

public struct EnemyPathSegment
{
    public Vector3 start;
    public Vector3 end;

    public EnemyPathSegment(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    public float GetDistance()
    {
        return Vector3.Distance(start, end);
    }

    public Vector3 Lerp(float t)
    {
        return Vector3.Lerp(start, end, t);
    }
}