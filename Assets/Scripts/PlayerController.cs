using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedIncrement;
    private Vector2 moveDirection;

    [SerializeField] private float rotationSpeed;
    private Vector3 mousePos;
    private Vector2 mousePosition;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashForce = 3f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    
    [SerializeField] private float dashCharge; // max 3? more dashes == further dash

    // [Header("Misc")]

    void Start()
    {
        
    }

    void Update() // process input
    {
        ProcessInputs();
    }

    void FixedUpdate() { // process physics calculations
        LookRotation(); // in fixed update to avoid jittering
        Move();
    }

    void ProcessInputs() {

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
            FMODUnity.RuntimeManager.PlayOneShot("event:/MOLE DASH");
            ScreenShakeController.Instance.ShakeCamera(3f, .2f);
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        // moveDirection = new Vector2(moveX, moveY).normalized;
        // moveDirection = (mousePos - transform.position).normalized;
        moveDirection = (mousePosition - (Vector2)transform.position).normalized;
    }

    void Move() {
        if (isDashing) {
            rb.velocity = moveDirection * moveSpeed * dashForce;
            return;
        }

        IncrementSpeed();
        rb.velocity = moveDirection * moveSpeed;
    }

    void IncrementSpeed() {
        moveSpeed += speedIncrement * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, maxSpeed);
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", isDashing);
        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * dashForce, moveDirection.y * moveSpeed * dashForce);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        anim.SetBool("isDashing", isDashing);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void LookRotation() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            print("COLLIDED");
            Destroy(other.gameObject);
        }
    }
    // void OnCollisionEnter2D(Collision2D other) {
    //     if (other.gameObject.tag == "Obstacle") {
    //         print("CRASHED");
    //     }
    // }
}
