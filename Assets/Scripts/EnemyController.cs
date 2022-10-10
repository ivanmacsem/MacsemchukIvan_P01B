using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource audioSource;

    public float health = 500f;
    public float stage1Spd = 6f;

    public float stage2Spd = 3f;
    private int basicMoveStage = 0;
    public float curTime = 0f;

    public Vector3 pointPicked;

    private bool pointReached = true;

    private Vector3 moveDirection;

    public ParticleSystem DmgEffect;

    public AudioClip DmgSound;
    public ParticleSystem DeathEffect;
    public AudioClip DeathSound;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(health > 200){           //stage 1
            curTime += Time.deltaTime;
            curTime %= 18;
            if(curTime<8){
                basicMoveStage=0;
            }
            else if(curTime<9){
                basicMoveStage=1;
            }
            else if(curTime<17){
                basicMoveStage=2;
            }
            else if(curTime<18){
                basicMoveStage=3;
            }

            switch (basicMoveStage)
            {
                case 0:
                    moveDirection=Vector3.back;
                break;
                case 1:
                    moveDirection=Vector3.left;
                break;
                case 2:
                    moveDirection=Vector3.forward;
                break;
                default:
                    moveDirection=Vector3.right;
                break;
            }
            rb.velocity = new Vector3(moveDirection.x * stage1Spd, 0, moveDirection.z * stage1Spd);
        }
        else{           //stage 2
            if(pointReached){
                pointPicked = new Vector3(Random.Range(13.48f, 19.48f), 0, Random.Range(-23.6f, 24.2f));
                moveDirection = (pointPicked - rb.position).normalized;
                pointReached = false;
            }
            else{
                rb.velocity = new Vector3(moveDirection.x * stage2Spd, 0, moveDirection.z * stage2Spd);
                if((rb.position - pointPicked).magnitude<0.8){
                    pointReached = true;
                }
            }
        }
    }

    public void TakeDamage(float dmg){
        health -= dmg;
        DmgEffect.Play();
        audioSource.PlayOneShot(DmgSound);
        if(health == 0){
            DeathEffect.transform.position = rb.position;
            DeathEffect.Play();
            audioSource.PlayOneShot(DeathSound);
            gameObject.SetActive(false);
        }
    }
}
