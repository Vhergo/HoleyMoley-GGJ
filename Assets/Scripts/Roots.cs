using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rootsSpeed;
    [SerializeField] private float maxRootsSpeed;
    [SerializeField] private float speedIncrement;

    [SerializeField] private PlayerController player;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer mole;

    [SerializeField] private PointsManager points;

    [SerializeField] private GameObject tryAgain;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject highScore;
    [SerializeField] private GameObject currentScore;

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

        if (other.gameObject.tag == "Player") {
            maxRootsSpeed = 0;
            player.isDead = true;
            player.transform.localScale *= player.deathAnimScale;
            mole.sortingOrder = player.orderInLayer;
            anim.SetBool("isDead", player.isDead);
            FMODUnity.RuntimeManager.PlayOneShot("event:/MOLEDEATH");

            player.maxSpeed = 0;
            player.rb.velocity = Vector2.zero;
            // anim.enabled = false;

            points.UpdateSavedScore();

            Invoke("spawnUI", 2f);
            // Destroy(other.gameObject);
            // replace with animation
            // add lose screen
        }
    }

    void spawnUI() {
        tryAgain.SetActive(true);
        mainMenu.SetActive(true);

        highScore.SetActive(true);
        currentScore.SetActive(true);

        points.depthText.enabled = false;
        points.killText.enabled = false;
    }
}
