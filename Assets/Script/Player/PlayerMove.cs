using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;          // 移动速度
    [SerializeField] private bool useRigidbody = true;      // 是否使用刚体移动
    [SerializeField] private bool normalizeDiagonal = true; // 是否归一化对角线速度

    [Header("动画（可选）")]
    [SerializeField] private Animator animator;             // 角色动画器
    [SerializeField] private string speedParam = "Speed";   // 动画器中的速度参数名

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    public GameObject myBag;
    bool isBagOpen = false;
    // Start is called before the first frame update

    private void Awake()
    {
        // 获取或添加Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
        if (rb == null && useRigidbody)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            // 俯视角通常不需要重力
            rb.gravityScale = 0f;
            rb.freezeRotation = true; // 防止物理旋转
        }

        // 如果没有手动赋值Animator，尝试自动获取
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenBag();

    }

    void FixedUpdate()
    {
        Move();
    }

    void OpenBag()//打开背包
    {
        if (myBag != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                isBagOpen = !isBagOpen;
                myBag.SetActive(isBagOpen);
            }

        }
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical);

        // 2. 对角线归一化处理
        if (normalizeDiagonal && moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }

        // 3. 更新动画参数（如果有动画器）
        if (animator != null)
        {
            // 将输入速度传递给动画器（通常用于混合树）
            animator.SetFloat(speedParam, moveInput.magnitude);

            // 如果需要根据方向播放不同动画，可以传递方向参数
            // animator.SetFloat("Horizontal", moveInput.x);
            // animator.SetFloat("Vertical", moveInput.y);
        }

        // 4. 记录最后移动方向（用于攻击朝向等）
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
        if (useRigidbody && rb != null)
        {
            // 使用刚体移动（推荐，物理交互更平滑）
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // 直接修改Transform（简单但可能与物理冲突）
            transform.Translate(moveInput * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    // 公共方法：获取最后移动方向（供其他脚本使用）
    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }
}

