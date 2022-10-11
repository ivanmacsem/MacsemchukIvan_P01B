using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public AudioSource audioSource;
    public float moveSpd = 6f;

    public PlayerHealth health;
    public InputActions playerControls;
    private InputAction move;
    private InputAction fire;

    private InputAction reset;
    private InputAction exit;

    public GameObject projectilePrefab;

    private bool canCast=true;
    Vector3 moveDirection = new Vector3();

    public float shootCooldown = 0.3f;
    public ParticleSystem FireEffect;
    public AudioClip FireSound;

    private void Awake() {
        playerControls = new InputActions();
    }

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        reset = playerControls.UI.Reset;
        reset.Enable();
        reset.performed += Reset;

        exit = playerControls.UI.Exit;
        exit.Enable();
        exit.performed += Exit;
    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
        health.Damaged -= OnTakeDamage;
        health.Killed -= OnKill;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();
        health.Damaged += OnTakeDamage;
        health.Killed += OnKill;
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector3>();
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, 0, 5.7f), rb.position.y, Mathf.Clamp(rb.position.z, -24f, 24f));
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(moveDirection.x * moveSpd, 0, moveDirection.z * moveSpd);
    }

    private void Fire(InputAction.CallbackContext context) {
        if(canCast) {
            StartCoroutine(castTimer(shootCooldown));

            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector3.right * 2f - Vector3.back*0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(2000);
            FireEffect.Play();
            audioSource.PlayOneShot(FireSound);
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

    void OnTakeDamage(float dmg){
        //damaged animation and sound
    }

    void OnKill(){
        //killed animation and sound
        gameObject.SetActive(false);
    }

    private void Reset(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Exit(InputAction.CallbackContext context) {
        Application.Quit();
    }
}

