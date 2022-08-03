using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject collisionParticle;


    public int damage;
    public int collateralHits;

    private void Update()
    {
        if (Vector3.Distance(transform.position, GameManager.I.sphereCenter) >= GameManager.I.sphereRadius)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Untagged"))
        {

            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        Instantiate(collisionParticle, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

}
