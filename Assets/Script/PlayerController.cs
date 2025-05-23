using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    [Range(0, 1)] public float airControl = 0.1f;

    [Header("Gravity Settings")]
    public float gravityMultiplier = 2f;
    public float baseGravity = -9.81f;
    public float gravityCooldown = 0.5f;
    public bool enableAirGravityFlip = false;
	private Vector3 currentGravity;
    private bool isGravityNormal = true;
    private float lastGravityFlipTime;
	private bool isFlippedGravityInAir = false;

	[Header("Color Settings")]
    public Renderer[] colorRenderers;
    public Material blackMat;
    public Material whiteMat;
    public bool isDefaultBlack = true;
	[HideInInspector] public bool isCurrentBlack = true;

    [Header("Animation Settings")]
    public string runState = "RunForward";
    public string jumpUpState = "JumpWhileRunningUp";
    public string fallState = "FallingLoop";
    public string sprintState = "Sprint";
    public string rollState = "RollForward";
    public float sprintThreshold = 5f;
    public float hardLandingSpeedThreshold = -5f;

    [Header("Death Settings")]
    public bool enableRespawnToStart = true;

	[SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private Animator anim;

    private int speedHash;
    private int verticalSpeedHash;
    private int isGroundedHash;
    private int jumpTriggerHash;
    private int hardLandingHash;

    private bool isGrounded;
    private float lastStuckCheck;
    private bool shouldHardLand;
    private bool isActionLocked;
    private bool isInJumpState;

    private GameObject currentPlatform;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<Collider>();
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (colorRenderers == null || colorRenderers.Length == 0)
        {
            FindColorRenderers();
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

        InitializePlayerState();
	}

    void FindColorRenderers()
    {
        Transform meshRoot = transform.Find("Mesh");
        if (meshRoot != null)
        {
            List<Renderer> renderers = new List<Renderer>();

            Transform body = meshRoot.Find("Body");
            if (body != null)
            {
                AddRendererIfFound(body, "Chest", renderers);
                AddRendererIfFound(body, "Arms", renderers);
                AddRendererIfFound(body, "Legs", renderers);
            }

            colorRenderers = renderers.ToArray();
        }

        if (colorRenderers == null || colorRenderers.Length == 0)
        {
            Debug.LogWarning("未找到需要变色的渲染器！请手动赋值或检查模型层级");
        }
    }

    void AddRendererIfFound(Transform parent, string childName, List<Renderer> renderers)
    {
        Transform child = parent.Find(childName);
        if (child != null)
        {
            Renderer r = child.GetComponent<Renderer>();
            if (r != null) renderers.Add(r);
        }
    }

	// Called when the game starts or when the player respawns
	void InitializePlayerState()
	{
		transform.position = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		isGrounded = true;
		isCurrentBlack = isDefaultBlack;
		UpdateColorMaterial();
		// Reset gravity
		if (currentGravity.y * baseGravity < 0f)
		{
			ReverseGravity();
		}
	}

	void Update()
    {
        if (col == null) return;

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
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
		if (!enableAirGravityFlip)
		{
			if (isFlippedGravityInAir) return;
		}
		if (Time.time - lastGravityFlipTime < gravityCooldown) return;

        Vector3 pivotPoint = GetPivotPosition();

        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;

        transform.Rotate(180f, 0f, 0f, Space.World);

        Vector3 newPivot = GetPivotPosition();
        transform.position += pivotPoint - newPivot;

        float newYVelocity = Mathf.Clamp(-rb.velocity.y * 0.7f, -jumpForce * 1.5f, jumpForce * 1.5f);
        rb.velocity = new Vector3(rb.velocity.x, newYVelocity, rb.velocity.z);

        lastGravityFlipTime = Time.time;
		isFlippedGravityInAir = true;
	}

    Vector3 GetPivotPosition()
    {
        if (col == null) return transform.position;
        float offset = col.bounds.extents.y;
        return transform.position + (isGravityNormal ? Vector3.up * offset : Vector3.down * offset);
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
        isCurrentBlack = !isCurrentBlack;
        UpdateColorMaterial();

        // 新增：站在平台上变色时立即检查失败
        if (isGrounded && currentPlatform != null)
        {
            if ((currentPlatform.CompareTag("BlackBlock") && !isCurrentBlack) ||
                (currentPlatform.CompareTag("WhiteBlock") && isCurrentBlack))
            {
                TriggerFailure();
            }
        }
    }

    void UpdateColorMaterial()
    {
        Material targetMat = isCurrentBlack ? blackMat : whiteMat;

        foreach (Renderer renderer in colorRenderers)
        {
            if (renderer != null)
            {
                renderer.material = targetMat;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckFailureCollision(collision.gameObject);

        bool wasGrounded = isGrounded;
        CheckGroundStatus(collision, true);

        if (!wasGrounded && isGrounded)
        {
            if (shouldHardLand)
            {
                anim.Play(rollState, 0, 0f);
            }
            else
            {
                anim.Play(runState, 0, 0f); // 新增：软着陆也切换回跑步
            }
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
        if (other.CompareTag("BlackBlock") && !isCurrentBlack)
        {
            TriggerFailure();
        }
        else if (other.CompareTag("WhiteBlock") && isCurrentBlack)
        {
            TriggerFailure();
        }
    }

    void CheckGroundStatus(Collision collision, bool enteringOrStaying)
    {
        bool isValidPlatform = false;

        if (collision.gameObject.CompareTag("BlackBlock"))
        {
            isValidPlatform = isCurrentBlack;
            currentPlatform = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("WhiteBlock"))
        {
            isValidPlatform = !isCurrentBlack;
            currentPlatform = collision.gameObject;
        }
        else
        {
            currentPlatform = null;
        }

        if (enteringOrStaying)
        {
            isGrounded = isValidPlatform;
			if (isGrounded)
			{
				isFlippedGravityInAir = false;
			}
		}
        else
        {
            isGrounded = false;
            currentPlatform = null;
        }
    }

    void TriggerFailure()
    {
        Debug.Log("触碰错误颜色！游戏结束");

        if (enableRespawnToStart)
        {
            InitializePlayerState();
		}
        else
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
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
            TriggerFailure();
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
        GUI.Label(new Rect(10, 70, 300, 50), $"重力方向: {(isGravityNormal ? "正常" : "反转")}", style);
        GUI.Label(new Rect(10, 100, 300, 50), $"反重力冷却: {cooldownLeft:F1}s", style);
    }

    void OnDrawGizmosSelected()
    {
        if (col == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(GetPivotPosition(), 0.1f);
    }
}
