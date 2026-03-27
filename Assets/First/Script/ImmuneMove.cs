using UnityEngine;

public class ImmuneMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}