using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private PointsManager pointManager;

    [Header("Movement")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] private float speedIncrement;
    [SerializeField] private float slowedScalar;
    [SerializeField] private float slowDuration;
    private Vector2 moveDirection;
    public bool isDead = false;

    [SerializeField] private float rotationSpeed;
    private Vector3 mousePos;
    private Vector2 mousePosition;

    [SerializeField] private float mouseDistanceLimit;
    [SerializeField] private Transform artificialTarget;
    [SerializeField] private float artificalDistance;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashForce = 3f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    
    [Header("Empower")]
    [SerializeField] private float empowerScalar;
    [Range(0, 1)] [SerializeField] private float empowerSizeScale;
    [SerializeField] private bool empowered = false;
    private float dashForceSave;
    private Vector3 originalScale;

    void Start()
    {
        dashForceSave = dashForce;
        originalScale = transform.localScale;
    }

    void Update() // process input
    {
        if (isDead) return;
        ProcessInputs();
    }

    void FixedUpdate() { // process physics calculations
        if (isDead) return;
        LookRotation(); // in fixed update to avoid jittering
        Move();
    }

    void ProcessInputs() {

        if (Input.GetKeyDown(KeyCode.Mouse0) && canDash) {
            StartCoroutine(Dash());
            FMODUnity.RuntimeManager.PlayOneShot("event:/MOLE DASH");
            ScreenShakeController.Instance.ShakeCamera(3f, .2f);
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (Vector3.Distance(mousePosition, transform.position) >= mouseDistanceLimit) {
            // moveDirection = new Vector2(moveX, moveY).normalized;
            // moveDirection = (mousePos - transform.position).normalized;
            moveDirection = (mousePosition - (Vector2)transform.position).normalized;
        }else {
            // artificialMousePos = 
            // moveDirection = (artificialMousePos - (Vector2)transform.position).normalized;
            moveDirection = ((Vector2)artificialTarget.position - (Vector2)transform.position).normalized;
        }
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

        if (empowered) {
            dashForce *= empowerScalar;
            // if (dashForce * empowerScalar > dashForceSave * empowerScalar) {
            //     dashForce *= dashForceSave * empowerScalar;
            // }
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * dashForce, moveDirection.y * moveSpeed * dashForce);
            Invoke("EmpowerOff", dashTime);
        }else {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * dashForce, moveDirection.y * moveSpeed * dashForce);
        }
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
            if (isDashing) {
                if (!empowered) {
                    float scaleX = transform.localScale.x * (1 + empowerSizeScale);
                    float scaleY = transform.localScale.y * (1 + empowerSizeScale);
                    gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1);
                }
                empowered = true;
                other.gameObject.GetComponent<Enemy>().Die();
                pointManager.enemyPointIncrement();
                FMODUnity.RuntimeManager.PlayOneShot("event:/BUGSQUISH");
            }else {
                StartCoroutine(Slowed());
            } 
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Obstacle") {
            if (empowered && isDashing) {
                Destroy(other.gameObject);
                pointManager.enemyPointIncrement();
                FMODUnity.RuntimeManager.PlayOneShot("event:/ROCKCRUSHING");
            }
        }
    }

    private IEnumerator Slowed() {
        float moveSpeedSave = moveSpeed;
        moveSpeed *= slowedScalar;
        yield return new WaitForSeconds(slowDuration);
        moveSpeed = moveSpeedSave;
    }

    void EmpowerOff() {
        empowered = false;
        gameObject.transform.localScale = originalScale;
        dashForce = dashForceSave;
    }
}
