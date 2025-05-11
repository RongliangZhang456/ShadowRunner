using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    private float lastStuckCheck = 0f;
//    // �ƶ�����
//    [Header("Movement Settings")]
//    public float moveSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.1f;


//    // ����ϵͳ
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // ��ɫϵͳ
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // �������
//    private Rigidbody rb;
//    private Renderer rend;

//    // ��ǰ�Ӵ����ĵ���
//    private GameObject currentPlatform;

//    [Header("Stuck Settings")]
//    public float unstuckForce = 10f; // �ɵ��ڵĽ⿨����С

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        rend = GetComponent<Renderer>();
//        rend.material = blackMat;
//        InitializePhysics();
//    }

//    void InitializePhysics()
//    {
//        // ������Ħ���������
//        PhysicMaterial mat = new PhysicMaterial();
//        mat.dynamicFriction = 0;
//        mat.staticFriction = 0;
//        GetComponent<Collider>().material = mat;

//        rb.useGravity = false;
//        currentGravity = new Vector3(0, baseGravity, 0);
//    }

//    void FixedUpdate()
//    {
//        HandleMovement();
//        ApplyCustomGravity();
//        CheckColorMatchOnStay(); // ������ɫ���
//        // ����ٶȵ�����̨
//        Debug.Log($"��ǰ�ٶȣ�{rb.velocity}");
//    }

//    void Update()
//    {
//        HandleInput();
//        HandleEdgeStuck();
//    }

//    void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            TryJump();
//        }

//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            ReverseGravity();
//        }

//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            ToggleColor();
//        }
//    }

//    //void HandleEdgeStuck()
//    //{
//    //    // �����ҵĺ����ٶ��Ƿ�ӽ� 0
//    //    if (Mathf.Abs(rb.velocity.x) == 0f && Mathf.Abs(rb.velocity.y) == 0f)
//    //    {
//    //        // ���ݵ�ǰ����������� 45 �ȵķ�������
//    //        Vector3 unstuckDirection;
//    //        if (isGravityNormal)
//    //        {
//    //            // �������£�ʩ�����Ϸ� 20 �ȵ���
//    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
//    //        }
//    //        else
//    //        {
//    //            // �������ϣ�ʩ�����·� 20 �ȵ���
//    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), -Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
//    //        }

//    //        // ʩ����
//    //        rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
//    //        Debug.Log($"��⵽��ס��ʩ�ӽ⿨�������� {unstuckDirection}����С {unstuckForce}");
//    //    }
//    //}


//    void HandleEdgeStuck()
//    {
//        // ����ȷ�Ŀ��ټ������
//        if (Mathf.Abs(rb.velocity.x) == 0f &&
//            Mathf.Abs(rb.velocity.y) == 0f &&
//           Time.time > lastStuckCheck + 0.5f)
//        {
//            Vector3 unstuckDirection = isGravityNormal ?
//                new Vector3(-30f, 0.3f, 0).normalized :
//                new Vector3(-30f, -0.3f, 0).normalized;

//            rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
//            lastStuckCheck = Time.time;
//        }
//    }


//    void HandleMovement()
//    {
//        float controlFactor = IsGrounded() ? 1f : airControl;

//        // ʹ��AddForce������ֱ������velocity
//        float speedDifference = moveSpeed - rb.velocity.x;
//        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

//        // ����ٶ�����
//        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
//        {
//            rb.velocity = new Vector3(
//                Mathf.Sign(rb.velocity.x) * moveSpeed,
//                rb.velocity.y,
//                0
//            );
//        }
//    }

//    void ApplyCustomGravity()
//    {
//        rb.AddForce(currentGravity * gravityMultiplier, ForceMode.Acceleration);
//    }

//    void TryJump()
//    {
//        if (IsGrounded())
//        {
//            float direction = isGravityNormal ? 1f : -1f;
//            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
//        }
//    }

//    void ReverseGravity()
//    {
//        isGravityNormal = !isGravityNormal;
//        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
//        transform.Rotate(180f, 0f, 0f);
//    }

//    void ToggleColor()
//    {
//        isBlack = !isBlack;
//        rend.material = isBlack ? blackMat : whiteMat;
//    }

//    bool IsGrounded()
//    {
//        float rayLength = 1.1f;
//        Vector3 rayDirection = isGravityNormal ? Vector3.down : Vector3.up;
//        return Physics.Raycast(transform.position, rayDirection, rayLength);
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("BlackBlock") || collision.gameObject.CompareTag("WhiteBlock"))
//        {
//            currentPlatform = collision.gameObject; // ���浱ǰƽ̨
//        }
//    }

