using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpd = 6f;
    public InputActions playerControls;
    private InputAction move;
    private InputAction fire;

    public GameObject projectilePrefab;

    private bool canCast=true;
    Vector3 moveDirection = new Vector3();

    public float shootCooldown = 0.3f;

    private void Awake() {
        playerControls = new InputActions();
    }

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector3>();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(moveDirection.x * moveSpd, 0, moveDirection.z * moveSpd);
    }

    private void Fire(InputAction.CallbackContext context) {
        if(canCast) {
            StartCoroutine(castTimer(shootCooldown));
            Debug.Log("Player Shot projectile");

            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector3.right * 2f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(2000);
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
}

