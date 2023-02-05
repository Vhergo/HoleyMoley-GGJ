using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Die(float deathDelay = 0) {
        Destroy(gameObject, deathDelay);
    }
}
