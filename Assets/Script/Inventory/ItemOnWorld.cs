using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public ItemData thisItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if (thisItem == null) return;

        // 1. 数据进入全局字典
        GameDataManager.Instance.AddItem(thisItem, 1);

        // 2. 刷新背包 UI
        if (InventoryManager.Instance != null)
        {
            InventoryManager.RefreshItem();
        }

        Debug.Log("捡到了：" + thisItem.itemName + "，目前拥有：" + GameDataManager.Instance.GetItemCount(thisItem));
    }
}