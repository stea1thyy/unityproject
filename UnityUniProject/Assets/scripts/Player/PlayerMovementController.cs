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

    void Awake()
    {
        // Input + controller setup
        _input = new PlayerMovementInput();
        cc = GetComponent<CharacterController>();
    }

    void OnEnable() => _input.Enable();
    void OnDisable() => _input.Disable();

    void Update()
    {
        // Journal key (always allowed)
        if (_input.PlayerActionMap.Journal.WasPressedThisFrame())
        {
            OreJournalUI.Instance.Open();
            return;
        }

        // Block movement while menus are open
        if (PopUpManager.IsAnyUIAcive)
            return;

        // Lock cursor during gameplay
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector2 moveInput = _input.PlayerActionMap.PlayerMovement.ReadValue<Vector2>();
        Vector2 lookInput = _input.PlayerActionMap.Look.ReadValue<Vector2>() * mouseSensitivity;

        Look(lookInput);
        GroundCheck();
        ApplyGravityAndJump();
        AlignToPlanet();
        Move(moveInput);
    }

    void Look(Vector2 input)
    {
        // Yaw on player, pitch on camera
        transform.Rotate(Vector3.up * input.x);

        pitch -= input.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void GroundCheck()
    {
        // Planet-based up/down
        Vector3 up = (transform.position - planet.position).normalized;
        Vector3 down = -up;

        // Match CharacterController shape
        float radius = cc.radius - 0.05f;
        Vector3 center = transform.position + cc.center;

        Vector3 p1 = center + up * (cc.height * 0.5f - radius);
        Vector3 p2 = center - up * (cc.height * 0.5f - radius);

        // Small cast distance for stability
        isGrounded = Physics.CapsuleCast(p1, p2, radius, down, out _, 0.3f);
    }

    void ApplyGravityAndJump()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        if (isGrounded)
        {
            // Stick to surface
            verticalVelocity = -2f;

            // Jump away from planet
            if (_input.PlayerActionMap.Jump.WasPressedThisFrame())
                verticalVelocity = -jumpForce * 0.7f;
        }
        else
        {
            // Pull back toward planet
            verticalVelocity += gravityStrength * 0.6f * Time.deltaTime;
        }
    }

    void Move(Vector2 input)
    {
        Vector3 up = (transform.position - planet.position).normalized;

        // Keep movement tangent to the surface
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, up).normalized;

        Vector3 move = (forward * input.y + right * input.x) * moveSpeed;

        // Extra push down helps on slopes
        if (isGrounded)
            move += -up * 1.5f;

        // Vertical handled separately
        Vector3 verticalMove = -up * verticalVelocity;

        cc.Move((move + verticalMove) * Time.deltaTime);
    }

    void AlignToPlanet()
    {
        // Rotate player so feet point at planet
        Vector3 up = (transform.position - planet.position).normalized;

        Quaternion current = transform.rotation;
        Quaternion target =
            Quaternion.FromToRotation(current * Vector3.up, up) * current;

        transform.rotation = Quaternion.Slerp(current, target, Time.deltaTime * 6f);
    }
}
