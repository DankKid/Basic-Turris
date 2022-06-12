using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridEditor : MonoBehaviour
{
    [SerializeField] private bool destroyGrid;
    [SerializeField] private bool generateGrid;

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private int radius;
    [SerializeField] private HexGrid grid;
    [SerializeField] private Transform gridTransform;

    [SerializeField] private List<GameObject> hexes;

    private void OnValidate()
    {
        if (destroyGrid)
        {
            destroyGrid = false;

            hexes.ForEach(h => OnValidateDestroyer.DestroyQueue.Add(h));
            hexes.Clear();

            grid.grid.Clear();
        }

        if (generateGrid)
        {
            generateGrid = false;

            int[] offsets = GetOffsets(radius);
            int y = (radius * 2) - 2;
            int row = radius - 1;
            for (int i = 0; i < radius; i++)
            {
                row++;
                int offset = offsets[i];
                for (int x = offset; x < row + offset; x++)
                {
                    PlaceHex(new Vector2Int(x, y));
                }
                y--;
            }
            for (int i = 0; i < radius - 1; i++)
            {
                row--;
                int offset = offsets[(radius - 2) - i];
                for (int x = offset; x < row + offset; x++)
                {
                    PlaceHex(new Vector2Int(x, y));
                }
                y--;
            }
        }
    }

    /*
    private GameObject GetCenter()
    {
        return grid.grid[grid.grid.Count / 2];
    }
    */

    private int[] GetOffsets(int radius)
    {
        // TODO REALLY, BRUH?
        int length = radius;
        if (radius % 2 == 0)
        {
            length++;
        }
        List<int> offsets = new();
        for (int i = 0; i < length; i++)
        {
            offsets.Add(i);
            offsets.Add(i);
        }
        if (radius % 2 == 0)
        {
            offsets.RemoveAt(0);
        }
        offsets = offsets.GetRange(0, radius);
        offsets.Reverse();
        return offsets.ToArray();
    }

    private void PlaceHex(Vector2Int coords)
    {
        grid.grid.RemoveAll(h => h.coords == coords);

        float xPos = coords.x * 1.73205080756f;
        float zPos = coords.y * 1.5f;
        if (coords.y % 2 == 1)
        {
            xPos += 0.86602540378f;
        }

        GameObject hex = Instantiate(prefabs[0], new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0), gridTransform);
        hexes.Add(hex);
        grid.grid.Add(new Hex()
        {
            coords = coords,
            prefab = prefabs[0],
        });
    }
}
