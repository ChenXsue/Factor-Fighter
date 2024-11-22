using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OperatorSpawner : MonoBehaviour
{
    public GameObject numberPrefab; // Assign the RandomNumber prefab here
    public float spawnInterval = 0.5f; // Time between spawns
    public float spawnRangeX = 5f; // Horizontal spawn range
    public float fallSpeed = 2f; // Speed of falling numbers

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnRandomNumber();
            spawnTimer = 0f;
        }
    }

    void SpawnRandomNumber()
    {
        Debug.Log("Spawning random number");
        // Generate a random number between 1 and 100
        int randomNumber = Random.Range(1, 101);

        // Determine the spawn position
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX), // Randomize X position
            transform.position.y, // Use the spawner's Y position
            0 // Z position (for 2D)
        );

        // Instantiate the number prefab
        GameObject spawnedNumber = Instantiate(numberPrefab, spawnPosition, Quaternion.identity);

        // Set the text to the random number
        TextMeshPro textComponent = spawnedNumber.GetComponent<TextMeshPro>();
        if (textComponent != null)
        {
            textComponent.text = randomNumber.ToString();
        }


        // Make it fall by moving downward
        // spawnedNumber.AddComponent<SpawnMovement>().fallSpeed = fallSpeed;
        SpawnMovement fallingObject = spawnedNumber.AddComponent<SpawnMovement>();
        fallingObject.fallSpeed = fallSpeed;
    }
}
