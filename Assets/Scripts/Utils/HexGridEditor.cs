using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridEditor : MonoBehaviour
{
    [SerializeField] private bool generateGrid;

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private int radius;
    [SerializeField] private HexGrid grid;
    [SerializeField] private Transform gridTransform;

    [SerializeField] private List<GameObject> hexes;

    private void OnValidate()
    {
        if (generateGrid)
        {
            generateGrid = false;

            hexes.ForEach(h => OnValidateDestroyer.DestroyQueue.Add(h));
            hexes.Clear();

            int rows = (radius * 2) - 1;
            int topRow = radius;
            for (int i = 0; i < radius; i++)
            {

            }
            for (int i = 0; i < radius - 1; i++)
            {
                
            }

            PlaceHex(Vector2Int.zero);

        }
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
