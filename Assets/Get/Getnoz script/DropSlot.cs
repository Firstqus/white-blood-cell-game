using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int slotIndex;
    public StepCard currentCard;

    private Image img;
    private TextMeshProUGUI label;

    Color emptyColor  = new Color(0.10f, 0.10f, 0.24f, 1f);
    Color filledColor = new Color(0.08f, 0.28f, 0.20f, 1f);
    Color hoverColor  = new Color(0.15f, 0.25f, 0.45f, 1f);

    void Awake()
    {
        img   = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        img.color = emptyColor;
        if (label != null) label.text = slotIndex.ToString();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (currentCard == null) img.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (currentCard == null) img.color = emptyColor;
    }

    public void OnDrop(PointerEventData e)
    {
        StepCard card = e.pointerDrag?.GetComponent<StepCard>();
        if (card == null) return;

        // ถ้า slot มีการ์ดอยู่แล้ว → ส่งกลับ
        if (currentCard != null)
        {
            currentCard.ReturnToCardContainer();
        }

        // วางการ์ดลง slot
        PlaceCard(card);
    }

    public void PlaceCard(StepCard card)
    {
        currentCard      = card;
        card.currentSlot = this;

        card.transform.SetParent(transform, false);

        RectTransform cardRT = card.GetComponent<RectTransform>();
    
        // รีเซ็ต anchor ให้อยู่กึ่งกลาง
        cardRT.anchorMin        = new Vector2(0.5f, 0.5f);
        cardRT.anchorMax        = new Vector2(0.5f, 0.5f);
        cardRT.pivot            = new Vector2(0.5f, 0.5f);
        cardRT.anchoredPosition = Vector2.zero;  // ← วางตรงกลาง slot
        cardRT.sizeDelta        = new Vector2(
            GetComponent<RectTransform>().rect.width  - 8f,
            GetComponent<RectTransform>().rect.height - 8f
        );

        img.color = filledColor;
        if (label != null) label.text = "";
    }

    public void RemoveCard()
    {
        currentCard  = null;
        img.color    = emptyColor;
        if (label != null) label.text = slotIndex.ToString();
    }
}