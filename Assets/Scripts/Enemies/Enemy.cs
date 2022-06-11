using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movementHeight;
    [SerializeField] float speed;
    [SerializeField] int startingHealth;

    private int pointIndex = 0;
    private int currentHealth;

    private List<Transform> points;

    public void Init(List<Transform> points)
    {
        this.points = points;
    }

    public Vector3 GetFuturePosition(float futureDistance)
    {
        // Handle case of last point
        return transform.position;
    }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }

        //TODO REMOVE RB ON THIS WHEN USING LERP FOR EVERYING

        Transform target = points[pointIndex];
        Vector3 targetPosition = target.position;
        Vector3 position = transform.position;

        /*
        Vector3 vector = (targetPosition - position).normalized;
        transform.eulerAngles = new Vector3(0, Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg, 0);

        Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        newPosition.y = movementHeight;
        transform.position = newPosition;
        */
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyPoints"))
        {
            pointIndex++;
            if (pointIndex == points.Count)
            {
                Die();
            }
        }
        
        if(other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile.collateralHits > 0)
            {
                //currentHealth -= projectile.damage;
                projectile.collateralHits--;
            }
            if (projectile.collateralHits <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void Die()
    {
        GameManager.I.enemy.Remove(this);
        Destroy(gameObject);
    }
}
