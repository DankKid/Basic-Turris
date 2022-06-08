using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    [SerializeField] List<GameObject> points = new List<GameObject>();
    int index = 0;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
        
        this.transform.LookAt(points[index].transform, Vector3.up);
        this.transform.position = Vector3.MoveTowards(this.transform.position, points[index].transform.position, speed * 0.5f);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyPoints")
        {
            
            index++;
            if (index == points.Count)
            {
                Destroy(this.gameObject);//Add Points before
            }
        }
        
    }

}
