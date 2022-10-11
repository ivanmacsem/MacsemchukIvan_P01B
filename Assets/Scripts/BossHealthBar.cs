using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Image img;
    public Sprite[] sprites;
    private int curSpriteidx = 0;
    public BossHealth health;

    void Start()
    {
        img = GetComponent<Image>();
        health.Damaged += onTakeDamage;
        health.Killed += onKill;
    }

    void onDisable(){
        health.Damaged -= onTakeDamage;
        health.Killed -= onKill;
    }
    void onTakeDamage()
    {
        if(curSpriteidx<49){
            curSpriteidx++;
        }
        img.sprite = sprites[curSpriteidx];
    }

    void onKill(){
        gameObject.SetActive(false);
    }
}
