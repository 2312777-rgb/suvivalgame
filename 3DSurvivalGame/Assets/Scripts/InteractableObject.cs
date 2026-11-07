using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool PlayerInRange;
    public string ItemName;

    private void Update()
    {
        if (PlayerInRange && Input.GetMouseButtonDown(0) && SelectionManager.instance.selectedObject == gameObject)
        {
            if (!InventorySystem.Instance.CheckifFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full");
            }
        }
    }

    public string GetItemName()
    {
        return ItemName;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}