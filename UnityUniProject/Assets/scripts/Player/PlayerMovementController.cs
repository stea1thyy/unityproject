using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private PlayerMovementInput _input;
    private CharacterController cc;

    [Header("Camera")]
    public Transform cameraPivot;
    public float mouseSensitivity = 0.15f;
    private float pitch = 0f;

    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Planet Gravity")]
    public Transform planet;
    public float gravityStrength = 30f;
    public float jumpForce = 12f;

    // Ground check distance = (CC height / 2) + small offset
    private float groundCheckDistance;
    private bool isGroundedCustom;

    private float verticalVelocity = 0f;

    private void Awake()
    {
        _input = new PlayerMovementInput();
        cc = GetComponent<CharacterController>();

        // Based on your CC height = 2
        groundCheckDistance = (cc.height / 2f) + 0.3f;
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
        AlignToPlanetSurface();
        Move(moveInput);
    }

    // LOOK
    private void Look(Vector2 input)
    {
        transform.Rotate(Vector3.up * input.x);

        pitch -= input.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // CUSTOM GROUND CHECK (planet-based)
    private void GroundCheck()
    {
        Vector3 toPlanet = (planet.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, toPlanet);
        isGroundedCustom = Physics.Raycast(ray, groundCheckDistance);
    }

    // GRAVITY + JUMP
    private void ApplyGravityAndJump()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        if (isGroundedCustom)
        {
            // Reset downward velocity when grounded
            verticalVelocity = 0f;

            // Jump input
            if (_input.PlayerActionMap.Jump.WasPressedThisFrame())
            {
                verticalVelocity = -jumpForce; // opposite of gravity
            }
        }
        else
        {
            // Add gravity over time
            verticalVelocity += gravityStrength * Time.deltaTime;
        }
    }

    
    // MOVEMENT (tangent to planet)
    private void Move(Vector2 input)
    {
        Vector3 up = (transform.position - planet.position).normalized;

        // Tangent vectors
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Vector3 right   = Vector3.ProjectOnPlane(transform.right,   up).normalized;

        // Horizontal move
        Vector3 moveHorizontal = (forward * input.y + right * input.x) * moveSpeed;

        // Vertical move along gravity axis
        Vector3 moveVertical = -up * verticalVelocity;

        // Combine and move
        Vector3 finalMove = moveHorizontal + moveVertical;

        cc.Move(finalMove * Time.deltaTime);
    }
    
    // ALIGN PLAYER UPRIGHT
    private void AlignToPlanetSurface()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        Quaternion currentRot = transform.rotation;
        Quaternion tilt = Quaternion.FromToRotation(currentRot * Vector3.up, up);

        transform.rotation = Quaternion.Slerp(currentRot, tilt * currentRot, Time.deltaTime * 6f);
    }
}
