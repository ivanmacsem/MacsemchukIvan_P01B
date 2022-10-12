using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    public AudioManager audioM;

    public BossHealth health;
    public PlayerHealth playerHealth;
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
    private bool gameOver = false;

    public float createCooldown = 15f;

    public GameObject motherlingPrefab;

    public ParticleSystem DmgEffect;

    public AudioClip DmgSound;
    public ParticleSystem DeathEffect;
    public AudioClip DeathSound;
    public Image lossText;
    public Image winText;
    public Animator exc;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<BossHealth>();
        health.Damaged += OnTakeDamage;
        health.StartStageTwo += OnStageTwo;
        health.Killed += OnKill;
        playerHealth.Killed += OnPlayerKill;
    }

    void onDisable(){
        health.Damaged -= OnTakeDamage;
        health.StartStageTwo -= OnStageTwo;
        health.Killed -= OnKill;
        playerHealth.Killed -= OnPlayerKill;
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
        Vector3 spawnPoint = new Vector3(13f, 0, Random.Range(-10.6f, 10.2f));
        exc.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-spawnPoint.z*38.25f, -80f, 0f);
        exc.enabled = true;
        float temp = timer;
        while(timer > temp-2){
            yield return null;
            timer -= Time.deltaTime;
        }
        exc.enabled = false;
        GameObject motherlingObject = Instantiate(motherlingPrefab, spawnPoint, Quaternion.identity);
        motherlingObject.GetComponent<Motherling>().bossHealth = health;
        motherlingObject.GetComponent<Motherling>().audioSource = beam.audioSource;
        motherlingObject.GetComponent<Motherling>().beam.audioSource = beam.audioSource;
        motherlingObject.transform.Rotate(0,180,0);
        while(timer > 0){
            yield return null;
            timer -= Time.deltaTime;
        }
        if(!gameOver){
            canCreate = true;
        }
    }

    void OnTakeDamage(){
        DmgEffect.Play();
        audioM.Play(DmgSound);
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
        audioM.Play(DeathSound);
        winText.enabled = true;
        exc.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void OnPlayerKill(){
        gameOver = true;
        beam.audioSource.mute = true;
        lossText.enabled = true;
        exc.gameObject.SetActive(false);
    }
}
