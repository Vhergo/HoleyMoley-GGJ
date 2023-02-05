using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void Die(float deathDelay = 2f) {
        anim.SetBool("isDead", true);
        Destroy(gameObject, deathDelay);
    }
}
