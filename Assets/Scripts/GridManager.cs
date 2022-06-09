using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    [SerializeField] private bool generateGrid;

    [SerializeField] private Transform grid;
    [SerializeField] private GameObject hex;
    [SerializeField] private Vector2Int size;
    [SerializeField] private string map;

    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material pathMaterial;

    [SerializeField] private List<GameObject> hexes = new();

    private void OnValidate()
    {
        if (generateGrid)
        {
            generateGrid = false;

            Vector3 position = grid.transform.position;
            Vector3 scale = grid.transform.localScale;

            grid.transform.position = Vector3.zero;
            grid.transform.localScale = Vector3.one;

            foreach (GameObject go in hexes)
            {
                if (go != null)
                {
                    Destroyer.DestroyOnUpdate.Add(go);
                }
            }

            TextAsset textFile = (TextAsset)Resources.Load("Maps/" + map);
            string[] rows = textFile.text.Split(Environment.NewLine);
            for (int y = 0; y < rows.Length; y++)
            {
                string row = rows[y].Replace(" ", "");
                for (int x = 0; x < row.Length; x++)
                {
                    float xPos = x * 1.73205080756f;
                    float zPos = y * 1.5f;
                    if (y % 2 == 1)
                    {
                        xPos += 0.86602540378f;
                    }

                    switch (row[x])
                    {
                        case 'X':
                            GameObject path = Instantiate(hex, new Vector3(xPos, 0.1f, zPos), Quaternion.Euler(-90, 0, 0), grid);
                            path.transform.localScale = new Vector3(1, 1, 0.5f);
                            hexes.Add(path);
                            path.GetComponent<MeshRenderer>().material = pathMaterial;
                            break;
                        case 'O':
                            GameObject wall = Instantiate(hex, new Vector3(xPos, 0.35f, zPos), Quaternion.Euler(-90, 0, 0), grid);
                            hexes.Add(wall);
                            wall.GetComponent<MeshRenderer>().material = wallMaterial;
                            break;
                    }
                }
            }

            grid.transform.position = position;
            grid.transform.localScale = scale;

            // x: 15.5*1.73205080756
            // z: -22.75: (15*1.5)?
            // ratio: 1.18007868132
            // -26.84679
            // -22.75
            // 2 2 2
        }
    }
}
