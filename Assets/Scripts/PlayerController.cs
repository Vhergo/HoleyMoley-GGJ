using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;

    [SerializeField] private float rotationSpeed;
    private Vector3 mousePos;
    private Vector2 mousePosition;

    [Header("Dash")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        
    }

    void Update() // process input
    {
        ProcessInputs();
        // LookRotation();
    }

    void FixedUpdate() { // process physics calculations
        LookRotation();
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
        moveDirection = (mousePos - transform.position).normalized;
        // moveDirection = (mousePosition - (Vector2)transform.position).normalized;
    }

    void Move() {
        if (isDashing) {
            // rb.velocity = new Vector2(moveDirection.x * moveSpeed * dashForce, moveDirection.y * moveSpeed * dashForce);
            rb.velocity = moveDirection * moveSpeed * dashForce;
            return;
        }

        // rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        rb.velocity = moveDirection * moveSpeed;
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * dashForce, moveDirection.y * moveSpeed * dashForce);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
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
}
