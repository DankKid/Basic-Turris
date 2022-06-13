using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float movementHeight;
    [SerializeField] private float speed;
    [SerializeField] private int startingHealth;
    [SerializeField] private int coreDamage;
    [SerializeField] private int deathReward;

    private int pointIndex = 0;
    private int currentHealth;

    private List<Transform> points;

    public void Init(List<Transform> points)
    {
        this.points = points;
    }

    public bool TryGetFuturePosition(float futureDistance, out Vector3 position)
    {
        // Handle case of last point
        position = target.position;
        return true;
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

        Vector3 vector = (targetPosition - position).normalized;
        transform.eulerAngles = new Vector3(0, Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg, 0);

        Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        newPosition.y = movementHeight;
        transform.position = newPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyPoints"))
        {
            pointIndex++;
            if (pointIndex == points.Count)
            {
                DamageCore();
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

    private void Die()
    {
        GameManager.I.enemy.Remove(this);
        GameManager.I.player.Coins += deathReward;
        Destroy(gameObject);
        //PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
    }

    private void DamageCore()
    {
        GameManager.I.enemy.Remove(this);
        GameManager.I.player.CoreHealth -= coreDamage;
        Destroy(gameObject);
    }
}
