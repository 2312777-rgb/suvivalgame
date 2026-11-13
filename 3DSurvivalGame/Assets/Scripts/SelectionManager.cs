using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }
    public bool onTarget;
    public GameObject selectedObject;

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        onTarget = false;
        if (interaction_Info_UI != null)
        {
            interaction_text = interaction_Info_UI.GetComponent<Text>();
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            // Kiểm tra: object PHẢI CÓ script InteractableObject VÀ người chơi phải ở trong tầm
            if (interactable != null && interactable.PlayerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                // Check tag được đưa vào bên trong để đảm bảo interactable không bị null
                if (interactable.CompareTag("pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                }
            }
            else
            {
                // Nhìn trúng vật khác (hoặc chưa đủ gần) -> Reset UI
                ResetUI();
            }
        }
        else
        {
            // Không nhìn trúng vật nào cả -> Reset UI
            ResetUI();
        }
    }

    // Tách phần tắt UI ra một hàm riêng để code gọn gàng, tránh lặp lại
    private void ResetUI()
    {
        onTarget = false;
        selectedObject = null;
        interaction_Info_UI.SetActive(false);
        handIcon.gameObject.SetActive(false);
        centerDotImage.gameObject.SetActive(true);
    }
    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }
    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}