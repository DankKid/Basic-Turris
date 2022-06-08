using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool generateGrid;
    [SerializeField] private bool checkGrid;

    [SerializeField] private Transform grid;
    [SerializeField] private GameObject hex;

    private readonly List<(GameObject, GameObject)> gridObjects = new();

    private void OnValidate()
    {
        if (checkGrid)
        {
            checkGrid = false;

            foreach ((GameObject wall, GameObject path) in gridObjects)
            {
                if (wall != null)
                {
                    Destroyer.DestroyOnUpdate.Add(path);
                }
                else
                {
                    if (path != null)
                    {
                        Destroyer.DestroyOnUpdate.Add(wall);
                    }
                }
            }
        }

        if (generateGrid)
        {
            generateGrid = false;

            foreach ((GameObject wall, GameObject path) in gridObjects)
            {
                if (wall != null)
                {
                    Destroyer.DestroyOnUpdate.Add(wall);
                }
                if (path != null)
                {
                    Destroyer.DestroyOnUpdate.Add(path);
                }
            }
            gridObjects.Clear();

            // x: 15.5*1.73205080756
            // z: -22.75: (15*1.5)?
            // ratio: 1.18007868132
            // -26.84679
            // -22.75
            // 2 2 2
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    float xPos = x * 1.73205080756f;
                    float zPos = z * 1.5f;
                    if (z % 2 == 1)
                    {
                        xPos += 0.86602540378f;
                    }

                    GameObject wall = Instantiate(hex, new Vector3(xPos, 0.35f, zPos), Quaternion.Euler(-90, 0, 0), grid);
  
                    GameObject path = Instantiate(hex, new Vector3(xPos, 0.1f, zPos), Quaternion.Euler(-90, 0, 0), grid);
                    path.transform.localScale = new Vector3(1, 1, 0.5f);

                    gridObjects.Add((wall, path));
                }
            }
        }
    }
}
