using UnityEngine;
using TMPro;
using System.Collections;

// แขวนสคริปต์นี้กับ Icon เม็ดเลือดขาว แล้วกำหนด atpCost ใน Inspector
public class DragCell : MonoBehaviour
{
    [Header("Lane Spawns")]
    public Transform lane1Spawn;
    public Transform lane2Spawn;
    public Transform lane3Spawn;

    [Header("Drag Limit (Y)")]
    public float minY = -4.5f;
    public float maxY = -3.17f;

    [Header("Prefab")]
    public GameObject immunePrefab;

    [Header("ATP Cost")]
    public float atpCost = 20f;

    [Header("Feedback UI (optional)")]
    public TextMeshProUGUI notEnoughATPText;
    public float           feedbackDuration = 1f;
    public TextMeshProUGUI costLabel;

    Vector3 startPos;
    bool    isDragging = false;

    void Start()
    {
        startPos = transform.position;
        if (notEnoughATPText != null) notEnoughATPText.gameObject.SetActive(false);
        if (costLabel        != null) costLabel.text = $"{atpCost} ATP";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mw.z = 0;
            Collider2D hit = Physics2D.OverlapPoint(mw);
            if (hit != null && hit.gameObject == gameObject) isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mp.z = 0;
            mp.y = Mathf.Clamp(mp.y, minY, maxY);
            transform.position = mp;
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            TrySpawnCell();
            transform.position = startPos;
            isDragging = false;
        }
    }

    void TrySpawnCell()
    {
        if (ATPManager.Instance != null && !ATPManager.Instance.TrySpend(atpCost))
        {
            ShowNotEnoughATP();
            return;
        }
        SpawnCell();
    }

    void SpawnCell()
    {
        float     x          = transform.position.x;
        Transform targetLane = lane1Spawn;

        if (Mathf.Abs(x - lane2Spawn.position.x) < Mathf.Abs(x - targetLane.position.x)) targetLane = lane2Spawn;
        if (Mathf.Abs(x - lane3Spawn.position.x) < Mathf.Abs(x - targetLane.position.x)) targetLane = lane3Spawn;

        Instantiate(immunePrefab, targetLane.position, Quaternion.identity);
    }

    void ShowNotEnoughATP()
    {
        if (notEnoughATPText != null)
        {
            notEnoughATPText.text = "Not enough ATP!";
            notEnoughATPText.gameObject.SetActive(true);
            Invoke(nameof(HideNotEnoughATP), feedbackDuration);
        }
        StartCoroutine(ShakeRoutine());
    }

    void HideNotEnoughATP()
    {
        if (notEnoughATPText != null) notEnoughATPText.gameObject.SetActive(false);
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            transform.position = startPos + new Vector3(Random.Range(-1f, 1f) * 0.05f, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
}