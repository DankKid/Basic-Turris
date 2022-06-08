using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Destroyer
{
    public static HashSet<GameObject> DestroyOnUpdate = new();

    static Destroyer()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        foreach (GameObject go in DestroyOnUpdate)
        {
            Object.DestroyImmediate(go);
        }
        DestroyOnUpdate.Clear();
    }
}
