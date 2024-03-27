using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cinemachine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject characterGO;
    [SerializeField] private Rigidbody charRigidbody;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private CinemachineVirtualCamera cinemachine;

    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector2 lookDirection;
    private float xRotation;
    private float yRotation;

    [Header("Character Settings")]

    [SerializeField] float characterSpeed = 10;
    [SerializeField] float horizontalSensitivity = 1;
    [SerializeField] float verticalSensitivity = -1;
    [SerializeField] float clampMinValue = -90;
    [SerializeField] float clampMaxValue = 90;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallMultiplier;
    [SerializeField] float jumpTimeout = 0.5f;
    [SerializeField] float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask jumpingLayer;

    [Header("Animation")]
    [SerializeField] public Animator ratAnimator;
    [SerializeField] float jumpTimeoutCurrent;
    [SerializeField] bool isMoving;
    [SerializeField] bool isRunning;
    [SerializeField] bool isJumping;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isFalling;
    [SerializeField] public bool isHurt;
    [SerializeField] public bool pause = false;
    static readonly int Speed = Animator.StringToHash("speed");
    static readonly int IsMoving = Animator.StringToHash("isMoving");
    static readonly int IsJump = Animator.StringToHash("isJump");
    static readonly int IsFall = Animator.StringToHash("isFall");
    static readonly int IsHurt = Animator.StringToHash("isHurt");

    [Header("Rooms")]
    [SerializeField] bool bathroom;
    [SerializeField] bool bedroom;
    [SerializeField] bool kitchen;
    [SerializeField] bool livingRoom;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();
        RegisterInputs();
        CursorSettings(false, CursorLockMode.Locked);
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
    void RegisterInputs()
    {
        playerInput.Player.Move.performed += WASD => moveInput = WASD.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += WASD => moveInput = Vector2.zero;
        playerInput.Player.Look.performed += LookInput;
        playerInput.Player.Jump.performed += Space => Space.ReadValueAsButton();
        playerInput.Player.Run.performed += Shift => Shift.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        //CursorSettings(false, CursorLockMode.Locked);
        PlayerLook();
        ratAnimator.SetBool(IsHurt, isHurt);
    }

    public void PlayerMove()
    {
        moveDirection = moveInput.x * transform.right + moveInput.y * transform.forward + transform.up * charRigidbody.velocity.y;
        ratAnimator.SetFloat(Speed, moveInput.magnitude);
        if (moveInput.magnitude > 0)
        {
            if (isRunning)
            {
                ratAnimator.SetFloat(Speed, 2);
            }
            else
            {
                ratAnimator.SetFloat(Speed, 1);
            }
            isMoving = true;
            charRigidbody.velocity = new Vector3(moveDirection.x * characterSpeed, moveDirection.y, moveDirection.z * characterSpeed);
        }
        else { isMoving = false; }

        if (isJumping && isGrounded && jumpTimeoutCurrent <= 0f)
        {
            jumpTimeoutCurrent = jumpTimeout;
            charRigidbody.AddForce(Vector3.up*0.1f, ForceMode.Impulse);
            charRigidbody.velocity = new Vector3(moveDirection.x * characterSpeed, jumpMultiplier * Vector3.up.y, moveDirection.z * characterSpeed);
            //transform.DOMove(transform.position + Vector3.up * jumpMultiplier + charRigidbody.velocity * jumpTimeout, jumpTimeout);
        }

        if (isFalling)
        {
            isJumping = false;
            charRigidbody.mass = 0.5f;
            charRigidbody.velocity = new Vector3(moveDirection.x * characterSpeed, fallMultiplier * Vector3.down.y, moveDirection.z * characterSpeed);
        }
        ratAnimator.SetBool(IsMoving, isMoving);
        ratAnimator.SetBool(IsJump, isJumping);
        ratAnimator.SetBool(IsFall, isFalling);
    }
    public void LookInput(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>().normalized;
    }
    public void PlayerLook()
    {
        //xRotation += lookDirection.y * verticalSensitivity;
        yRotation += lookDirection.x * horizontalSensitivity;
        //xRotation = Mathf.Clamp(xRotation, clampMinValue, clampMaxValue);
        this.transform.rotation = Quaternion.Euler(0, yRotation, 0).normalized;
        //cameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0).normalized;
    }
    public void RunInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;
            characterSpeed = 4;
        }
        else
        {
            isRunning = false;
            characterSpeed = 2;
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }
    public void GroundCheck()
    {
        Color rayColor;
        if (Physics.OverlapSphere(transform.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Collide).Length > 0)
        {
            rayColor = Color.green;
            isGrounded = true;
            isFalling = false;
            ratAnimator.SetBool(IsFall, false);
            charRigidbody.mass = 1;
        }
        else
        {
            rayColor = Color.red;
            isGrounded = false;
        }
        Debug.DrawRay(transform.position, -transform.up, rayColor);
        if (jumpTimeoutCurrent > 0)
        {
            jumpTimeoutCurrent -= Time.deltaTime;
        }

        if (charRigidbody.velocity.y < -0.0001f && jumpTimeoutCurrent < 0 && !isGrounded)
        {
            isFalling = true;
        }
    }
    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pause = !pause;
            GameUIManager.Instance.PauseMenu(pause);
            CursorSettings(true, CursorLockMode.Confined);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 12)
        {
            TaskManager.Instance.SetBathroom();
        }
        if (other.gameObject.layer == 13)
        {
            TaskManager.Instance.SetBedroom();
        }
        if (other.gameObject.layer == 14)
        {
            TaskManager.Instance.SetKitchen();
        }
        if (other.gameObject.layer == 15)
        {
            TaskManager.Instance.SetLivingRoom();
        }
    }
    public void CursorSettings(bool cursorVisibility, CursorLockMode cursorLockMode)
    {
        Cursor.visible = cursorVisibility;
        Cursor.lockState = cursorLockMode;
    }
}
