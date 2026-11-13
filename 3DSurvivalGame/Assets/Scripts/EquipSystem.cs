using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numberHolder;
    public int selectedNumber = -1;
    public GameObject selectedItem; 


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectQuickSlot(6);
        }
    }
    void selectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isInsideQuickSlots = false;
                }
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isInsideQuickSlots = true;
                foreach (Transform child in numberHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
                Text toBeChange = numberHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChange.color = Color.white;
            }    
            else
            {
                selectedNumber = -1;
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isInsideQuickSlots = false;
                    selectedItem = null;
                }
                foreach (Transform child in numberHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
        }
    }
    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }
    bool checkIfSlotIsFull(int slotNumber)
    {
        if  (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);


        InventorySystem.Instance.ReCaculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}