using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform spawnLane1;
    public Transform spawnLane2;
    public Transform spawnLane3;

    [Header("Prefabs")]
    public GameObject virusPrefab;
    public GameObject bacteriaPrefab;
    public GameObject parasitePrefab;

    [Header("Hint UI")]
    public TextMeshProUGUI hintText;
    public float hintDuration    = 2f;
    public float delayAfterHints = 3f;

    [Header("Spawn Timing (ปรับโดย WaveManager)")]
    public float minSpawnGap   = 1f;
    public float maxSpawnGap   = 2.5f;
    public float roundInterval = 5f;

    [Header("Difficulty (ปรับโดย WaveManager)")]
    public float pathogenSpeed = 2f;
    [Tooltip("1 = ปกติ, 2+ = Swarm")]
    public int   swarmCount    = 1;

    PathogenType[] laneTypes  = new PathogenType[3];
    Transform[]    spawnPoints;
    readonly string[] laneNames = { "Lane 1", "Lane 2", "Lane 3" };

    void Start()
    {
        spawnPoints = new Transform[] { spawnLane1, spawnLane2, spawnLane3 };

        PathogenType[] all = { PathogenType.Virus, PathogenType.Bacteria, PathogenType.Parasite };
        for (int i = all.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (all[i], all[j]) = (all[j], all[i]);
        }
        laneTypes = all;

        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        yield return StartCoroutine(ShowAllHints());
        yield return new WaitForSeconds(delayAfterHints);
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnOneByOne());
            yield return new WaitForSeconds(roundInterval);
        }
    }

    IEnumerator SpawnOneByOne()
    {
        List<int> order = new List<int> { 0, 1, 2 };
        for (int i = order.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }

        foreach (int i in order)
        {
            if (spawnPoints[i] == null) continue;

            GameObject prefab = GetPrefab(laneTypes[i]);
            if (prefab != null)
            {
                for (int s = 0; s < swarmCount; s++)
                {
                    Vector3 pos = spawnPoints[i].position + Vector3.up * (s * 1.2f);
                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);

                    VirusMove mover = go.GetComponent<VirusMove>();
                    if (mover != null) mover.speed = pathogenSpeed;
                }
            }

            yield return new WaitForSeconds(Random.Range(minSpawnGap, maxSpawnGap));
        }
    }

    IEnumerator ShowAllHints()
    {
        if (hintText == null) yield break;

        for (int i = 0; i < 3; i++)
        {
            string enemy = laneTypes[i] switch
            {
                PathogenType.Virus    => "Virus → use CD8",
                PathogenType.Bacteria => "Bacteria → use Macrophage",
                PathogenType.Parasite => "Parasite → use Eosinophil",
                _                     => ""
            };
            hintText.text = laneNames[i] + ": " + enemy;
            hintText.gameObject.SetActive(true);
            yield return new WaitForSeconds(hintDuration);
            hintText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    GameObject GetPrefab(PathogenType type) => type switch
    {
        PathogenType.Virus    => virusPrefab,
        PathogenType.Bacteria => bacteriaPrefab,
        PathogenType.Parasite => parasitePrefab,
        _                     => null
    };
}