using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;
    public List<ItemData> allItemsDatabase; // 你的所有物品数据列表

    [Header("事件通道")]
    public OnInventoryChangeSO onInventoryChanged;

    private void Awake() { Instance = this; }

    private void OnEnable()
    {
        if (onInventoryChanged != null) onInventoryChanged.OnEventRaised += RefreshItem;
        RefreshItem();
    }

    private void OnDisable()
    {
        if (onInventoryChanged != null) onInventoryChanged.OnEventRaised -= RefreshItem;
    }

    public void RefreshItem()
    {
        if (slotGrid == null) return;
        foreach (Transform child in slotGrid.transform) Destroy(child.gameObject);

        // 遍历字典
        foreach (var kvp in GameDataManager.Instance.inventory)
        {
            int id = kvp.Key;       //  ID
            int count = kvp.Value;

            if (count <= 0) continue;

            //  根据 ID 找对应的 ItemData
            ItemData data = allItemsDatabase.Find(x => x.itemID == id);

            if (data != null)
            {
                Slot newItem = Instantiate(slotPrefab, slotGrid.transform);
                newItem.slotItem = data;
                newItem.slotImage.sprite = data.image;
                newItem.slotIndex.text = count.ToString();
            }
        }
    }


}