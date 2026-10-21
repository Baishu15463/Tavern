using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemData slotItem;
    public Image slotImage;
    public Text slotIndex;

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInformation(slotItem.itemInfo);
    }
}
