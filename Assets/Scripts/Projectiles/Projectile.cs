using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int collateralHits;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}
