using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridEditor : MonoBehaviour
{
    [SerializeField] private bool newGrid;
    [SerializeField] private bool loadGrid;

    [SerializeField] private int radius;
    [SerializeField] private HexGrid grid;
    [SerializeField] private Transform gridTransform;

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<Material> materials;

    [SerializeField] private TMPro.TMP_Text selectedMaterialText;

    [SerializeField] private List<Vector2Int> hexPositions;
    [SerializeField] private List<GameObject> hexes;

    public Material selectedMaterial;

    private void Awake()
    {
        selectedMaterial = materials[0];
    }

    private void Update()
    {
        gridTransform.position = Vector3.zero;
        Vector3 scale = gridTransform.localScale;
        gridTransform.localScale = Vector3.one;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedMaterial = materials[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedMaterial = materials[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedMaterial = materials[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedMaterial = materials[3];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedMaterial = materials[4];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedMaterial = materials[5];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedMaterial = materials[6];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedMaterial = materials[7];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedMaterial = materials[8];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedMaterial = materials[9];
        }

        selectedMaterialText.text = selectedMaterial.name.ToString();


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int index = hexes.IndexOf(hit.transform.gameObject);
            if (Input.GetMouseButton(0))
            {
                Replace(index, 0);
            }
            else if (Input.GetMouseButton(1))
            {
                Replace(index, 1);
            }
        }

        Vector3 center = GetCenter();
        gridTransform.position = new Vector3(-center.x * scale.x, 0, -center.z * scale.z);
        gridTransform.localScale = scale;
    }

    private void Replace(int index, int prefab)
    {
        GameObject oldHex = hexes[index];
        Destroy(oldHex);
        hexes.RemoveAt(index);

        GameObject newHex;

        grid.grid[index].prefab = prefabs[prefab];
        grid.grid[index].material = selectedMaterial;

        switch (prefab)
        {
            case 0:
                break;
            case 1:
                grid.grid[index].yOffset = -0.175f;
                break;
            default:
                return;
        }

        newHex = InstantiateHex(grid.grid[index]);

        hexes.Insert(index, newHex);
    }

    private void OnValidate()
    {
        NewGrid();

        if (loadGrid)
        {
            loadGrid = false;

            LoadGrid();
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

            LoadGrid();
        }
    }

    private void LoadGrid()
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
            InstantiateHex(hex);
        }

        Vector3 center = GetCenter();
        gridTransform.position = new Vector3(-center.x * scale.x, 0, -center.z * scale.z);
        gridTransform.localScale = scale;
    }

    private Vector3 GetCenter()
    {
        return GetHexPos(grid.grid[grid.grid.Count / 2].coords);
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
