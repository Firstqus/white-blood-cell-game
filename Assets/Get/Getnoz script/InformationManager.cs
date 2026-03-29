using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class InformationManager : MonoBehaviour
{
    public Transform      slotContainer;
    public Transform      cardContainer;
    public GameObject     panelResult;
    public TextMeshProUGUI textResult;
    public GameObject     btnNext;

    [Header("Sound")]
    public AudioClip clickSound;
    public AudioClip typingSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        panelResult.SetActive(false);
        btnNext.SetActive(false);

        foreach (Transform child in cardContainer)
        {
            StepCard sc = child.GetComponent<StepCard>();
            if (sc != null) sc.cardContainer = cardContainer;
        }

        ShuffleCards();
    }

    void PlayClick()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    void ShuffleCards()
    {
        int count = cardContainer.childCount;
        for (int i = count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            cardContainer.GetChild(i).SetSiblingIndex(rand);
        }
    }

    public void OnCheckPressed()
    {
        // เช็คว่าใส่การ์ดครบทุกช่องไหม
        PlayClick();
        foreach (Transform slotT in slotContainer)
        {
            DropSlot slot = slotT.GetComponent<DropSlot>();
            if (slot == null) continue;
            if (slot.currentCard == null)
            {
                textResult.text  = "ใส่การ์ดให้ครบทุกช่องก่อนนะครับ";
                textResult.color = new Color(0.94f, 0.60f, 0.60f);
                panelResult.SetActive(true);
                btnNext.SetActive(false);
                return;
            }
        }

        if (IsCorrectOrder()) ShowSuccess();
        else                  ShowFail();
    }

    bool IsCorrectOrder()
    {
        foreach (Transform slotT in slotContainer)
        {
            DropSlot slot = slotT.GetComponent<DropSlot>();
            if (slot == null) continue;
            if (slot.currentCard.correctOrder != slot.slotIndex)
                return false;
        }
        return true;
    }

    void ShowSuccess()
    {
        panelResult.SetActive(true);
        btnNext.SetActive(true);
        textResult.color = new Color(0.62f, 0.88f, 0.79f);
        StartCoroutine(TypeText(
            "เมื่อแบคทีเรียเข้าสู่ร่างกาย Neutrophil จะเป็นด่านแรก\n" +
            "ตามด้วย Macrophage จดจำ Antigen และส่งสัญญาณ\n\n" +
            "T-Helper Cell → B-Cell → Antibody จำเพาะ\n\n" +
            "Memory Cell ถูกเก็บไว้ — หากเชื้อกลับมา\n" +
            "ร่างกายพร้อมสู้ได้เร็วกว่าเดิมทันที\n\n" +
            "นี่คือหลักการของ Immunological Memory"
        ));
    }

    void ShowFail()
    {
        panelResult.SetActive(true);
        btnNext.SetActive(false);
        textResult.color = new Color(0.94f, 0.60f, 0.60f);
        StartCoroutine(TypeText("ลำดับยังไม่ถูกต้อง\nลองเรียงใหม่อีกครั้งนะครับ"));
    }

    IEnumerator TypeText(string content)
    {
        textResult.text = "";
        foreach (char c in content)
        {
            textResult.text += c;

            if (typingSound != null && c != ' ' && c != '\n' && !audioSource.isPlaying)
                audioSource.PlayOneShot(typingSound);

            yield return new WaitForSeconds(0.05f);
        }

        audioSource.Stop(); // หยุดเสียงตอนพิมพ์จบ
    }

    public void OnRetryPressed()
    {
        PlayClick();  // เพิ่มบรรทัดนี้
        panelResult.SetActive(false);
    }

    public void OnNextPressed()
    {
        PlayClick();  // เพิ่มบรรทัดนี้
        SceneManager.LoadScene("GameEnd");
    }
}