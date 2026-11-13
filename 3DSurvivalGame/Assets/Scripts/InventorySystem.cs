using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static CraftingSystem instance { get; private set; }
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public List<GameObject> slotlist = new List<GameObject>();
    public List<string> itemlist = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;
    // public bool isFull;

    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

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
        Cursor.visible = false;

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
            Cursor.visible = true;
            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
         
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
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

            ReCaculateList();
            CraftingSystem.instance.RefreshNeededItems();

            Sprite sprite = itemToAdd.GetComponent<Image>().sprite;
            TriggerPickPopUp(itemName, sprite);


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
    void TriggerPickPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true); 
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
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
