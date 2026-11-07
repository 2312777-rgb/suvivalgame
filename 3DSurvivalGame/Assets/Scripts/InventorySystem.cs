using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public List<GameObject> slotlist = new List<GameObject>();
    public List<string> itemlist = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;
   // public bool isFull;

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


    void Start()
    {
        isOpen = false;
        PopulateSlotList();

    }
    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotlist.Add(child.gameObject);
            }    
        }
    }    


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
         
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }
    public void AddToInventory(String itemName)
    {
            whatSlotToEquip = FindNextEmtySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName),whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemlist.Add(itemName);

    }    
    public bool CheckifFull()
    {
        int couter = 0;
        foreach(GameObject slot in slotlist)
        {
            if (slot.transform.childCount > 0)
            {
                couter += 1;
            } 
        }
        if (couter == slotlist.Count)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private GameObject FindNextEmtySlot()
    {
        foreach(GameObject slot in slotlist)
        {
            if (slot.transform.childCount == 0 )
            {
                return slot;
            }    
        }
        return new GameObject();
    }
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int count = 0;

        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);
                string childName = child.name.ToLower();

                // Kiểm tra xem tên Item trong slot có chứa từ khóa cần xóa không
                if (childName.Contains(nameToRemove.ToLower()))
                {
                    // 1. Tách child khỏi slot ngay lập tức để childCount giảm về 0
                    child.SetParent(null);

                    // 2. Xóa GameObject hình ảnh trên UI
                    Destroy(child.gameObject);

                    count++;

                    // Khi đã xóa đủ số lượng yêu cầu thì dừng
                    if (count >= amountToRemove)
                    {
                        break;
                    }
                }
            }
        }

        // 3. Cập nhật lại dữ liệu danh sách itemlist sau khi đã xóa UI
        ReCaculateList();
    }
    public void ReCaculateList()
    {
        itemlist.Clear();
        foreach (GameObject slot in slotlist)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str1 = name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemlist.Add(result);
            }
        }
    }
}
