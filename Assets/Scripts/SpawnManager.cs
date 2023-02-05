using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles; //obstacles and enemies
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;

    [Range(0, 1)] [SerializeField] private float spawnMargine; // percentage of screen with
    [Range(0, 1)] [SerializeField] private float spawnVariation;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnRange;
    [SerializeField] private float initialSpawnDelay = 5f;
    [SerializeField] private float timeSinceLastSpawn;

    [SerializeField] private GameObject roots;
    [SerializeField] private float spawnDelay;

    private float leftLimit;
    private float rightLimit;

    private Vector2 spawnPos;
    private GameObject obstacleToSpawn;
    private GameObject spawnedObject;


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
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn < initialSpawnDelay) return;

        if (timeSinceLastSpawn >= spawnInterval && InSpawnRange()) {
            timeSinceLastSpawn = 0;
            initialSpawnDelay = 0;
            Spawn();
        }
    }

    void Spawn() {
        ChooseSpawnType();
        CalculateSpawnPosition();
        spawnedObject = Instantiate(obstacleToSpawn, spawnPos, Quaternion.identity);
    }

    private bool InSpawnRange() {
        if (spawnedObject == null) return true;
        if ((player.position.y - spawnedObject.transform.position.y) < spawnRange) return true;
        return false;
    }

    void CalculateSpawnPosition() {
        spawnPos = new Vector2 (Random.Range(leftLimit, rightLimit), player.position.y - spawnDistance);
    }

    void ChooseSpawnType() {
        int spawnIndex = Random.Range(0, obstacles.Length);
        obstacleToSpawn = obstacles[spawnIndex];
    }
}
