using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth health;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        health.Damaged += onTakeDamage;
        health.Killed += onKill;
    }

    void onDisable(){
        health.Damaged -= onTakeDamage;
        health.Killed -= onKill;
    }
    void onTakeDamage(float dmg)
    {
        slider.value-=dmg;
    }

    void onKill(){
        gameObject.SetActive(false);
    }
}
