using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerPrefab;

    [NonSerialized] public PlayerManager player;

    private void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 50, 0), Quaternion.identity);
    }
}
