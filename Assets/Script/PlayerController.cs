using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // �ƶ�����
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    [Range(0, 1)] public float airControl = 0.1f;

    // ����ϵͳ
    [Header("Gravity Settings")]
    public float gravityMultiplier = 2f;
    public float baseGravity = -9.81f;
    public float gravityCooldown = 0.5f; // ��������ȴʱ��
    private Vector3 currentGravity;
    private bool isGravityNormal = true;
    private float lastGravityFlipTime;

    // ��ɫϵͳ
    [Header("Color Settings")]
    public Material blackMat;
    public Material whiteMat;
    [HideInInspector] public bool isBlack = true;

    // ��������
    [Header("Animation Settings")]
    public string runState = "RunForward";
    public string jumpUpState = "JumpWhileRunningUp";
    public string fallState = "FallingLoop";
    public string sprintState = "Sprint";
    public string rollState = "RollForward";
    public float sprintThreshold = 5f;
    public float hardLandingSpeedThreshold = -5f;

    // �������
    private Rigidbody rb;
    private Renderer rend;
    private Collider col;
    private Animator anim;

    // ����������ϣ
    private int speedHash;
    private int verticalSpeedHash;
    private int isGroundedHash;
    private int jumpTriggerHash;
    private int hardLandingHash;

    // ״̬����
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

        // ��ʼ������������ϣ
        speedHash = Animator.StringToHash("Speed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        jumpTriggerHash = Animator.StringToHash("JumpTrigger");
        hardLandingHash = Animator.StringToHash("HardLanding");

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        currentGravity = new Vector3(0, baseGravity, 0);

        // ������Ħ���������
        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        col.material = mat;
    }

    void Update()
    {
        // ���¶�����״̬
        UpdateActionLockStatus();

        // ������Ծ״̬
        isInJumpState = anim.GetCurrentAnimatorStateInfo(0).IsName(jumpUpState) ||
                       anim.GetCurrentAnimatorStateInfo(0).IsName(fallState);

        HandleInput();

        HandleEdgeStuck();
        UpdateAnimationParameters();

        // ʵʱ����Ƿ���ҪӲ��½
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
        // ��Ծ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isActionLocked)
        {
            float direction = isGravityNormal ? 1f : -1f;
            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
            anim.SetTrigger(jumpTriggerHash);
        }

        // ��������������Ծ״̬����+��ȴ��飩
        if (Input.GetKeyDown(KeyCode.LeftShift) && isInJumpState)
        {
            ReverseGravity();
        }

        // ��ɫ�л�
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColor();
        }
    }

    void ReverseGravity()
    {
        // ��ȴʱ����
        if (Time.time - lastGravityFlipTime < gravityCooldown) return;

        // ִ�з�����
        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
        transform.Rotate(180f, 0f, 0f);

        // ��ȫ�޸��ٶȣ���ֹ������ת�����ٶ�ʧ�أ�
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
            Debug.Log("��������������Ϸ������");
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

        GUI.Label(new Rect(10, 10, 300, 50), $"�ӵ�״̬: {isGrounded}", style);
        GUI.Label(new Rect(10, 40, 300, 50), $"�ٶ�: {rb.velocity}", style);
        GUI.Label(new Rect(10, 70, 300, 50), $"��Ծ״̬: {isInJumpState}", style);
        GUI.Label(new Rect(10, 100, 300, 50), $"��������ȴ: {cooldownLeft:F1}s", style);
    }
}