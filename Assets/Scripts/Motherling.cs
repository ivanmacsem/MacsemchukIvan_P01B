using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motherling : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource audioSource;
    public BossBeam beam;
    public BossHealth bossHealth;
    private bool canCast = true;
    public AudioClip DmgSound;
    public GameObject DeathEffect;
    public float beamCooldown = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bossHealth.Killed += Kill;
    }
    void Update()
    {
        if(canCast) {
            StartCoroutine(castTimer(beamCooldown));
            beam.Fire(1.5f, false);
        }
    }

    IEnumerator castTimer(float timer){
        canCast = false;
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        canCast = true;
    }

    public void Kill(){
        GameObject deathEff = Instantiate(DeathEffect, rb.position, Quaternion.identity);
        deathEff.GetComponent<ParticleSystem>().Play();
        audioSource.PlayOneShot(DmgSound);
        gameObject.SetActive(false);
    }
}
