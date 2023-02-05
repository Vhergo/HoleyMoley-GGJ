using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float emittingDelay;
    [SerializeField] private float loopDelay;

    [Header("Hole")]
    [SerializeField] private GameObject moleHole;
    [SerializeField] private float holeDuration;

    float leftConstraint = Screen.width;
    float rightConstraint = Screen.width;

    float buffer = 0f;
    Camera cam;
    float distanceZ;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        distanceZ = Mathf.Abs(cam.transform.position.z + transform.position.z);
        leftConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, distanceZ)).x;
        rightConstraint = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, distanceZ)).x;
    }

    private void FixedUpdate() {
        if (transform.position.x < leftConstraint - buffer)
        {
            tr.emitting = false;
            // transform.position = new Vector3(rightConstraint - .10f, transform.position.y, transform.position.z);
            Invoke("LoopLeft", loopDelay);
            Invoke("EnableEmitting", emittingDelay);
        }

        if (transform.position.x > rightConstraint)
        {
            tr.emitting = false;
            // transform.position = new Vector3(leftConstraint, transform.position.y, transform.position.z);
            Invoke("LoopRight", loopDelay);
            Invoke("EnableEmitting", emittingDelay);
        }
    }

    void LoopLeft() {
        SpawnHole();
        FMODUnity.RuntimeManager.PlayOneShot("event:/MOLEHOLE");
        transform.position = new Vector3(rightConstraint - .10f, transform.position.y, transform.position.z);
    }

    void LoopRight() {
        SpawnHole();
        FMODUnity.RuntimeManager.PlayOneShot("event:/MOLEHOLE");
        transform.position = new Vector3(leftConstraint, transform.position.y, transform.position.z);
    }

    void SpawnHole() {
        
        GameObject hole = Instantiate(moleHole, transform.position, Quaternion.identity);
        Destroy(hole, holeDuration);
    }

    void EnableEmitting() {
        tr.emitting = true;
    }
}