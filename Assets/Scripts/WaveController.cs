using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyTypes = new List<GameObject>();
    bool debugging = true;
    [SerializeField] GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(debugging && Input.GetKeyDown(KeyCode.Keypad1)){
            Instantiate(enemyTypes[0], spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
