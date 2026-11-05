using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemData slotItem;
    public Image slotImage;
    public Text slotIndex; // 如果你用的是TMP，这里改成 TMP_Text，并在上面脚本同步修改

    public void ItemOnClicked()
    {
        if (slotItem != null)
        {
            InventoryManager.UpdateItemInformation(slotItem.itemInfo);
        }
    }
}