using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Hex Grid", menuName = "ScriptableObjects/HexGrid", order = 1)]
public class HexGrid : ScriptableObject
{
    public int radius;
    public List<Hex> grid = new();
}

[Serializable]
public class Hex
{
    public Vector2Int coords;
    public GameObject prefab;
    public Material material;
    public float yOffset;
}
