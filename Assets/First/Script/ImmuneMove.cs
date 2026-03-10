using UnityEngine;

public class ImmuneMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {
            Destroy(collision.gameObject); // ลบไวรัส
            Destroy(gameObject);           // ลบเม็ดเลือดขาว
        }
    }
}