//    void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject == currentPlatform)
//        {
//            currentPlatform = null;
//        }
//    }

//    void CheckColorMatchOnStay()
//    {
//        if (currentPlatform == null) return;

//        bool isBlockBlack = currentPlatform.CompareTag("BlackBlock");
//        if (isBlockBlack != isBlack)
//        {
//            Debug.Log("��ɫ��ƥ�䣬��Ϸ������");
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("DeathZone"))
//        {
//            Debug.Log("��������������Ϸ������");
//#if UNITY_EDITOR
//            //UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }

//}







//using UnityEngine;

//[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Animator))]
//public class PlayerController : MonoBehaviour
//{
//    // �ƶ�����
//    [Header("Movement Settings")]
//    public float moveSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.1f;

//    // ����ϵͳ
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // ��ɫϵͳ
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // ��������
//    [Header("Animation Settings")]
//    public string runState = "RunForward";
//    public string jumpUpState = "JumpWhileRunningUp";
//    public string fallState = "FallingLoop";
//    public string sprintState = "Sprint";

//    // �������
//    private Rigidbody rb;
//    private Renderer rend;
//    private Collider col;
//    private Animator anim;

//    // ������
//    private bool isGrounded;
//    private float lastStuckCheck;
//    private float Speed;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rend = GetComponent<Renderer>();
//        col = GetComponent<Collider>();
//        anim = GetComponent<Animator>();

//        rb.useGravity = false;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        currentGravity = new Vector3(0, baseGravity, 0);

//        // ������Ħ���������
//        PhysicMaterial mat = new PhysicMaterial();
//        mat.dynamicFriction = 0;
//        mat.staticFriction = 0;
//        col.material = mat;
//    }

//    void Update()
//    {
//        HandleInput();
//        HandleEdgeStuck();
//        UpdateAnimation(); // ÿ֡���¶���
//    }

//    void FixedUpdate()
//    {
//        HandleMovement();
//        ApplyCustomGravity();

//        // ����ӵ�״̬���ٶ�
//        Debug.Log($"�ӵ�״̬: {isGrounded}, ��ǰ�ٶ�: {rb.velocity}");
//    }

//    void UpdateAnimation()
//    {
//        // ����״̬�л�����
//        if (!isGrounded)
//        {
//            if (rb.velocity.y > 0.1f)
//            {
//                anim.Play(jumpUpState);
//            }
//            else if (rb.velocity.y < -0.1f)
//            {
//                anim.Play(fallState);
//            }
//        }
//        else
//        {
//            Speed = Mathf.Abs(rb.velocity.x);
//            if (Mathf.Abs(rb.velocity.x) > 0.1f)
//            {
//                anim.Play(runState);
//            }
//            else if (Mathf.Abs(rb.velocity.x) > 5f)
//            {
//                anim.Play(sprintState);
//            }
//        }
//    }

//    void HandleInput()
//    {
//        // ��Ծ
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            float direction = isGravityNormal ? 1f : -1f;
//            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
//            anim.Play(jumpUpState, 0, 0f); // ����������Ծ����
//        }

//        // ������ת
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            ReverseGravity();
//        }

//        // ��ɫ�л�
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            ToggleColor();
//        }
//    }

//    void HandleMovement()
//    {
//        float controlFactor = isGrounded ? 1f : airControl;
//        float speedDifference = moveSpeed - rb.velocity.x;
//        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

//        // �ٶ�����
//        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
//        {
//            rb.velocity = new Vector3(
//                Mathf.Sign(rb.velocity.x) * moveSpeed,
//                rb.velocity.y,
//                0
//            );
//        }
//    }

//    void ApplyCustomGravity()
//    {
//        rb.AddForce(currentGravity * gravityMultiplier, ForceMode.Acceleration);
//    }

//    void HandleEdgeStuck()
//    {
//        if (Time.time > lastStuckCheck + 0.5f &&
//            Mathf.Abs(rb.velocity.x) < 0.1f &&
//            !isGrounded)
//        {
//            Vector3 pushDir = isGravityNormal ?
//                new Vector3(-0.7f, 0.3f, 0).normalized :
//                new Vector3(-0.7f, -0.3f, 0).normalized;

//            rb.AddForce(pushDir * 10f, ForceMode.Impulse);
//            lastStuckCheck = Time.time;
//        }
//    }

//    void ReverseGravity()
//    {
//        isGravityNormal = !isGravityNormal;
//        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
//        transform.Rotate(180f, 0f, 0f);
//    }

