using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StepCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int correctOrder;
    [HideInInspector] public DropSlot currentSlot;
    [HideInInspector] public Transform cardContainer;

    [Header("Sound")]
    public AudioClip dragSound;
    private AudioSource audioSource;

    private Canvas canvas;
    private CanvasGroup cg;
    private RectTransform rt;
    private Vector2 originalSize;

    void Awake()
    {
        canvas       = GetComponentInParent<Canvas>();
        cg           = GetComponent<CanvasGroup>();
        rt           = GetComponent<RectTransform>();
        originalSize = rt.sizeDelta;

        audioSource  = gameObject.AddComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (dragSound != null)
            audioSource.PlayOneShot(dragSound); // เล่นเสียงตอนเริ่มลาก

        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        Vector3 worldPos = transform.position;
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        transform.position = worldPos;
        rt.sizeDelta       = originalSize;

        cg.blocksRaycasts = false;
        cg.alpha          = 0.8f;
    }

    public void OnDrag(PointerEventData e)
    {
        transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        cg.blocksRaycasts = true;
        cg.alpha          = 1f;

        if (currentSlot == null)
            GoBackToContainer();
    }

    public void GoBackToContainer()
    {
        currentSlot = null;
        transform.SetParent(cardContainer, false);
        rt.anchorMin        = new Vector2(0.5f, 0.5f);
        rt.anchorMax        = new Vector2(0.5f, 0.5f);
        rt.pivot            = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta        = originalSize;
    }
}