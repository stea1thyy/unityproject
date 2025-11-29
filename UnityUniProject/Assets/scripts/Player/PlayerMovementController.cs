using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private PlayerMovementInput _input;
    private CharacterController cc;

    [Header("Camera")]
    public Transform cameraPivot;
    public float mouseSensitivity = 2f;
    private float pitch = 0f;

    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 8f;

    [Header("Gravity")]
    public Transform planet;
    public float gravityStrength = 30f;

    private bool isGrounded;
    private float verticalVelocity = 0f;

    private void Awake()
    {
        _input = new PlayerMovementInput();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 moveInput = _input.PlayerActionMap.PlayerMovement.ReadValue<Vector2>();
        Vector2 lookInput = _input.PlayerActionMap.Look.ReadValue<Vector2>() * mouseSensitivity;

        Look(lookInput);
        GroundCheck();
        ApplyGravityAndJump();
        AlignToPlanet();
        Move(moveInput);
    }

    // CAMERA LOOK 
    private void Look(Vector2 input)
    {
        // Horizontal rotation
        transform.Rotate(Vector3.up * input.x);

        // Vertical camera rotation
        pitch -= input.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // GROUND CHECK (CAPSULE CAST) 
    private void GroundCheck()
    {
        Vector3 up = (transform.position - planet.position).normalized;
        Vector3 down = -up;

        float r = cc.radius - 0.05f;
        Vector3 center = transform.position + cc.center;

        Vector3 p1 = center + up * (cc.height * 0.5f - r);
        Vector3 p2 = center - up * (cc.height * 0.5f - r);

        // Larger distance so uneven terrain still registers grounding
        isGrounded = Physics.CapsuleCast(p1, p2, r, down, out _, 0.3f);
    }

    // GRAVITY + JUMP 
    private void ApplyGravityAndJump()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        if (isGrounded)
        {
            // Reset vertical force when landed
            verticalVelocity = -2f;

            // Jump input
            if (_input.PlayerActionMap.Jump.WasPressedThisFrame())
            {
                // Boost helps counter steep slopes
                verticalVelocity = -jumpForce * 0.7f;
            }
        }
        else
        {
            // Softer gravity so steep slopes aren't too strong
            verticalVelocity += gravityStrength * 0.6f * Time.deltaTime;
        }
    }

    // MOVEMENT 
    private void Move(Vector2 input)
    {
        Vector3 up = (transform.position - planet.position).normalized;

        // Project movement onto planet surface
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Vector3 right   = Vector3.ProjectOnPlane(transform.right,   up).normalized;

        // Basic horizontal movement
        Vector3 move = (forward * input.y + right * input.x) * moveSpeed;

        // Light slope sticking to prevent sliding
        if (isGrounded)
            move += -up * 1.5f;

        // Boost to climb out of craters
        float slopeDot = Vector3.Dot(move.normalized, -up);
        if (slopeDot > 0.4f)
            move += (-up * slopeDot * 4f);

        // Vertical gravity + jump
        Vector3 verticalMove = -up * verticalVelocity;

        cc.Move((move + verticalMove) * Time.deltaTime);
    }

    // ALIGN PLAYER TO PLANET NORMAL
    private void AlignToPlanet()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        Quaternion current = transform.rotation;
        Quaternion target = Quaternion.FromToRotation(current * Vector3.up, up) * current;

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(current, target, Time.deltaTime * 6f);
    }
}
