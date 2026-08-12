using UnityEngine;
using UnityEngine.UI;
using TMPro;                          // ← 关键：引入 TextMeshPro 命名空间
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    // 引用
    public KitchenSystem kitchen;
    public TMP_Text resultText;        // ← 改成 TMP_Text
    public GameObject ingredientButtonPrefab;
    public Transform buttonContainer;

    // 当前玩家已选的食材
    private List<IngredientData> selectedIngredients = new List<IngredientData>();

    // 所有可用的食材
    public List<IngredientData> allIngredients;

    void Start()
    {
        // 如果 allIngredients 为空，报个错提醒自己
        if (allIngredients == null || allIngredients.Count == 0)
        {
            Debug.LogError("请先在 Inspector 里给 All Ingredients 列表添加食材！");
            return;
        }

        // 如果按钮预制体没拖，报错提醒
        if (ingredientButtonPrefab == null)
        {
            Debug.LogError("请先拖入 Ingredient Button Prefab！");
            return;
        }

        // 如果按钮容器没拖，报错提醒
        if (buttonContainer == null)
        {
            Debug.LogError("请先拖入 Button Container！");
            return;
        }

        // 为每个食材动态生成按钮
        foreach (IngredientData ingredient in allIngredients)
        {
            GameObject btnObj = Instantiate(ingredientButtonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();

            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            if (btnText != null)
            {
                btnText.text = ingredient.ingredientName;
            }
            else
            {
                Debug.LogWarning("按钮预制体里找不到 TMP_Text 组件，请检查预制体");
            }

            // 绑定点击事件
            btn.onClick.AddListener(() => AddIngredient(ingredient));
        }
    }

    // 添加食材到选中列表
    public void AddIngredient(IngredientData ingredient)
    {
        if (!selectedIngredients.Contains(ingredient))
        {
            selectedIngredients.Add(ingredient);
            Debug.Log("已选：" + ingredient.ingredientName);
            UpdateSelectedDisplay();
        }
        else
        {
            Debug.Log(ingredient.ingredientName + " 已经选过了");
        }
    }

    // 清空已选食材
    public void ClearSelection()
    {
        selectedIngredients.Clear();
        Debug.Log("已清空选择");
        UpdateSelectedDisplay();
    }

    // 调用厨房系统匹配菜谱
    public void OnCookButtonClick()
    {
        if (selectedIngredients.Count == 0)
        {
            resultText.text = "请先选择食材！";
            return;
        }

        RecipeData matchedRecipe = kitchen.MatchRecipe(selectedIngredients);

        if (matchedRecipe != null)
        {
            // 新增：检查库存是否足够
            if (GameDataManager.Instance.HasIngredients(matchedRecipe))
            {
                // 消耗食材
                GameDataManager.Instance.ConsumeIngredients(matchedRecipe);
                // 加钱
                GameDataManager.Instance.AddMoney(matchedRecipe.basePrice);
                // 显示结果
                resultText.text = "做出：" + matchedRecipe.recipeName + "！\n赚了 " + matchedRecipe.basePrice + " 金币";

                // 清空选择，准备下一道菜
                ClearSelection();
            }
            else
            {
                resultText.text = "食材不够！需要：";
                foreach (IngredientData ing in matchedRecipe.ingredients)
                {
                    resultText.text += "\n" + ing.ingredientName + " x1";
                }
            }
        }
        else
        {
            resultText.text = "这些食材组合不出任何菜谱...";
        }
    }

    // 显示已选食材
    private void UpdateSelectedDisplay()
    {
        if (selectedIngredients.Count == 0)
        {
            // 可以留空或者显示提示
            return;
        }

        string names = "";
        foreach (IngredientData ing in selectedIngredients)
        {
            names += ing.ingredientName + " ";
        }
        Debug.Log("当前已选：" + names);
    }
}