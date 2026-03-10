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
    public float hintDuration = 2f;
    public float delayAfterHints = 3f;

    [Header("Spawn Timing")]
    public float minSpawnGap = 1f;   // เว้นระหว่าง lane อย่างน้อยกี่วิ
    public float maxSpawnGap = 2.5f; // เว้นระหว่าง lane สูงสุดกี่วิ
    public float roundInterval = 5f; // รอกี่วิก่อนจะปล่อยรอบถัดไป

    PathogenType[] laneTypes = new PathogenType[3];
    Transform[] spawnPoints;
    string[] laneNames = { "Lane 1", "Lane 2", "Lane 3" };

    void Start()
    {
        spawnPoints = new Transform[] { spawnLane1, spawnLane2, spawnLane3 };

        // สุ่มแบบไม่ซ้ำ — แต่ละ lane ได้ชนิดต่างกันแน่นอน
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
        // 1. แสดง hint ครบ 3 lane
        yield return StartCoroutine(ShowAllHints());

        // 2. รอหลัง hint สุดท้าย
        yield return new WaitForSeconds(delayAfterHints);

        // 3. เริ่ม spawn
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

    // ปล่อยทีละ lane โดยสุ่มลำดับและเว้นช่วงแต่ละตัว
    IEnumerator SpawnOneByOne()
    {
        // สุ่มลำดับ lane ที่จะออกรอบนี้
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
                Instantiate(prefab, spawnPoints[i].position, Quaternion.identity);

            // เว้นระยะสุ่มก่อนปล่อย lane ถัดไป
            float gap = Random.Range(minSpawnGap, maxSpawnGap);
            yield return new WaitForSeconds(gap);
        }
    }

    IEnumerator ShowAllHints()
    {
        if (hintText == null) yield break;

        for (int i = 0; i < 3; i++)
        {
            string enemy = laneTypes[i] switch
            {
                PathogenType.Virus    => "Virus → ใช้ CD8",
                PathogenType.Bacteria => "Bacteria → ใช้ Macrophage",
                PathogenType.Parasite => "Parasite → ใช้ Eosinophil",
                _                     => ""
            };

            hintText.text = laneNames[i] + ": " + enemy;
            hintText.gameObject.SetActive(true);

            yield return new WaitForSeconds(hintDuration);

            hintText.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.3f);
        }
    }

    GameObject GetPrefab(PathogenType type)
    {
        return type switch
        {
            PathogenType.Virus    => virusPrefab,
            PathogenType.Bacteria => bacteriaPrefab,
            PathogenType.Parasite => parasitePrefab,
            _                     => null
        };
    }
}