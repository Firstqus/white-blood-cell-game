using UnityEngine;

// วางสคริปต์นี้บน GameObject ว่างๆ ใน Scene
public class CytokineSpawner : MonoBehaviour
{
    public static CytokineSpawner Instance;

    [Header("Cytokine Prefabs")]
    public GameObject disinfectantPrefab;
    public GameObject vitaminPrefab;
    public GameObject atpBoostPrefab;
    public GameObject speedDownPrefab;

    [Header("Spawn Area")]
    public float minX = -2.5f;
    public float maxX =  3.5f;
    public float spawnY = 5f;

    [Header("Time-based Drop")]
    public bool  enableTimedDrop = true;
    public float dropInterval    = 20f;

    [Header("Kill-based Drop")]
    [Tooltip("% โอกาสหล่นต่อ kill (0-100)")]
    public float dropChancePercent = 15f;

    [Header("Drop Weights")]
    public int weightDisinfectant = 2;
    public int weightVitamin      = 3;
    public int weightATPBoost     = 4;
    public int weightSpeedDown    = 2;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (enableTimedDrop)
            InvokeRepeating(nameof(SpawnRandom), dropInterval, dropInterval);
    }

    public void OnEnemyKilled()
    {
        if (Random.Range(0f, 100f) < dropChancePercent)
            SpawnRandom();
    }

    public void SpawnRandom()
    {
        GameObject prefab = PickRandomPrefab();
        if (prefab == null) return;
        float x = Random.Range(minX, maxX);
        Instantiate(prefab, new Vector3(x, spawnY, 0), Quaternion.identity);
    }

    GameObject PickRandomPrefab()
    {
        int total = weightDisinfectant + weightVitamin + weightATPBoost + weightSpeedDown;
        int roll  = Random.Range(0, total);

        if (roll < weightDisinfectant) return disinfectantPrefab;
        roll -= weightDisinfectant;
        if (roll < weightVitamin)      return vitaminPrefab;
        roll -= weightVitamin;
        if (roll < weightATPBoost)     return atpBoostPrefab;
        return speedDownPrefab;
    }
}