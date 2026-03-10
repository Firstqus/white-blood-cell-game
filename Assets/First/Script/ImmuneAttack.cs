using UnityEngine;

// แขวนสคริปต์นี้ไว้กับ Prefab ของเม็ดเลือดขาวแต่ละตัว
// ตั้งค่า targetType ใน Inspector:
//   CD8        → Virus
//   Macrophage → Bacteria
//   Eosinophil → Parasite

public class ImmuneAttack : MonoBehaviour
{
    public PathogenType targetType;  // ฆ่าได้แค่ชนิดเดียว

    void OnTriggerEnter2D(Collider2D other)
    {
        Pathogen pathogen = other.GetComponent<Pathogen>();
        if (pathogen == null) return;

        if (pathogen.type == targetType)
        {
            // ถูกชนิด → กำจัด + ได้คะแนน
            GameManager.Instance?.AddScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            // ผิดชนิด → เม็ดเลือดขาวตาย ศัตรูผ่านต่อ
            Destroy(gameObject);
        }
    }
}