using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(transform.position.magnitude > 1000f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(float force)
    {
        rigidbody2d.AddForce(Vector3.right * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            Debug.Log("Enemy Hit!");
        }
        
        Destroy(gameObject);
    }
}
