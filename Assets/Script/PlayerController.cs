using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastStuckCheck = 0f;
    // �ƶ�����
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    [Range(0, 1)] public float airControl = 0.1f;


    // ����ϵͳ
    [Header("Gravity Settings")]
    public float gravityMultiplier = 2f;
    public float baseGravity = -9.81f;
    private Vector3 currentGravity;
    private bool isGravityNormal = true;

    // ��ɫϵͳ
    [Header("Color Settings")]
    public Material blackMat;
    public Material whiteMat;
    [HideInInspector] public bool isBlack = true;

    // �������
    private Rigidbody rb;
    private Renderer rend;

    // ��ǰ�Ӵ����ĵ���
    private GameObject currentPlatform;

    [Header("Stuck Settings")]
    public float unstuckForce = 10f; // �ɵ��ڵĽ⿨����С

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
        // ������Ħ���������
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
        CheckColorMatchOnStay(); // ������ɫ���
        // ����ٶȵ�����̨
        Debug.Log($"��ǰ�ٶȣ�{rb.velocity}");
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
    //    // �����ҵĺ����ٶ��Ƿ�ӽ� 0
    //    if (Mathf.Abs(rb.velocity.x) == 0f && Mathf.Abs(rb.velocity.y) == 0f)
    //    {
    //        // ���ݵ�ǰ����������� 45 �ȵķ�������
    //        Vector3 unstuckDirection;
    //        if (isGravityNormal)
    //        {
    //            // �������£�ʩ�����Ϸ� 20 �ȵ���
    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
    //        }
    //        else
    //        {
    //            // �������ϣ�ʩ�����·� 20 �ȵ���
    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), -Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
    //        }

    //        // ʩ����
    //        rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
    //        Debug.Log($"��⵽��ס��ʩ�ӽ⿨�������� {unstuckDirection}����С {unstuckForce}");
    //    }
    //}


    void HandleEdgeStuck()
    {
        // ����ȷ�Ŀ��ټ������
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

        // ʹ��AddForce������ֱ������velocity
        float speedDifference = moveSpeed - rb.velocity.x;
        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

        // ����ٶ�����
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
            currentPlatform = collision.gameObject; // ���浱ǰƽ̨
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
            Debug.Log("��ɫ��ƥ�䣬��Ϸ������");
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
            Debug.Log("��������������Ϸ������");
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
