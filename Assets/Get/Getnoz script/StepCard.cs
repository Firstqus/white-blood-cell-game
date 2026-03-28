using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StepCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int correctOrder;
    public DropSlot currentSlot;
    public Transform cardContainer;

    private Canvas canvas;
    private CanvasGroup cg;
    private RectTransform rt;
    private Vector2 originalSize;

    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        cg     = GetComponent<CanvasGroup>();
        rt     = GetComponent<RectTransform>();
        originalSize = rt.sizeDelta;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // ล้าง slot เดิม
        if (currentSlot != null)
        {
            currentSlot.RemoveCard();
            currentSlot = null;
        }

        // เก็บขนาดและตำแหน่งก่อน
        Vector3 worldPos = transform.position;

        // ย้ายขึ้นบน Canvas
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        transform.position = worldPos;

        rt.sizeDelta = originalSize;

        cg.blocksRaycasts = false;
        cg.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData e)
    {
        transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        cg.blocksRaycasts = true;
        cg.alpha = 1f;

        // ถ้ายังไม่มี slot รับ → กลับ CardContainer
        if (currentSlot == null)
        {
            ReturnToCardContainer();
        }
    }

    public void ReturnToCardContainer()
    {
        currentSlot = null;
        transform.SetParent(cardContainer, false);
        rt.sizeDelta = originalSize;
        rt.localPosition = Vector3.zero;
    }
}