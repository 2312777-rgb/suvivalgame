using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (canvas == null)
        {
            canvas = transform.root.GetComponentInParent<Canvas>();
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        }

        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float scale = (canvas != null) ? canvas.scaleFactor : 1f;
        rectTransform.anchoredPosition += eventData.delta / scale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        Debug.Log("OnEndDrag");
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}