using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public int money = 0;

    //字典的 Key(物品ID）
    public Dictionary<int, int> inventory = new Dictionary<int, int>();

    [Header("事件通道")]
    public OnItemChangeSO onItemGained;
    public OnInventoryChangeSO onInventoryChanged;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (onItemGained != null) onItemGained.OnEventRaised += HandleItemGained;
    }

    private void OnDisable()
    {
        if (onItemGained != null) onItemGained.OnEventRaised -= HandleItemGained;
    }

    private void HandleItemGained(ItemData item)
    {
        AddItem(item, 1);
        if (onInventoryChanged != null) onInventoryChanged.RaiseEvent();
    }

    public bool HasIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return false;
        foreach (ItemData required in recipe.item)
        {
            // itemID 查
            if (!inventory.ContainsKey(required.itemID) || inventory[required.itemID] <= 0) return false;
        }
        return true;
    }

    public void ConsumeIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.item == null) return;
        foreach (ItemData required in recipe.item)
        {
            // itemID 扣
            if (inventory.ContainsKey(required.itemID))
            {
                inventory[required.itemID]--;
                if (inventory[required.itemID] < 0) inventory[required.itemID] = 0;
            }
        }
        if (onInventoryChanged != null) onInventoryChanged.RaiseEvent();
    }

    public void AddItem(ItemData itemData, int amount)
    {
        if (itemData == null) return;
        // itemID 加
        if (inventory.ContainsKey(itemData.itemID)) inventory[itemData.itemID] += amount;
        else inventory[itemData.itemID] = amount;
    }

    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        //  itemID 查
        return inventory.ContainsKey(item.itemID) ? inventory[item.itemID] : 0;
    }

    public void AddMoney(int amount) { money += amount; }
    public bool RemoveMoney(int amount) { if (money >= amount) { money -= amount; return true; } return false; }
}