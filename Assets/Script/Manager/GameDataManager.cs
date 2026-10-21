using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int money = 0;
    // 物品ID（名字）和数量
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    // 初始物品列表 (改成 ItemData)
    public List<ItemData> startItems;
    // 初始物品数量列表
    public List<int> startAmounts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // 如果已经有实例了，销毁自己并直接返回，不再初始化
        }

        InitializeInventory();
    }

    void InitializeInventory()
    {
        // 增加 startAmounts != null 的判断，防止空引用报错
        if (inventory.Count == 0 && startItems != null && startAmounts != null)
        {
            for (int i = 0; i < startItems.Count; i++)
            {
                ItemData item = startItems[i];
                // 防止 startAmounts 数量比 startItems 少导致越界
                int amount = (i < startAmounts.Count) ? startAmounts[i] : 0;

                if (amount > 0)
                {
                    inventory[item.itemName] = amount;
                }
            }
        }
    }

    // 检查玩家是否拥有制作指定食谱所需的所有食材
    public bool HasIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return false;

        foreach (ItemData required in recipe.item) // 改成 ItemData
        {
            string id = required.itemName; // 改成 itemName
            if (!inventory.ContainsKey(id) || inventory[id] <= 0)
            {
                return false;
            }
        }
        return true;
    }

    // 扣除制作指定食谱所需的所有食材
    public void ConsumeIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return;

        foreach (ItemData required in recipe.item) // 改成 ItemData
        {
            string id = required.itemName; // 改成 itemName
            if (inventory.ContainsKey(id))
            {
                inventory[id]--;
                if (inventory[id] < 0)
                {
                    inventory[id] = 0;
                }
            }
        }
    }

    // 添加物品
    public void AddItem(ItemData itemData, int amount) // 方法名也建议改成 AddItem
    {
        if (itemData == null) return;

        string id = itemData.itemName; // 改成 itemName
        if (inventory.ContainsKey(id))
        {
            inventory[id] += amount;
        }
        else
        {
            inventory[id] = amount;
        }
    }

    // 获取指定物品的数量
    public int GetItemCount(ItemData item) // 方法名也建议改成 GetItemCount
    {
        if (item == null) return 0;

        string id = item.itemName; // 改成 itemName
        return inventory.ContainsKey(id) ? inventory[id] : 0;
    }

    //加钱
    public void AddMoney(int amount)
    {
        money += amount;
    }

    //扣钱
    public bool RemoveMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}