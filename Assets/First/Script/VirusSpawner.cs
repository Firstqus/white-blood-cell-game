using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    public GameObject virusPrefab;
    public Transform[] spawnPoints;
    public float spawnTime = 2f;

    void Start()
    {
        InvokeRepeating("SpawnVirus", 1f, spawnTime);
    }

    void SpawnVirus()
    {
        int lane = Random.Range(0, spawnPoints.Length);
        Instantiate(virusPrefab, spawnPoints[lane].position, Quaternion.identity);
    }
}