using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] int index = 0;
    [SerializeField] float speed;
    [SerializeField] int startHealth;
    [SerializeField] int currentHealth;

    private List<Transform> points;

    public void Init(List<Transform> points)
    {
        currentHealth = startHealth;
        this.points = points;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        Transform target = points[index];
        transform.LookAt(target, Vector3.up);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyPoints"))
        {
            index++;
            if (index == points.Count)
            {
                Destroy(gameObject);//Add Points before
            }
        }
        
        if(other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile.collateralHits > 0)
            {
                currentHealth -= projectile.damage;
                projectile.collateralHits--;
            }
            if (projectile.collateralHits <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
