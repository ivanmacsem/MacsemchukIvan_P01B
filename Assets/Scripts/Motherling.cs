using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motherling : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource audioSource;
    public BossBeam beam;
    private bool canCast = true;
    public AudioClip DmgSound;
    public ParticleSystem DeathEffect;
    public float beamCooldown = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(canCast) {
            StartCoroutine(castTimer(beamCooldown));
            beam.Fire(1f);
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
        DeathEffect.Play();
        audioSource.PlayOneShot(DmgSound);
        gameObject.SetActive(false);
    }
}
