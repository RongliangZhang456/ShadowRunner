using UnityEngine;
using System.Collections.Generic;

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
    public float gravityCooldown = 0.5f;
    private Vector3 currentGravity;
    private bool isGravityNormal = true;
    private float lastGravityFlipTime;

    // ��ɫϵͳ
    [Header("Color Settings")]
    public Renderer[] colorRenderers; // ��Ҫ��ɫ��������Ⱦ����Chest, Arms, Legs�ȣ�
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
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();

        // �Զ�������Ҫ��ɫ����Ⱦ��
        if (colorRenderers == null || colorRenderers.Length == 0)
        {
            FindColorRenderers();
        }

        // ��ʼ����������
        speedHash = Animator.StringToHash("Speed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        jumpTriggerHash = Animator.StringToHash("JumpTrigger");
        hardLandingHash = Animator.StringToHash("HardLanding");

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        currentGravity = new Vector3(0, baseGravity, 0);

        // �������
        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        col.material = mat;

        UpdateColorMaterial();
    }

    // �Զ�������Ҫ��ɫ����Ⱦ��
    void FindColorRenderers()
    {
        Transform meshRoot = transform.Find("Mesh");
        if (meshRoot != null)
        {
            List<Renderer> renderers = new List<Renderer>();

            // �����������λ
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
            Debug.LogWarning("δ�ҵ���Ҫ��ɫ����Ⱦ�������ֶ���ֵ����ģ�Ͳ㼶");
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

        // ��¼��ǰ����λ�ã�������������
        Vector3 pivotPoint = GetPivotPosition();

        // ��ת����
        isGravityNormal = !isGravityNormal;
        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;

        // ����ռ���ת
        transform.Rotate(180f, 0f, 0f, Space.World);

        // λ�ò���
        Vector3 newPivot = GetPivotPosition();
        transform.position += pivotPoint - newPivot;

        // �ٶȵ���
        float newYVelocity = Mathf.Clamp(-rb.velocity.y * 0.7f, -jumpForce * 1.5f, jumpForce * 1.5f);
        rb.velocity = new Vector3(rb.velocity.x, newYVelocity, rb.velocity.z);

        lastGravityFlipTime = Time.time;
    }

    Vector3 GetPivotPosition()
    {
        // ���ݵ�ǰ�������򷵻���ת����λ��
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
        isBlack = !isBlack;
        UpdateColorMaterial();
    }

    void UpdateColorMaterial()
    {
        Material targetMat = isBlack ? blackMat : whiteMat;

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
        //if (collision.gameObject.CompareTag("DeathZone")) return;

        bool isValidPlatform = false;

        if (collision.gameObject.CompareTag("BlackBlock"))
        {
            isValidPlatform = isBlack;
        }
        else if (collision.gameObject.CompareTag("WhiteBlock"))
        {
            isValidPlatform = !isBlack;
        }

        isValidPlatform = true;

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
        Debug.Log("����������ɫ����Ϸ����");
#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
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

        float cooldownLeft = Mathf.Max(0, gravityCooldown - (Time.time - lastGravityFlipTime));

        GUI.Label(new Rect(10, 10, 300, 50), $"�ӵ�״̬: {isGrounded}", style);
        GUI.Label(new Rect(10, 40, 300, 50), $"�ٶ�: {rb.velocity}", style);
        GUI.Label(new Rect(10, 70, 300, 50), $"��������: {(isGravityNormal ? "����" : "��ת")}", style);
        GUI.Label(new Rect(10, 100, 300, 50), $"��������ȴ: {cooldownLeft:F1}s", style);
    }

    // ���Կ��ӻ�
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(GetPivotPosition(), 0.1f);
    }
}