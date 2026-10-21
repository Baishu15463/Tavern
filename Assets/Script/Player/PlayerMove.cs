using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool useRigidbody = true;
    [SerializeField] private bool normalizeDiagonal = true;

    [SerializeField] private Animator animator;


    private Rigidbody2D rb;

    // 当前帧的输入（每帧在 Update 里刷新，FixedUpdate 里使用）
    private Vector2 moveInput;


    // 最后朝向，用于静止时决定播放哪个方向的 Idle
    private Vector2 lastMoveDirection = Vector2.down;
    [Header("UI")]
    public GameObject myBag;
    [SerializeField] private GameObject cookUI;

    [Header("状态")]
    bool isBagOpen = false;
    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null && useRigidbody)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        OpenBag();
        OpenCookUI();
        ReadInput();
    }

    void FixedUpdate()
    {
        Move(); 
    }

    void OpenBag()
    {
        if (myBag != null && Input.GetKeyDown(KeyCode.E))
        {
            isBagOpen = !isBagOpen;
            myBag.SetActive(isBagOpen);
        }
    }//打开背包

    // 读取输入，判定 isMoving，更新动画参数
    void ReadInput()// 在 Update 中调用，检测输入，更新动画参数
    {
        if (isBagOpen)
        {
            moveInput = Vector2.zero;
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(horizontal, vertical);

            if (normalizeDiagonal && moveInput.magnitude > 1f)
                moveInput.Normalize();
        }

        // 每帧重新判定：有输入就是 true，没输入就是 false
        isMoving = moveInput != Vector2.zero;

        // 记录最后朝向（仅在有输入时更新，保持朝向不变）
        if (isMoving)
            lastMoveDirection = moveInput;

        // 更新动画参数
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);

            if (isMoving)
            {
                animator.SetFloat("moveX", moveInput.x);
                animator.SetFloat("moveY", moveInput.y);
            }
            else
            {
                animator.SetFloat("idleX", lastMoveDirection.x);
                animator.SetFloat("idleY", lastMoveDirection.y);
            }
        }
    }

    void Move()// 在 FixedUpdate 中调用，使用 Rigidbody 或 Transform 移动，与Move检测输入分开，避免在 Update 中直接移动
    {
        if (useRigidbody && rb != null)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(moveInput * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }

    private void OpenCookUI()// 打开厨房系统UI
    {
        if(cookUI != null && Input.GetKeyDown(KeyCode.C))
        {
            cookUI.SetActive(!cookUI.activeSelf);
        }
    }
}