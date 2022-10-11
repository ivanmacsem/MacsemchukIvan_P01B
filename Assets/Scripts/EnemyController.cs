using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource audioSource;

    public BossHealth health;
    public BossBeam beam;
    private bool stage2 = false;
    public float stage1Spd = 6f;

    public float stage2Spd = 3f;
    private int basicMoveStage = 0;
    public float curTime = 0f;
    public GameObject[] arms;

    public Vector3 pointPicked;

    private bool pointReached = true;

    private Vector3 moveDirection;

    private bool canCast = true;

    public float beamCooldown = 5f;

    private bool canCreate = true;

    public float createCooldown = 15f;

    public GameObject motherlingPrefab;

    public ParticleSystem DmgEffect;

    public AudioClip DmgSound;
    public ParticleSystem DeathEffect;
    public AudioClip DeathSound;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<BossHealth>();
        health.Damaged += OnTakeDamage;
        health.StartStageTwo += OnStageTwo;
        health.Killed += OnKill;
    }

    void onDisable(){
        health.Damaged -= OnTakeDamage;
        health.StartStageTwo -= OnStageTwo;
        health.Killed -= OnKill;
    }

    void Update()
    {
        if(!stage2){           //stage 1
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
            if(canCreate){
                StartCoroutine(createTimer(createCooldown));
                Vector3 spawnPoint = new Vector3(13f, 0, Random.Range(-10.6f, 10.2f));
                GameObject motherlingObject = Instantiate(motherlingPrefab, spawnPoint, Quaternion.identity);
                motherlingObject.transform.Rotate(0,180,0);
            }
        }
        if(canCast) {
            StartCoroutine(castTimer(beamCooldown));
            beam.Fire(1.5f, true);
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
    IEnumerator createTimer(float timer){
        canCreate = false;
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        canCreate = true;
    }

    void OnTakeDamage(){
        DmgEffect.Play();
        audioSource.PlayOneShot(DmgSound);
    }

    void OnStageTwo(){
        stage2 = true;
        foreach(GameObject g in arms){
            g.SetActive(false);
        }
    }

    void OnKill(){
        DeathEffect.transform.position = rb.position;
        DeathEffect.Play();
        audioSource.PlayOneShot(DeathSound);
        gameObject.SetActive(false);
    }
}
