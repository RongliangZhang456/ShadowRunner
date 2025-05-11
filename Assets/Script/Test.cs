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
//    // 移动参数
//    [Header("Movement Settings")]
//    public float moveSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.1f;


//    // 重力系统
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // 颜色系统
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // 组件引用
//    private Rigidbody rb;
//    private Renderer rend;

//    // 当前接触到的地面
//    private GameObject currentPlatform;

//    [Header("Stuck Settings")]
//    public float unstuckForce = 10f; // 可调节的解卡力大小

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
//        // 创建无摩擦物理材质
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
//        CheckColorMatchOnStay(); // 新增颜色检测
//        // 输出速度到控制台
//        Debug.Log($"当前速度：{rb.velocity}");
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
//    //    // 检测玩家的横向速度是否接近 0
//    //    if (Mathf.Abs(rb.velocity.x) == 0f && Mathf.Abs(rb.velocity.y) == 0f)
//    //    {
//    //        // 根据当前重力方向计算 45 度的方向向量
//    //        Vector3 unstuckDirection;
//    //        if (isGravityNormal)
//    //        {
//    //            // 重力向下，施加左上方 20 度的力
//    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
//    //        }
//    //        else
//    //        {
//    //            // 重力向上，施加左下方 20 度的力
//    //            unstuckDirection = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * 20), -Mathf.Sin(Mathf.Deg2Rad * 20), 0).normalized;
//    //        }

//    //        // 施加力
//    //        rb.AddForce(unstuckDirection * unstuckForce, ForceMode.Impulse);
//    //        Debug.Log($"检测到卡住，施加解卡力：方向 {unstuckDirection}，大小 {unstuckForce}");
//    //    }
//    //}


//    void HandleEdgeStuck()
//    {
//        // 更精确的卡顿检测条件
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

//        // 使用AddForce而不是直接设置velocity
//        float speedDifference = moveSpeed - rb.velocity.x;
//        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

//        // 添加速度限制
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
//            currentPlatform = collision.gameObject; // 保存当前平台
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
//            Debug.Log("颜色不匹配，游戏结束！");
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
//            Debug.Log("进入死亡区域，游戏结束！");
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
//    // 移动参数
//    [Header("Movement Settings")]
//    public float moveSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.1f;

//    // 重力系统
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // 颜色系统
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // 动画参数
//    [Header("Animation Settings")]
//    public string runState = "RunForward";
//    public string jumpUpState = "JumpWhileRunningUp";
//    public string fallState = "FallingLoop";
//    public string sprintState = "Sprint";

//    // 组件引用
//    private Rigidbody rb;
//    private Renderer rend;
//    private Collider col;
//    private Animator anim;

//    // 地面检测
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

//        // 设置无摩擦物理材质
//        PhysicMaterial mat = new PhysicMaterial();
//        mat.dynamicFriction = 0;
//        mat.staticFriction = 0;
//        col.material = mat;
//    }

//    void Update()
//    {
//        HandleInput();
//        HandleEdgeStuck();
//        UpdateAnimation(); // 每帧更新动画
//    }

//    void FixedUpdate()
//    {
//        HandleMovement();
//        ApplyCustomGravity();

//        // 输出接地状态和速度
//        Debug.Log($"接地状态: {isGrounded}, 当前速度: {rb.velocity}");
//    }

//    void UpdateAnimation()
//    {
//        // 根据状态切换动画
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
//        // 跳跃
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            float direction = isGravityNormal ? 1f : -1f;
//            rb.AddForce(Vector3.up * jumpForce * direction, ForceMode.Impulse);
//            anim.Play(jumpUpState, 0, 0f); // 立即播放跳跃动画
//        }

//        // 重力反转
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            ReverseGravity();
//        }

//        // 颜色切换
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

//        // 速度限制
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
//        Debug.Log($"接触物体: {collision.gameObject.name}");
//        Debug.Log(enteringOrStaying);
//        // 死亡区域直接返回
//        if (collision.gameObject.CompareTag("DeathZone")) return;

