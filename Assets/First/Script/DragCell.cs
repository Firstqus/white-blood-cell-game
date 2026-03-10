using UnityEngine;

// แขวนสคริปต์นี้ไว้กับ Icon ของแต่ละเม็ดเลือดขาว (Macrophage, Eosinophil, CD8)
// ตั้งค่า immunePrefab ให้ตรงกับแต่ละตัว

public class DragCell : MonoBehaviour
{
    [Header("Lane Spawns")]
    public Transform lane1Spawn;
    public Transform lane2Spawn;
    public Transform lane3Spawn;

    [Header("Drag Limit (Y)")]
    public float minY = -4.5f;
    public float maxY = -3.17f;

    [Header("Prefab ของเม็ดเลือดขาวตัวนี้")]
    public GameObject immunePrefab;

    Vector3 startPos;
    bool isDragging = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // เช็คว่า click ตรงตัวนี้ไหม
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
            if (hit != null && hit.gameObject == gameObject)
                isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);
            transform.position = mousePos;
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            SpawnCell();
            transform.position = startPos;
            isDragging = false;
        }
    }

    void SpawnCell()
    {
        float x = transform.position.x;

        Transform targetLane = lane1Spawn;

        if (Mathf.Abs(x - lane2Spawn.position.x) < Mathf.Abs(x - targetLane.position.x))
            targetLane = lane2Spawn;

        if (Mathf.Abs(x - lane3Spawn.position.x) < Mathf.Abs(x - targetLane.position.x))
            targetLane = lane3Spawn;

        Instantiate(immunePrefab, targetLane.position, Quaternion.identity);
    }
}