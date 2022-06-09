using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    [SerializeField] List<GameObject> points = new List<GameObject>();
    [SerializeField] int index = 0;
    [SerializeField] float speed;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GameObject parent = GameObject.Find("Points");
        Debug.Log(parent.transform.childCount);
        for(int i = 0; i < parent.transform.childCount - 1; i++)
        {
            points.Add(GameObject.Find(i.ToString()));
        }
    }

    void FixedUpdate()
    {
        if(currentHealth <= 0)
        {
            Destroy(this);
        }
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
        
        if(other.tag == "Projectile")
        {
            currentHealth -= other.GetComponent<DamageValues>().damage;
        }


    }

}
