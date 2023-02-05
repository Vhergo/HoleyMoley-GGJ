using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform background1, background2;
    // [SerializeField] private 
    [SerializeField] private Transform cam;
    [SerializeField] private float size;

    void Start()
    {
        size = Mathf.Abs(background1.position.y - background2.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < size) {
            if (transform.position.y < background2.position.y) {
                Vector3 newPos = new Vector3(background1.position.x, background2.position.y - size, background1.position.z);
                background1.position = newPos;
                SwitchBackground();
            }

            if (transform.position.y >= background1.position.y) {
                Vector3 newPos = new Vector3(background2.position.x, background1.position.y + size, background2.position.z);
                background2.position = newPos;
                SwitchBackground();
            }
        }
    }

    void SwitchBackground() {
        Transform temp = background1;
        background1 = background2;
        background1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1; 
        background2 = temp;
        background2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2; 
    }
}
