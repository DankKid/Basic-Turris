using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class OnValidateDestroyer
{
    public static HashSet<GameObject> DestroyQueue = new();

    static OnValidateDestroyer()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        foreach (GameObject go in DestroyQueue)
        {
            Object.DestroyImmediate(go);
        }
        DestroyQueue.Clear();
    }
}
#endif