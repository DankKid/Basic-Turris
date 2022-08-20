using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    int type;
    float timer;
    public List<GameObject> meshes = new List<GameObject>();
    bool collected = false;
    float playerFireFreq, playerBulletSpeed, playerWalkSpeed, playerRunSpeed, playerJumpSpeed, playerBulletDamage;
    PlayerManager pManager;
    bool timerStarted;
    [Range(1, 2)] public float gunMult, speedMult, damageMult; 
    void Awake()
    {
        
        type = Random.Range(0, 2);


        switch (type)
        {
            case 0:
                meshes[0].SetActive(true);
                break;
            case 1:
                meshes[1].SetActive(true);
                break;
            case 2:
                meshes[2].SetActive(true);
                break;
            case 3:
                meshes[3].SetActive(true);
                break;
            case 4:
                meshes[4].SetActive(true);
                break;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;
            pManager = other.gameObject.GetComponent<PlayerManager>();

            OriginalStat(pManager);




            if(type == 0)
            {
                StartTimer();
                pManager.fireFrequency *= gunMult;
                pManager.bulletSpeed *= gunMult;
                //Increase Fire Frequency and Bullet Speed?
            }
            if (type == 1)
            {
                StartTimer();
                pManager.walkingSpeed *= speedMult;
                pManager.runningSpeed *= speedMult;
                pManager.jumpSpeed *= speedMult;
                //Increase Movement Speeds
            }
            if (type == 2)
            {
                StartTimer();
                //Bullet Damage
                pManager.bulletPrefab.GetComponent<Projectile>().damage *= damageMult;
            }



        }
    }

    void StartTimer()
    {
        timer = 15;
        timerStarted = true;
    }

    void ResetPower(PlayerManager pManager)
    {
        pManager.fireFrequency = playerFireFreq;
        pManager.bulletSpeed = playerBulletSpeed;

        pManager.walkingSpeed = playerWalkSpeed;
        pManager.runningSpeed = playerRunSpeed;
        pManager.jumpSpeed = playerJumpSpeed;

        pManager.bulletPrefab.GetComponent<Projectile>().damage = (int) playerBulletDamage;
    }

    void OriginalStat(PlayerManager pManager)
    {
        playerFireFreq = pManager.fireFrequency;
        playerBulletSpeed = pManager.bulletSpeed;

        playerWalkSpeed = pManager.walkingSpeed;
        playerRunSpeed = pManager.runningSpeed;
        playerJumpSpeed = pManager.jumpSpeed;

        playerBulletDamage = pManager.bulletPrefab.GetComponent<Projectile>().damage;
    }



    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else
        {
            if (pManager != null && timerStarted)
            {
                ResetPower(pManager);
                
            }
        }
    }
}