//        // 检查是否有效平台
//        bool isValidPlatform = false;
//        if (collision.gameObject.CompareTag("BlackBlock") && isBlack) isValidPlatform = true;
//        if (collision.gameObject.CompareTag("WhiteBlock") && !isBlack) isValidPlatform = true;
//        isValidPlatform = true;
//        if (isValidPlatform)
//        {
//            //// 检查接触点是否在玩家下方(对于正常重力)或上方(对于反转重力)
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

//        // 如果没有找到有效接触点且是退出碰撞
//        if (!enteringOrStaying)
//        {
//            isGrounded = false;
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("DeathZone"))
//        {
//            Debug.Log("进入死亡区域，游戏结束！");
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }

//    // 调试用GUI显示
//    void OnGUI()
//    {
//        GUIStyle style = new GUIStyle();
//        style.fontSize = 20;
//        style.normal.textColor = Color.red;

//        GUI.Label(new Rect(10, 10, 300, 50), $"接地状态: {isGrounded}", style);
//        GUI.Label(new Rect(10, 40, 300, 50), $"当前速度: {rb.velocity}", style);
//    }
//}

//using UnityEngine;

//[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator)), RequireComponent(typeof(Collider))]
//public class PlayerController : MonoBehaviour
//{
//    // 移动参数
//    [Header("Movement Settings")]
//    public float walkSpeed = 5f;
//    public float sprintSpeed = 8f;
//    public float jumpForce = 12f;
//    [Range(0, 1)] public float airControl = 0.2f;

//    // 重力系统
//    [Header("Gravity Settings")]
//    public float gravityMultiplier = 2f;
//    public float baseGravity = -9.81f;
//    private Vector3 currentGravity;
//    private bool isGravityNormal = true;

//    // 颜色系统
//    [Header("Color Settings")]
//    public Material blackMat;
//    public Material whiteMat;
//    [HideInInspector] public bool isBlack = true;

//    // 动画参数
//    [Header("Animation Settings")]
//    public string runState = "RunForward";
//    public string jumpUpState = "JumpWhileRunningUp";
//    public string fallState = "FallingLoop";
//    public string sprintState = "Sprint";

//    // 组件引用
//    private Animator anim;
//    private Rigidbody rb;
//    private Renderer rend;
//    private Collider col;
//    private bool isGrounded;
//    private float lastStuckCheckTime;
//    private int groundContactCount; // 记录与地面的接触点数量

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

//        // 优化物理材质
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

//        // 重置地面状态
//        if (groundContactCount > 0)
//        {
//            isGrounded = true;
//            groundContactCount = 0; // 重置计数器
//        }
//        else
//        {
//            isGrounded = false;
//        }

//        // 输出接地状态
//        Debug.Log($"接地状态: {isGrounded}");
//    }

//    void HandleInput()
//    {
//        // 颜色切换
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            ToggleColor();
//        }

//        // 跳跃
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            TryJump();
//        }

//        // 重力反转
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            ReverseGravity();
//        }
//    }

//    void UpdateAnimation()
//    {
//        float speed = Mathf.Abs(rb.velocity.x);

//        Debug.Log($"当前速度：{rb.velocity}");

//        // 跳跃/下落状态
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
//            // 移动状态
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

//        // 物理移动
//        float speedDifference = targetSpeed - rb.velocity.x;
//        rb.AddForce(Vector3.right * speedDifference * controlFactor * 10f);

//        // 速度限制
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
//        // 检查是否站在平台上
//        if (IsValidPlatform(collision.gameObject))
//        {
//            // 检查接触点是否在玩家下方(对于正常重力)或上方(对于反转重力)
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
//        // 处理持续碰撞的情况
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

//        // 检查平台颜色是否匹配
//        Platform platformComponent = platform.GetComponent<Platform>();
//        if (platformComponent != null)
//        {
//            return platformComponent.isBlackPlatform == isBlack;
//        }

//        // 如果没有Platform组件，则默认为有效平台
//        return true;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("DeathZone"))
//        {
//            Debug.Log("进入死亡区域，游戏结束！");

//#if UNITY_EDITOR
//            //UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
//        }
//    }
//}
