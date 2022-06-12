using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridEditor : MonoBehaviour
{
    [SerializeField] private bool newGrid;
    [SerializeField] private bool resizeGrid;

    [SerializeField] private int radius;
    [SerializeField] private HexGrid grid;
    [SerializeField] private Transform gridTransform;

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<Material> materials;

    [SerializeField] private List<Vector2Int> hexPositions;
    [SerializeField] private List<GameObject> hexes;

    private void Update()
    {
        gridTransform.position = Vector3.zero;
        Vector3 scale = gridTransform.localScale;
        gridTransform.localScale = Vector3.one;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int index = hexes.IndexOf(hit.transform.gameObject);
            if (Input.GetMouseButtonDown(0))
            {
                //Replace(index, 0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Replace(index, 1);
            }
        }

        Transform center = GetCenter();
        gridTransform.position = new Vector3(-center.position.x * scale.x, 0, -center.position.z * scale.z);
        gridTransform.localScale = scale;
    }

    /*
    private void Replace(int index, int prefab)
    {
        GameObject oldHex = hexes[index];
        Destroy(oldHex);
        hexes.RemoveAt(index);
        GameObject newHex;
        Vector3 pos = GetHexPos(hexPositions[index]);
        switch (prefab)
        {
            case 0:
                newHex = InstantiateHex(prefabs[prefab], pos);
                break;
            case 1:
                pos = new Vector3(pos.x, -0.175f, pos.z);
                newHex = InstantiateHex(prefabs[prefab], pos);
                break;
            default:
                return;
        }
        hexes.Insert(index, newHex);
        grid.grid[index].prefab = prefabs[prefab];
    }
    */

    private void OnValidate()
    {
        NewGrid();

        if (resizeGrid)
        {
            resizeGrid = false;

            Vector2Int coordOffset = new(radius - grid.radius, radius - grid.radius);
            Dictionary<Vector2Int, Hex> overrideWith = new();
            foreach (Hex hex in grid.grid)
            {
                hex.coords += coordOffset;
                overrideWith.Add(hex.coords, hex);
            }

            grid.radius = radius;

            grid.grid.Clear();

            int[] offsets = GetOffsets(radius);
            int y = (radius * 2) - 2;
            int row = radius - 1;
            for (int i = 0; i < radius; i++)
            {
                row++;
                int offset = offsets[i];
                for (int x = offset; x < row + offset; x++)
                {
                    grid.grid.Add(new Hex()
                    {
                        coords = new Vector2Int(x, y),
                        prefab = prefabs[0],
                        material = materials[0],
                        yOffset = 0
                    });
                }
                y--;
            }
            for (int i = 0; i < radius - 1; i++)
            {
                row--;
                int offset = offsets[(radius - 2) - i];
                for (int x = offset; x < row + offset; x++)
                {
                    grid.grid.Add(new Hex()
                    {
                        coords = new Vector2Int(x, y),
                        prefab = prefabs[0],
                        material = materials[0],
                        yOffset = 0
                    });
                }
                y--;
            }

            LoadGrid(overrideWith);
        }
    }

    private void NewGrid()
    {
        if (newGrid)
        {
            newGrid = false;

            grid.grid.Clear();

            grid.radius = radius;

            int[] offsets = GetOffsets(radius);
            int y = (radius * 2) - 2;
            int row = radius - 1;
            for (int i = 0; i < radius; i++)
            {
                row++;
                int offset = offsets[i];
                for (int x = offset; x < row + offset; x++)
                {
                    grid.grid.Add(new Hex()
                    {
                        coords = new Vector2Int(x, y),
                        prefab = prefabs[0],
                        material = materials[0],
                        yOffset = 0
                    });
                }
                y--;
            }
            for (int i = 0; i < radius - 1; i++)
            {
                row--;
                int offset = offsets[(radius - 2) - i];
                for (int x = offset; x < row + offset; x++)
                {
                    grid.grid.Add(new Hex()
                    {
                        coords = new Vector2Int(x, y),
                        prefab = prefabs[0],
                        material = materials[0],
                        yOffset = 0
                    });
                }
                y--;
            }

            LoadGrid(new Dictionary<Vector2Int, Hex>());
        }
    }

    private void LoadGrid(Dictionary<Vector2Int, Hex> overrideWith)
    {
        gridTransform.position = Vector3.zero;
        Vector3 scale = gridTransform.localScale;
        gridTransform.localScale = Vector3.one;

        hexes.ForEach(h => OnValidateDestroyer.DestroyQueue.Add(h));
        hexes.Clear();
        hexPositions.Clear();

        int[] offsets = GetOffsets(grid.radius);

        foreach (Hex hex in grid.grid)
        {
            Hex useHex = hex;
            if (overrideWith.ContainsKey(hex.coords))
            {
                useHex = overrideWith[hex.coords];
                /*
                int top = (grid.radius * 2) - 2;
                int distanceFromTop = top - useHex.coords.y;
                if (useHex.coords.y >= grid.radius - 1)
                {
                    useHex.coords.x += offsets[radius - distanceFromTop];
                }
                else
                {
                    useHex.coords.x += offsets[radius - useHex.coords.y];
                }
                */
            }

            InstantiateHex(useHex);
        }

        Transform center = GetCenter();
        gridTransform.position = new Vector3(-center.position.x * scale.x, 0, -center.position.z * scale.z);
        gridTransform.localScale = scale;
    }

    private Transform GetCenter()
    {
        return hexes[hexes.Count / 2].transform;
    }

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


    private Vector3 GetHexPos(Vector2Int coords)
    {
        float xPos = coords.x * 1.73205080756f;
        float zPos = coords.y * 1.5f;
        if (coords.y % 2 == 1)
        {
            xPos += 0.86602540378f;
        }
        return new Vector3(xPos, 0, zPos);
    }

    private GameObject InstantiateHex(Hex hex)
    {
        Vector3 position = GetHexPos(hex.coords);
        position.y += hex.yOffset;
        GameObject newHex = Instantiate(hex.prefab, position, Quaternion.Euler(-90, 0, 0), gridTransform);
        hexes.Add(newHex);
        hexPositions.Add(hex.coords);
        MeshRenderer meshRenderer = newHex.GetComponent<MeshRenderer>();
        meshRenderer.material = hex.material;
        return newHex;
    }
}
