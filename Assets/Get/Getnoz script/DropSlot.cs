using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    [HideInInspector] public StepCard currentCard;

    private Image img;
    private TextMeshProUGUI label;

    Color emptyColor  = new Color(0f, 0f, 0f, 0.3f);
    Color filledColor = new Color(0.08f, 0.28f, 0.20f, 0.8f);

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

    public void OnDrop(PointerEventData e)
    {
        StepCard card = e.pointerDrag?.GetComponent<StepCard>();
        if (card == null) return;

        if (currentCard != null)
            currentCard.GoBackToContainer();

        if (card.currentSlot != null)
            card.currentSlot.ClearSlot();

        PlaceCard(card);
    }

    void PlaceCard(StepCard card)
    {
        currentCard      = card;
        card.currentSlot = this;

        card.transform.SetParent(transform, false);

        // ยืด Card เต็ม Slot
        RectTransform cardRT = card.GetComponent<RectTransform>();
        cardRT.anchorMin        = Vector2.zero;
        cardRT.anchorMax        = Vector2.one;
        cardRT.offsetMin        = new Vector2(4, 4);
        cardRT.offsetMax        = new Vector2(-4, -4);

        // ยืด Text เต็ม Slot ด้วย
        var tmp = card.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            RectTransform tmpRT = tmp.GetComponent<RectTransform>();
            tmpRT.anchorMin = Vector2.zero;
            tmpRT.anchorMax = Vector2.one;
            tmpRT.offsetMin = new Vector2(10, 5);
            tmpRT.offsetMax = new Vector2(-10, -5);

            tmp.enableAutoSizing = true;
            tmp.fontSizeMin      = 16;
            tmp.fontSizeMax      = 36;
            tmp.alignment        = TextAlignmentOptions.Center;
        }

        img.color = filledColor;
        if (label != null) label.text = "";
    }

    public void ClearSlot()
    {
        currentCard = null;
        img.color   = emptyColor;
        if (label != null) label.text = slotIndex.ToString();
    }
}