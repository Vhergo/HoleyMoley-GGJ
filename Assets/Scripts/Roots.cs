using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rootsSpeed;
    [SerializeField] private float rootsDelay;
    [SerializeField] private Transform playerPos;
    [SerializeField] private bool canMove = false;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) rb.velocity = Vector2.down.normalized * rootsSpeed;
    }
}
