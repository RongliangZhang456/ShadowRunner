using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // 移动参数
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    [Range(0, 1)] public float airControl = 0.1f;

    // 重力系统
    [Header("Gravity Settings")]
    public float gravityMultiplier = 2f;
    public float baseGravity = -9.81f;
    public float gravityCooldown = 0.5f;
    private Vector3 currentGravity;
    private bool isGravityNormal = true;
    private float lastGravityFlipTime;

    // 颜色系统
    [Header("Color Settings")]
    public Renderer targetRenderer;
    public Material blackMat;
    public Material whiteMat;
    [HideInInspector] public bool isBlack = true;

    // 动画参数
    [Header("Animation Settings")]
    public string runState = "RunForward";
    public string jumpUpState = "JumpWhileRunningUp";
    public string fallState = "FallingLoop";
    public string sprintState = "Sprint";
    public string rollState = "RollForward";
    public float sprintThreshold = 5f;
    public float hardLandingSpeedThreshold = -5f;

    // 组件引用
    private Rigidbody rb;
    private Collider col;
    private Animator anim;

    // 动画参数哈希
    private int speedHash;
    private int verticalSpeedHash;
    private int isGroundedHash;
    private int jumpTriggerHash;
    private int hardLandingHash;

    // 状态变量
    private bool isGrounded;
    private float lastStuckCheck;
    private bool shouldHardLand;
    private bool isActionLocked;
    private bool isInJumpState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();

        if (targetRenderer == null)
        {
            Transform chest = transform.Find("Mesh/Body/Chest");
            if (chest != null) targetRenderer = chest.GetComponent<Renderer>();
        }

        speedHash = Animator.StringToHash("Speed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        jumpTriggerHash = Animator.StringToHash("JumpTrigger");
        hardLandingHash = Animator.StringToHash("HardLanding");

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        currentGravity = new Vector3(0, baseGravity, 0);

        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        col.material = mat;

        UpdateColorMaterial();
    }

    void Update()
    {
        UpdateActionLockStatus();
        isInJumpState = anim.GetCurrentAnimatorStateInfo(0).IsName(jumpUpState) ||
                       anim.GetCurrentAnimatorStateInfo(0).IsName(fallState);

        HandleInput();
        HandleEdgeStuck();
        UpdateAnimationParameters();

        shouldHardLand = rb.velocity.y < hardLandingSpeedThreshold;
        anim.SetBool(hardLandingHash, shouldHardLand);
    }

    void UpdateActionLockStatus()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        isActionLocked = stateInfo.IsName(jumpUpState) ||
                        (stateInfo.IsName(fallState) && !isGrounded);
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyCustomGravity();
    }

    void UpdateAnimationParameters()
    {
        if (!isActionLocked)
        {
            anim.SetFloat(speedHash, Mathf.Abs(rb.velocity.x));
            anim.SetFloat(verticalSpeedHash, rb.velocity.y);
        }
        anim.SetBool(isGroundedHash, isGrounded);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isActionLocked)
        {
            float direction = isGravityNormal ? 1f : -1f;
            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
            anim.SetTrigger(jumpTriggerHash);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isInJumpState)
        {
            ReverseGravity();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColor();
        }
    }

    void ReverseGravity()
    {
        if (Time.time - lastGravityFlipTime < gravityCooldown) return;

        // 获取角色顶部位置
        Vector3 topPosition = transform.position + (isGravityNormal ? Vector3.up * col.bounds.extents.y : Vector3.down * col.bounds.extents.y);

        // 翻转重力方向
        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;

        // 旋转角色
        transform.Rotate(180f, 0f, 0f);

        // 调整角色位置，使顶部位置保持不变
        Vector3 newTopPosition = transform.position + (isGravityNormal ? Vector3.up * col.bounds.extents.y : Vector3.down * col.bounds.extents.y);
        transform.position += topPosition - newTopPosition;

        // 调整垂直速度，避免翻转后速度过大
        float newYVelocity = Mathf.Clamp(-rb.velocity.y * 0.7f, -jumpForce * 1.2f, jumpForce * 1.2f);
        rb.velocity = new Vector3(rb.velocity.x, newYVelocity, rb.velocity.z);

        lastGravityFlipTime = Time.time;
    }

    void HandleMovement()
    {
        float controlFactor = isGrounded ? 1f : airControl;
        float speedDifference = moveSpeed - rb.velocity.x;
        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

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

    void HandleEdgeStuck()
    {
        if (Time.time > lastStuckCheck + 0.5f &&
            Mathf.Abs(rb.velocity.x) < 0.1f &&
            !isGrounded &&
            !isActionLocked)
        {
            Vector3 pushDir = isGravityNormal ?
                new Vector3(-0.7f, 0.3f, 0).normalized :
                new Vector3(-0.7f, -0.3f, 0).normalized;

            rb.AddForce(pushDir * 10f, ForceMode.Impulse);
            lastStuckCheck = Time.time;
        }
    }

    void ToggleColor()
    {
        isBlack = !isBlack;
        UpdateColorMaterial();
    }

    void UpdateColorMaterial()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material = isBlack ? blackMat : whiteMat;
        }
        else
        {
            Debug.LogWarning("目标渲染器未赋值！");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 立即检测错误颜色碰撞
        CheckFailureCollision(collision.gameObject);

        bool wasGrounded = isGrounded;
        CheckGroundStatus(collision, true);

        if (!wasGrounded && isGrounded && shouldHardLand)
        {
            anim.Play(rollState, 0, 0f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        CheckGroundStatus(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        CheckGroundStatus(collision, false);
    }

    void CheckFailureCollision(GameObject other)
    {
        if (other.CompareTag("BlackBlock") && !isBlack)
        {
            TriggerFailure();
        }
        else if (other.CompareTag("WhiteBlock") && isBlack)
        {
            TriggerFailure();
        }
    }

    void CheckGroundStatus(Collision collision, bool enteringOrStaying)
    {
        if (collision.gameObject.CompareTag("DeathZone")) return;

        bool isValidPlatform = false;

        if (collision.gameObject.CompareTag("BlackBlock"))
        {
            isValidPlatform = isBlack;
        }
        else if (collision.gameObject.CompareTag("WhiteBlock"))
        {
            isValidPlatform = !isBlack;
        }

        if (enteringOrStaying)
        {
            isGrounded = isValidPlatform;
        }
        else
        {
            isGrounded = false;
        }
    }

    void TriggerFailure()
    {
        Debug.Log("触碰错误颜色！游戏结束");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnRollComplete()
    {
        anim.Play(runState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            TriggerFailure();
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, 10, 300, 50), $"接地状态: {isGrounded}", style);
        GUI.Label(new Rect(10, 40, 300, 50), $"速度: {rb.velocity}", style);
    }
}