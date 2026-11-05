using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // ★ 必须加这个命名空间

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData slotItem;
    public Image slotImage;
    public Text slotIndex;

    // 鼠标移入时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotItem != null && TooltipManager.Instance != null)
        {
            // 拼接你想显示的文本内容（名称 + 价格 + 描述）
            string content = $"<b>{slotItem.itemName}</b>\n价格: {slotItem.price}\n\n{slotItem.itemInfo}";
            TooltipManager.Instance.ShowTooltip(content);
        }
    }

    // 鼠标移出时触发
    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}