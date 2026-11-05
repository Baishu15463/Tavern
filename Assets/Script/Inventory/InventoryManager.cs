using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject slotGrid;
    public Slot slotPrefab;
    public TMP_Text itemInformation;

    // 用于在列表中查找对应的 ItemData SO模板
    public List<ItemData> allItemsDatabase;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        if (itemInformation != null) itemInformation.text = "";
    }

    public static void UpdateItemInformation(string itemInfo)
    {
        if (Instance != null && Instance.itemInformation != null)
            Instance.itemInformation.text = itemInfo;
    }

    public static void RefreshItem()
    {
        if (Instance == null || Instance.slotGrid == null) return;

        // 1. 清空旧格子
        foreach (Transform child in Instance.slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. 遍历字典（真实库存）
        foreach (var kvp in GameDataManager.Instance.inventory)
        {
            string itemName = kvp.Key;
            int count = kvp.Value;

            if (count <= 0) continue;

            // 3. 去数据库找图标
            ItemData data = Instance.allItemsDatabase.Find(x => x.itemName == itemName);
            if (data != null)
            {
                CreateNewItem(data, count);
            }
        }
    }

    public static void CreateNewItem(ItemData item, int count)
    {
        Slot newItem = Instantiate(Instance.slotPrefab, Instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.image;
        newItem.slotIndex.text = count.ToString(); // 显示数量
    }
}