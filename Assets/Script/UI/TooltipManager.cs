using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPrefab; 
    private GameObject currentTooltip;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTooltip(string content)
    {
        if (currentTooltip == null)
        {
            // 生成悬浮窗，并作为 Canvas 的子物体
            currentTooltip = Instantiate(tooltipPrefab, transform);
        }

        // 设置里面的文字
        TMP_Text text = currentTooltip.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = content;

        currentTooltip.SetActive(true);
        UpdatePosition(); // 更新位置
    }

    public void HideTooltip()
    {
        if (currentTooltip != null)
        {
            currentTooltip.SetActive(false);
        }
    }

    private void Update()
    {
        // 让悬浮窗一直跟着鼠标
        if (currentTooltip != null && currentTooltip.activeSelf)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        // 把鼠标屏幕坐标转换成 UI 坐标
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            Input.mousePosition,
            null,
            out mousePos);

        // 加上一点偏移，防止鼠标挡住文字
        currentTooltip.GetComponent<RectTransform>().localPosition = mousePos + new Vector2(100  , -100);
    }
}