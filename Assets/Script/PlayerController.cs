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
    public float gravityCooldown = 0.5f; // 反重力冷却时间
    private Vector3 currentGravity;
    private bool isGravityNormal = true;
    private float lastGravityFlipTime;

    // 颜色系统
    [Header("Color Settings")]
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
    private Renderer rend;
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
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();

        // 初始化动画参数哈希
        speedHash = Animator.StringToHash("Speed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        jumpTriggerHash = Animator.StringToHash("JumpTrigger");
        hardLandingHash = Animator.StringToHash("HardLanding");

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        currentGravity = new Vector3(0, baseGravity, 0);

        // 设置无摩擦物理材质
        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        col.material = mat;
    }

    void Update()
    {
        // 更新动作锁状态
        UpdateActionLockStatus();

        // 更新跳跃状态
        isInJumpState = anim.GetCurrentAnimatorStateInfo(0).IsName(jumpUpState) ||
                       anim.GetCurrentAnimatorStateInfo(0).IsName(fallState);

        HandleInput();

        HandleEdgeStuck();
        UpdateAnimationParameters();

        // 实时检测是否需要硬着陆
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
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isActionLocked)
        {
            float direction = isGravityNormal ? 1f : -1f;
            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
            anim.SetTrigger(jumpTriggerHash);
        }

        // 反重力（仅在跳跃状态可用+冷却检查）
        if (Input.GetKeyDown(KeyCode.LeftShift) && isInJumpState)
        {
            ReverseGravity();
        }

        // 颜色切换
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColor();
        }
    }

    void ReverseGravity()
    {
        // 冷却时间检查
        if (Time.time - lastGravityFlipTime < gravityCooldown) return;

        // 执行反重力
        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
        transform.Rotate(180f, 0f, 0f);

        // 安全修改速度（防止连续反转导致速度失控）
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
        rend.material = isBlack ? blackMat : whiteMat;
    }

    void OnCollisionEnter(Collision collision)
    {
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

    void CheckGroundStatus(Collision collision, bool enteringOrStaying)
    {
        if (collision.gameObject.CompareTag("DeathZone")) return;

        bool isValidPlatform = false;
        if (collision.gameObject.CompareTag("BlackBlock") && isBlack) isValidPlatform = true;
        if (collision.gameObject.CompareTag("WhiteBlock") && !isBlack) isValidPlatform = true;

        if (isValidPlatform)
        {
            isGrounded = enteringOrStaying;
        }
        else if (!enteringOrStaying)
        {
            isGrounded = false;
        }
    }

    public void OnRollComplete()
    {
        anim.Play(runState);
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

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.red;

        float cooldownLeft = Mathf.Max(0, gravityCooldown - (Time.time - lastGravityFlipTime));

        GUI.Label(new Rect(10, 10, 300, 50), $"接地状态: {isGrounded}", style);
        GUI.Label(new Rect(10, 40, 300, 50), $"速度: {rb.velocity}", style);
        GUI.Label(new Rect(10, 70, 300, 50), $"跳跃状态: {isInJumpState}", style);
        GUI.Label(new Rect(10, 100, 300, 50), $"反重力冷却: {cooldownLeft:F1}s", style);
    }
}