//    void ToggleColor()
//    {
//        isBlack = !isBlack;
//        rend.material = isBlack ? blackMat : whiteMat;
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        CheckGroundStatus(collision, true);
//    }

//    void OnCollisionStay(Collision collision)
//    {
//        CheckGroundStatus(collision, true);
//    }

//    void OnCollisionExit(Collision collision)
//    {
//        CheckGroundStatus(collision, false);
//    }

//    void CheckGroundStatus(Collision collision, bool enteringOrStaying)
//    {
//        Debug.Log($"�Ӵ�����: {collision.gameObject.name}");
//        Debug.Log(enteringOrStaying);
//        // ��������ֱ�ӷ���
//        if (collision.gameObject.CompareTag("DeathZone")) return;

//        // ����Ƿ���Чƽ̨
//        bool isValidPlatform = false;
//        if (collision.gameObject.CompareTag("BlackBlock") && isBlack) isValidPlatform = true;
//        if (collision.gameObject.CompareTag("WhiteBlock") && !isBlack) isValidPlatform = true;
//        isValidPlatform = true;
//        if (isValidPlatform)
//        {
//            //// ���Ӵ����Ƿ�������·�(������������)���Ϸ�(���ڷ�ת����)
//            //foreach (ContactPoint contact in collision.contacts)
//            //{
//            //    Vector3 relativePoint = transform.InverseTransformPoint(contact.point);
//            //    if ((isGravityNormal && relativePoint.y < -0.5f) ||
//            //        (!isGravityNormal && relativePoint.y > 0.5f))
//            //    {
//            //        isGrounded = enteringOrStaying;
//            //        return;
//            //    }
//            //}

//            isGrounded = enteringOrStaying;
//            return;
//        }

//        // ���û���ҵ���Ч�Ӵ��������˳���ײ
//        if (!enteringOrStaying)
//        {
//            isGrounded = false;
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("DeathZone"))
//        {
//            Debug.Log("��������������Ϸ������");
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }

//    // ������GUI��ʾ
//    void OnGUI()
//    {
//        GUIStyle style = new GUIStyle();
//        style.fontSize = 20;
//        style.normal.textColor = Color.red;

//        GUI.Label(new Rect(10, 10, 300, 50), $"�ӵ�״̬: {isGrounded}", style);
//        GUI.Label(new Rect(10, 40, 300, 50), $"��ǰ�ٶ�: {rb.velocity}", style);
//    }
//}

//using UnityEngine;

//[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator)), RequireComponent(typeof(Collider))]
//public class PlayerController : MonoBehaviour
//{
//    // �ƶ�����
//    [Header("Movement Settings")]
//    public float walkSpeed = 5f;
//    public float sprintSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.2f;

//    // ����ϵͳ
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // ��ɫϵͳ
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // ��������
//    [Header("Animation Settings")]
//    public string runState = "RunForward";
//    public string jumpUpState = "JumpWhileRunningUp";
//    public string fallState = "FallingLoop";
//    public string sprintState = "Sprint";

//    // �������
//    private Animator anim;
//    private Rigidbody rb;
//    private Renderer rend;
//    private Collider col;
//    private bool isGrounded;
//    private float lastStuckCheckTime;
//    private int groundContactCount; // ��¼�����ĽӴ�������

//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody>();
//        rend = GetComponent<Renderer>();
//        col = GetComponent<Collider>();

//        InitializePhysics();
//    }

//    void InitializePhysics()
//    {
//        rb.useGravity = false;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        currentGravity = new Vector3(0, baseGravity, 0);

//        // �Ż��������
//        PhysicMaterial mat = new PhysicMaterial();
//        mat.dynamicFriction = 0;
//        mat.staticFriction = 0;
//        mat.bounciness = 0;
//        col.material = mat;
//    }

//    void Update()
//    {
//        HandleInput();
//        UpdateAnimation();
//        HandleEdgeStuck();
//    }

//    void FixedUpdate()
//    {
//        HandleMovement();
//        ApplyCustomGravity();

//        // ���õ���״̬
//        if (groundContactCount > 0)
//        {
//            isGrounded = true;
//            groundContactCount = 0; // ���ü�����
//        }
//        else
//        {
//            isGrounded = false;
//        }

//        // ����ӵ�״̬
//        Debug.Log($"�ӵ�״̬: {isGrounded}");
//    }

//    void HandleInput()
//    {
//        // ��ɫ�л�
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            ToggleColor();
//        }

//        // ��Ծ
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            TryJump();
//        }

