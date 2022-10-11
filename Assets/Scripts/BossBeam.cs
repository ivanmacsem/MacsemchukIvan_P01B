using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    public ParticleSystem chargeUp;
    private ParticleSystem particles;
    private BoxCollider bCollider;
    public AudioSource audioSource;
    public AudioClip beamSound;
    public AudioClip chargeUpSound;
    private bool isBoss; 

    void Awake(){
        particles = GetComponent<ParticleSystem>();
        bCollider = GetComponent<BoxCollider>();
    }
    public void Fire(float chargeTime, bool boss)
    {
        isBoss = boss;
        chargeUp.Play();
        StartCoroutine(timer(chargeTime));
    }

    IEnumerator timer(float timer){
        audioSource.PlayOneShot(chargeUpSound);
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        particles.Play();
        audioSource.PlayOneShot(beamSound);
        bCollider.enabled = true;
        timer = 1f;
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        bCollider.enabled = false;
    }

    void OnTriggerStay(Collider other){
        PlayerHealth e = other.GetComponent<PlayerHealth>();
        if(e!=null){
            if(isBoss){
                e.TakeDamage(3f);
            }
            else{
                e.TakeDamage(2f);
            }
        }
    }
}
