using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 200;
    public event Action<float> Damaged = delegate { };
    public event Action Killed = delegate { };
    public void TakeDamage(float dmg){
        health -= dmg;
        Damaged.Invoke(dmg);
        if(health <= 0){
            Killed.Invoke();
        }
    }
}
