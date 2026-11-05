using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int money = 0;
    // 唯一真实库存：物品名字 -> 数量
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public List<ItemData> startItems;
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
            return;
        }

        InitializeInventory();
    }

    void InitializeInventory()
    {
        if (inventory.Count == 0 && startItems != null && startAmounts != null)
        {
            for (int i = 0; i < startItems.Count; i++)
            {
                ItemData item = startItems[i];
                int amount = (i < startAmounts.Count) ? startAmounts[i] : 0;

                if (amount > 0)
                {
                    inventory[item.itemName] = amount;
                }
            }
        }
    }

    // 检查是否有足够食材做菜
    public bool HasIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return false;

        foreach (ItemData required in recipe.item)
        {
            string id = required.itemName;
            if (!inventory.ContainsKey(id) || inventory[id] <= 0)
            {
                return false;
            }
        }
        return true;
    }

    // 扣除做菜消耗的食材
    public void ConsumeIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return;

        foreach (ItemData required in recipe.item)
        {
            string id = required.itemName;
            if (inventory.ContainsKey(id))
            {
                inventory[id]--;
                if (inventory[id] < 0) inventory[id] = 0;
            }
        }
    }

    // 增加物品（捡起、购买、做菜产出）
    public void AddItem(ItemData itemData, int amount)
    {
        if (itemData == null) return;

        string id = itemData.itemName;
        if (inventory.ContainsKey(id))
        {
            inventory[id] += amount;
        }
        else
        {
            inventory[id] = amount;
        }
    }

    // 获取指定物品的当前数量
    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        string id = item.itemName;
        return inventory.ContainsKey(id) ? inventory[id] : 0;
    }

    public void AddMoney(int amount) { money += amount; }

    public bool RemoveMoney(int amount)
    {
        if (money >= amount) { money -= amount; return true; }
        return false;
    }
}