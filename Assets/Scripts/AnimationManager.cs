using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Vector2 mousePos;
    [SerializeField] private SpriteRenderer sr;

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= transform.position.x) {
            sr.flipX = true;
        }else {
            sr.flipX = false;
        }
    }
}
