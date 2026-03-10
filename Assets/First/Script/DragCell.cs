using UnityEngine;

public class DragCell : MonoBehaviour
{
    public Transform lane1Spawn;
    public Transform lane2Spawn;
    public Transform lane3Spawn;

    public float minY = -4.5f;
    public float maxY = -3.17f;

    public GameObject immunePrefab;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);

            transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            SpawnCell();

            transform.position = startPos;
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