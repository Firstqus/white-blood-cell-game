using UnityEngine;

public class ImmuneMove : MonoBehaviour
{
    public float speed = 2f;
    public PathogenType targetType;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        Pathogen pathogen = collision.gameObject.GetComponent<Pathogen>();

        if (pathogen == null)
        {
            Debug.Log("No Pathogen script on: " + collision.gameObject.name);
            return;
        }

        Debug.Log("Pathogen type: " + pathogen.type + " | Target: " + targetType);

        if (pathogen.type == targetType)
        {
            GameManager.Instance?.AddScore(10);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // เผื่อ Trigger ทำงานแทน
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);

        Pathogen pathogen = other.GetComponent<Pathogen>();
        if (pathogen == null) return;

        if (pathogen.type == targetType)
        {
            GameManager.Instance?.AddScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}