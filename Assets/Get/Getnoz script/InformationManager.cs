using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InformationManager : MonoBehaviour
{
    public Transform   slotContainer;
    public Transform   cardContainer;
    public GameObject  panelResult;
    public TextMeshProUGUI textResult;
    public GameObject  btnNext;

    void Start()
    {
        panelResult.SetActive(false);
        btnNext.SetActive(false);

    // บอกทุก Card ว่า cardContainer คืออะไร
        foreach (Transform child in cardContainer)
        {
            StepCard sc = child.GetComponent<StepCard>();
            if (sc != null) sc.cardContainer = cardContainer;
        }

        ShuffleCards();
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
        // เช็คว่าทุก slot มีการ์ดครบไหม
        foreach (Transform slotT in slotContainer)
        {
            DropSlot slot = slotT.GetComponent<DropSlot>();
            if (slot.currentCard == null)
            {
                ShowMessage("ใส่การ์ดให้ครบทุกช่องก่อนนะครับ", false);
                return;
            }
        }

        if (IsCorrectOrder())
            ShowSuccess();
        else
            ShowFail();
    }

    bool IsCorrectOrder()
    {
        foreach (Transform slotT in slotContainer)
        {
            DropSlot slot = slotT.GetComponent<DropSlot>();
            if (slot.currentCard.correctOrder != slot.slotIndex)
                return false;
        }
        return true;
    }

    void ShowSuccess()
    {
        panelResult.SetActive(true);
        btnNext.SetActive(true);
        textResult.text =
            "เมื่อแบคทีเรียเข้าสู่ร่างกาย Neutrophil จะเป็นด่านแรก" +
            "ที่จับกินเชื้อทันที ตามด้วย Macrophage ที่จดจำ Antigen\n\n" +
            "T-Helper Cell → B-Cell → ผลิต Antibody จำเพาะ\n\n" +
            "Memory Cell ถูกเก็บไว้ — หากเชื้อกลับมา พร้อมสู้ทันที\n\n" +
            "นี่คือหลักการของ Immunological Memory และวัคซีน";
        textResult.color = new Color(0.62f, 0.88f, 0.79f);
    }

    void ShowFail()
    {
        ShowMessage("ลำดับยังไม่ถูกต้อง\nลองเรียงใหม่อีกครั้งนะครับ", false);
    }

    void ShowMessage(string msg, bool showNext)
    {
        panelResult.SetActive(true);
        btnNext.SetActive(showNext);
        textResult.text  = msg;
        textResult.color = new Color(0.94f, 0.60f, 0.60f);
    }

    public void OnRetryPressed() => panelResult.SetActive(false);
    public void OnNextPressed()  => SceneManager.LoadScene("GameEnd");
}