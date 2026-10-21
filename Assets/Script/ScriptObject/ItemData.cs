using UnityEngine;

[CreateAssetMenu(fileName = "Newitem", menuName = "Tavern/New item")]
public class ItemData : ScriptableObject
{
    public string itemName;   // 物品名称
    public int buyPrice;            // 购买价格
    public Sprite image;             // 物品图标
    public int itemHeld; // 物品持有数量
    [TextArea]
    public string itemInfo; // 物品描述
}