using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int health = 500;
    public event Action Damaged = delegate { };
    public event Action StartStageTwo = delegate { };
    public event Action Killed = delegate { };
    public void TakeDamage(){
        health -= 10;
        Damaged.Invoke();
        if(health == 200){
            StartStageTwo.Invoke();
        }
        if(health <= 0){
            Killed.Invoke();
        }
    }
}
