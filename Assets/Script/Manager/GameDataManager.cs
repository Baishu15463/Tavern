using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public int money = 0;
    public Dictionary<string, int> inventory = new Dictionary<string, int>(); //食材ID和数量

    public List<IngredientData> startIngredients; //初始食材列表
    public List<int> startAmounts; //初始食材数量列表

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
        }
        InitializeInventory();
    }

    void InitializeInventory()
    {
        if (inventory.Count == 0 && startIngredients != null)
        {
            for (int i = 0; i < startIngredients.Count; i++)
            {
                IngredientData ing = startIngredients[i];
                int amount = (i < startAmounts.Count) ? startAmounts[i] : 0;
                if (amount > 0) 
                {
                    inventory[ing.ingredientName] = amount;
                }
            }
        }
    }
    // 检查玩家是否拥有制作指定食谱所需的所有食材
    public bool HasIngredients(RecipeData recipe)
    {
        foreach (IngredientData required in recipe.ingredients)
        {
            string id = required.ingredientName;
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
        foreach (IngredientData required in recipe.ingredients)
        {
            string id = required.ingredientName;
            if (inventory.ContainsKey(id))
            {
                inventory[id]--;
                if (inventory[id] < 0)
                {
                    inventory[id] = 0; // 确保数量不会为负数
                }
            }
        }
    }

    //添加食材
    public void AddIngredient(IngredientData ingredient, int amount)
    {
        string id = ingredient.ingredientName;
        if (inventory.ContainsKey(id))
        {
            inventory[id] += amount;
        }
        else
        {
            inventory[id] = amount;
        }
    }

    // 获取指定食材的数量
    public int GetIngredientCount(IngredientData ingredient)
    {
        string id = ingredient.ingredientName;
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
        if(money >= amount)
        {
            money-= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}


