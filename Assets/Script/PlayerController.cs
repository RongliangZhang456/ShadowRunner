using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastStuckCheck = 0f;
    // 移动参数
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    [Range(0, 1)] public float airControl = 0.1f;


    // 重力系统
    [Header("Gravity Settings")]
    public float gravityMultiplier = 2f;
    public float baseGravity = -9.81f;
    private Vector3 currentGravity;
    private bool isGravityNormal = true;

    // 颜色系统
    [Header("Color Settings")]
    public Material blackMat;
    public Material whiteMat;
    [HideInInspector] public bool isBlack = true;

    // 组件引用
    private Rigidbody rb;
    private Renderer rend;

    // 当前接触到的地面
    private GameObject currentPlatform;

    [Header("Stuck Settings")]
    public float unstuckForce = 10f; // 可调节的解卡力大小

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rend = GetComponent<Renderer>();
        rend.material = blackMat;
        InitializePhysics();
    }

    void InitializePhysics()
    {
        // 创建无摩擦物理材质
        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        GetComponent<Collider>().material = mat;

        rb.useGravity = false;
        currentGravity = new Vector3(0, baseGravity, 0);
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyCustomGravity();
        CheckColorMatchOnStay(); // 新增颜色检测
        // 输出速度到控制台
        Debug.Log($"当前速度：{rb.velocity}");
    }

    void Update()
    {
        HandleInput();
        HandleEdgeStuck();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ReverseGravity();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColor();
        }
    }

    //void HandleEdgeStuck()
    //{
    //    // 检测玩家的横向速度是否接近 0
    //    if (Mathf.Abs(rb.velocity.x) == 0f && Mathf.Abs(rb.velocity.y) == 0f)
    //    {
    //        // 根据当前重力方向计算 45 度的方向向量
    //        Vector3 unstuckDirection;
    //        if (isGravityNormal)
    //        {
    //            // 重力向下，施加左上方 20 度的力
    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
    //        }
    //        else
    //        {
    //            // 重力向上，施加左下方 20 度的力
    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), -Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
    //        }

    //        // 施加力
    //        rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
    //        Debug.Log($"检测到卡住，施加解卡力：方向 {unstuckDirection}，大小 {unstuckForce}");
    //    }
    //}


    void HandleEdgeStuck()
    {
        // 更精确的卡顿检测条件
        if (Mathf.Abs(rb.velocity.x) == 0f &&
            Mathf.Abs(rb.velocity.y) == 0f &&
           Time.time > lastStuckCheck + 0.5f)
        {
            Vector3 unstuckDirection = isGravityNormal ?
                new Vector3(-30f, 0.3f, 0).normalized :
                new Vector3(-30f, -0.3f, 0).normalized;

            rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
            lastStuckCheck = Time.time;
        }
    }


    void HandleMovement()
    {
        float controlFactor = IsGrounded() ? 1f : airControl;

        // 使用AddForce而不是直接设置velocity
        float speedDifference = moveSpeed - rb.velocity.x;
        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

        // 添加速度限制
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            rb.velocity = new Vector3(
                Mathf.Sign(rb.velocity.x) * moveSpeed,
                rb.velocity.y,
                0
            );
        }
    }

    void ApplyCustomGravity()
    {
        rb.AddForce(currentGravity * gravityMultiplier, ForceMode.Acceleration);
    }

    void TryJump()
    {
        if (IsGrounded())
        {
            float direction = isGravityNormal ? 1f : -1f;
            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
        }
    }

    void ReverseGravity()
    {
        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
        transform.Rotate(180f, 0f, 0f);
    }

    void ToggleColor()
    {
        isBlack = !isBlack;
        rend.material = isBlack ? blackMat : whiteMat;
    }

    bool IsGrounded()
    {
        float rayLength = 1.1f;
        Vector3 rayDirection = isGravityNormal ? Vector3.down : Vector3.up;
        return Physics.Raycast(transform.position, rayDirection, rayLength);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BlackBlock") || collision.gameObject.CompareTag("WhiteBlock"))
        {
            currentPlatform = collision.gameObject; // 保存当前平台
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    void CheckColorMatchOnStay()
    {
        if (currentPlatform == null) return;

        bool isBlockBlack = currentPlatform.CompareTag("BlackBlock");
        if (isBlockBlack != isBlack)
        {
            Debug.Log("颜色不匹配，游戏结束！");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("进入死亡区域，游戏结束！");
#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayDirection = isGravityNormal ? Vector3.down : Vector3.up;
        Gizmos.DrawRay(transform.position, rayDirection * 1.1f);
    }
}
