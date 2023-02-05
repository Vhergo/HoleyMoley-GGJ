using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rootsSpeed;
    [SerializeField] private float maxRootsSpeed;
    [SerializeField] private float speedIncrement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IncrementSpeed();
        rb.velocity = Vector2.down.normalized * rootsSpeed;
    }

    void IncrementSpeed() {
        rootsSpeed += speedIncrement * Time.deltaTime;
        rootsSpeed = Mathf.Min(rootsSpeed, maxRootsSpeed);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Obstacle") {
            Destroy(other.gameObject);
        }
    }
}
