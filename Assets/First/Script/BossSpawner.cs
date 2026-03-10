using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject virusPrefab;

    public float spawnTime = 2f;

    public float lane1X = -1.44f;
    public float lane2X = 0.735f;
    public float lane3X = 2.9f;

    public float spawnY = 4.5f;

    void Start()
    {
        InvokeRepeating("SpawnVirus", 1f, spawnTime);
    }

    void SpawnVirus()
    {
        int lane = Random.Range(1, 4);

        float x = lane1X;

        if (lane == 2)
            x = lane2X;

        if (lane == 3)
            x = lane3X;

        Vector3 spawnPos = new Vector3(x, spawnY, 0);

        Instantiate(virusPrefab, spawnPos, Quaternion.identity);
    }
}