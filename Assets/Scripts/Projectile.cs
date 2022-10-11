using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;

    public GameObject blastDmgEffect;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(transform.position.magnitude > 35f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(float force)
    {
        rb.AddForce(Vector3.right * force);
    }

    void OnCollisionEnter(Collision other)
    {
        BossHealth e = other.collider.GetComponent<BossHealth>();
        if (e != null)
        {
            e.TakeDamage();
            GameObject effectObject = Instantiate(blastDmgEffect, rb.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
