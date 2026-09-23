using UnityEngine;

public class MapController : MonoBehaviour
{
    [Tooltip("越小越靠前，通常设为 100 或 1000")]
    public int precision = 100;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // 核心公式：Order in Layer = -Y坐标 * 精度
        // Y 越小（越靠下），-Y 越大，Order 越大，渲染越靠前
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * precision);
    }
}