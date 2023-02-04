using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles; //obstacles and enemies
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance;
    [Range(0, 1)] [SerializeField] private float spawnMargine; // percentage of screen with
    [Range(0, 1)] [SerializeField] private float spawnVariation;

    private float leftLimit;
    private float rightLimit;

    private Vector2 obstacleSpawn;
    private GameObject currentSpawnObject;
    private GameObject previousSpawnObject;
    private Vector2 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        float distanceZ = Mathf.Abs(cam.transform.position.z + transform.position.z);
        leftLimit = cam.ScreenToWorldPoint(new Vector3(Screen.width * spawnMargine, 0, distanceZ)).x;
        rightLimit = cam.ScreenToWorldPoint(new Vector3(Screen.width * (1 - spawnMargine), 0, distanceZ)).x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn() {
        Instantiate(currentSpawnObject, spawnPos, Quaternion.identity);
    }
    
    void CalculateSpawnPosition() {
        Vector2 spawnPos = new Vector2 (Random.Range(leftLimit, rightLimit), player.position.y - spawnDistance);
    }

    void ChooseSpawnType() {
        int spawnIndex = Random.Range(0, obstacles.Length);
        previousSpawnObject = currentSpawnObject;
        currentSpawnObject = obstacles[spawnIndex];

    }
}
