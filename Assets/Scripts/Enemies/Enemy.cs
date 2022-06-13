using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float movementHeight;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private int startingHealth;
    [SerializeField] private int coreDamage;
    [SerializeField] private int deathReward;

    private int currentHealth;

    private EnemyPath path;
    private double spawnTime = 0;

    private float currentHeading = 0;
    private float targetHeading = 0;

    private Vector3 targetOffset;

    public void Init(EnemyPath path)
    {
        this.path = path;
        spawnTime = Time.timeAsDouble;
        targetOffset = target.position - transform.position;


        if (path.TryGetPosition(0, speed, out Vector3 position, out Vector3 endpoint))
        {
            Vector3 vector = (endpoint - position).normalized;
            targetHeading = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
            currentHeading = targetHeading;
            transform.eulerAngles = new Vector3(0, currentHeading, 0);

            position.y = movementHeight;
            transform.position = position;
        }
    }

    public bool TryGetFutureTargetPosition(float futureDistance, out Vector3 position)
    {
        if (path.TryGetPosition((float)(Time.timeAsDouble - spawnTime) + futureDistance, speed, out position, out _))
        {
            position.y = movementHeight;
            position += targetOffset;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetTargetPosition()
    {
        return target.position;
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

        if (path.TryGetPosition((float)(Time.timeAsDouble - spawnTime), speed, out Vector3 position, out Vector3 endpoint))
        {
            Vector3 vector = (endpoint - position).normalized;
            targetHeading = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
            currentHeading = Mathf.MoveTowardsAngle(currentHeading, targetHeading, turnSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, currentHeading, 0);

            position.y = movementHeight;
            transform.position = position;
        }
        else
        {
            DamageCore();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
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
