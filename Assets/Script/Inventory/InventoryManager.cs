using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory myBag; 
    public GameObject slotGrid;
    public Slot sloPrefab;
    public Text itemInfromation;


    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfromation.text = "";
    }

    public static void UpdateItemInformation(string itemInfo)
    {
        instance.itemInfromation.text = itemInfo;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static void  CreateNewItem(ItemData item)//用于创建新的物品栏
    {
        Slot newItem = Instantiate(instance.sloPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.image;
        newItem.slotIndex.text = item.itemHeld.ToString(); 
    }

    public static void RefreshItem()//用于检测获取重复物品时刷新数量
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            CreateNewItem(instance.myBag.itemList[i]);
        }
    }
}
