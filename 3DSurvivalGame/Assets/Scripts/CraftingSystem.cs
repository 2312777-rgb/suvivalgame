using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreemUI;
    public List<string> InventoryItemList = new List<string>();

    Button toolsBTN;
    Button craftAxeBTN;
    Text AxeReq1, AxeReq2;
    public bool isOpen;

    // Đã hợp lệ vì Blueprint.cs không còn là MonoBehaviour
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Birch", 1);

    public static CraftingSystem instance { get; private set; }

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

    void Start()
    {
        isOpen = false;
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(OpenToolsCategory);

        AxeReq1 = toolsScreemUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreemUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();
        craftAxeBTN = toolsScreemUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(() => CrafAniItem(AxeBLP));
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreemUI.SetActive(true);
        RefreshNeededItems(); // Cập nhật danh sách vật phẩm khi vừa mở bảng Tools
    }

    void Update()
    {
        // Xử lý bật/tắt bảng Crafting bằng phím C
        if (Input.GetKeyDown(KeyCode.C))
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                craftingScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                craftingScreenUI.SetActive(false);
                toolsScreemUI.SetActive(false);

                if (InventorySystem.Instance != null && !InventorySystem.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

    void CrafAniItem(Blueprint blueprintToCraf)
    {
        // 1. Thêm vật phẩm đã chế tạo vào Inventory
        InventorySystem.Instance.AddToInventory(blueprintToCraf.itemName);

        // 2. Trừ bớt nguyên liệu tương ứng
        if (blueprintToCraf.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraf.Req1, blueprintToCraf.Req1amount);
        }
        else if (blueprintToCraf.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraf.Req1, blueprintToCraf.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraf.Req2, blueprintToCraf.Req2amount);
        }

        // 3. Tính toán lại Inventory và làm mới UI
        InventorySystem.Instance.ReCaculateList();
        RefreshNeededItems();
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int birch_count = 0;

        InventoryItemList = InventorySystem.Instance.itemlist;

        foreach (string itemName in InventoryItemList)
        {
            string nameClean = itemName.ToLower().Trim();

            if (nameClean.Contains("stone"))
            {
                stone_count++;
            }
            else if (nameClean.Contains("birch"))
            {
                birch_count++;
            }
        }

        AxeReq1.text = "3 Stone[" + stone_count + "]";
        AxeReq2.text = "1 Birch[" + birch_count + "]";

        // Bật/Tắt nút Chế Tạo dựa trên số lượng nguyên liệu
        if (stone_count >= 3 && birch_count >= 1)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }
}