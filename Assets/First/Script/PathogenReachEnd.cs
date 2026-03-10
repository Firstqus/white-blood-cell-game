using UnityEngine;

// แขวนสคริปต์นี้ไว้กับ GameObject ที่เป็นเส้น "ด้านล่างสุด" (trigger zone)
// เมื่อศัตรูผ่านแนวนี้ → HP ลด

public class PathogenReachEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Pathogen pathogen = other.GetComponent<Pathogen>();

        if (pathogen != null)
        {
            GameManager.Instance?.TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}