//        // ������ת
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            ReverseGravity();
//        }
//    }

//    void UpdateAnimation()
//    {
//        float speed = Mathf.Abs(rb.velocity.x);

//        Debug.Log($"��ǰ�ٶȣ�{rb.velocity}");

//        // ��Ծ/����״̬
//        if (!isGrounded)
//        {
//            if (rb.velocity.y > 0.1f)
//            {
//                anim.Play(jumpUpState);
//            }
//            else if (rb.velocity.y < -0.1f)
//            {
//                anim.Play(fallState);
//            }
//        }
//        else
//        {
//            // �ƶ�״̬
//            if (speed > 0.1f)
//            {
//                if (speed > sprintSpeed)
//                {
//                    anim.Play(sprintState);
//                }
//                else if (speed > walkSpeed)
//                {
//                    anim.Play(runState);
//                }
//                else
//                {
//                    anim.Play(runState);
//                }
//            }
//        }
//    }

//    void HandleMovement()
//    {
//        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
//        float controlFactor = isGrounded ? 1f : airControl;

//        // �����ƶ�
//        float speedDifference = targetSpeed - rb.velocity.x;
//        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

//        // �ٶ�����
//        if (Mathf.Abs(rb.velocity.x) > targetSpeed)
//        {
//            rb.velocity = new Vector3(
//                Mathf.Sign(rb.velocity.x) * targetSpeed,
//                rb.velocity.y,
//                0
//            );
//        }
//    }

//    void TryJump()
//    {
//        float direction = isGravityNormal ? 1f : -1f;
//        rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
//        anim.Play(jumpUpState, 0, 0f);
//    }

//    void HandleEdgeStuck()
//    {
//        if (Time.time > lastStuckCheckTime + 0.5f &&
//            Mathf.Abs(rb.velocity.x) < 0.1f &&
//            !isGrounded)
//        {
//            Vector3 pushDir = isGravityNormal ?
//                new Vector3(-0.7f, 0.3f, 0).normalized :
//                new Vector3(-0.7f, -0.3f, 0).normalized;

//            rb.AddForce(pushDir * 10f, ForceMode.Impulse);
//            lastStuckCheckTime = Time.time;
//        }
//    }

//    void ApplyCustomGravity()
//    {
//        rb.AddForce(currentGravity * gravityMultiplier, ForceMode.Acceleration);
//    }

//    void ReverseGravity()
//    {
//        isGravityNormal = !isGravityNormal;
//        currentGravity.y = isGravityNormal ? baseGravity : -baseGravity;
//        transform.Rotate(180f, 0f, 0f);
//    }

//    void ToggleColor()
//    {
//        isBlack = !isBlack;
//        rend.material = isBlack ? blackMat : whiteMat;
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        // ����Ƿ�վ��ƽ̨��
//        if (IsValidPlatform(collision.gameObject))
//        {
//            // ���Ӵ����Ƿ�������·�(������������)���Ϸ�(���ڷ�ת����)
//            foreach (ContactPoint contact in collision.contacts)
//            {
//                Vector3 relativePoint = transform.InverseTransformPoint(contact.point);
//                if ((isGravityNormal && relativePoint.y < -0.5f) ||
//                    (!isGravityNormal && relativePoint.y > 0.5f))
//                {
//                    groundContactCount++;
//                    break;
//                }
//            }
//        }
//    }

//    void OnCollisionStay(Collision collision)
//    {
//        // ���������ײ�����
//        if (IsValidPlatform(collision.gameObject))
//        {
//            foreach (ContactPoint contact in collision.contacts)
//            {
//                Vector3 relativePoint = transform.InverseTransformPoint(contact.point);
//                if ((isGravityNormal && relativePoint.y < -0.5f) ||
//                    (!isGravityNormal && relativePoint.y > 0.5f))
//                {
//                    groundContactCount++;
//                    break;
//                }
//            }
//        }
//    }

//    bool IsValidPlatform(GameObject platform)
//    {
//        if (platform.CompareTag("DeathZone")) return false;

//        // ���ƽ̨��ɫ�Ƿ�ƥ��
//        Platform platformComponent = platform.GetComponent<Platform>();
//        if (platformComponent != null)
//        {
//            return platformComponent.isBlackPlatform == isBlack;
//        }

//        // ���û��Platform�������Ĭ��Ϊ��Чƽ̨
//        return true;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("DeathZone"))
//        {
//            Debug.Log("��������������Ϸ������");

//#if UNITY_EDITOR
//            //UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }
//}
