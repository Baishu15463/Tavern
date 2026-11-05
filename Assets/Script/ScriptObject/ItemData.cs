using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Tavern/Item")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    [TextArea] public string itemInfo;
    public Sprite image;
    public int price;
}