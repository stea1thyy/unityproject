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

    [Header("UI")]
    public GameObject settingsCanvas;   

    private bool settingsOpen = false;
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
        // Journal key 
        if (_input.PlayerActionMap.Journal.WasPressedThisFrame())
        {
            OreJournalUI.Instance.Open();
            return;
        }

        // Settings key M 
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleSettings();
            return;
        }

        // Block movement while any UI is open
        if (PopUpManager.IsAnyUIAcive)
            return;

        // Lock cursor only during gameplay
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

    // SETTINGS TOGGLE
    void ToggleSettings()
    {
        if (settingsCanvas == null)
            return;

        settingsOpen = !settingsOpen;
        settingsCanvas.SetActive(settingsOpen);

        PopUpManager.IsAnyUIAcive = settingsOpen;

        if (settingsOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // CAMERA LOOK
    void Look(Vector2 input)
    {
        // Yaw on player, pitch on camera
        transform.Rotate(Vector3.up * input.x);

        pitch -= input.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // GROUND CHECK
    void GroundCheck()
    {
        Vector3 up = (transform.position - planet.position).normalized;
        Vector3 down = -up;

        float radius = cc.radius - 0.05f;
        Vector3 center = transform.position + cc.center;

        Vector3 p1 = center + up * (cc.height * 0.5f - radius);
        Vector3 p2 = center - up * (cc.height * 0.5f - radius);

        isGrounded = Physics.CapsuleCast(p1, p2, radius, down, out _, 0.3f);
    }

    // GRAVITY + JUMP
    void ApplyGravityAndJump()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        if (isGrounded)
        {
            verticalVelocity = -2f;

            if (_input.PlayerActionMap.Jump.WasPressedThisFrame())
                verticalVelocity = -jumpForce * 0.7f;
        }
        else
        {
            verticalVelocity += gravityStrength * 0.6f * Time.deltaTime;
        }
    }

    // MOVEMENT
    void Move(Vector2 input)
    {
        Vector3 up = (transform.position - planet.position).normalized;

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, up).normalized;

        Vector3 move = (forward * input.y + right * input.x) * moveSpeed;

        if (isGrounded)
            move += -up * 1.5f;

        Vector3 verticalMove = -up * verticalVelocity;

        cc.Move((move + verticalMove) * Time.deltaTime);
    }

    // PLANET ALIGNMENT
    void AlignToPlanet()
    {
        Vector3 up = (transform.position - planet.position).normalized;

        Quaternion current = transform.rotation;
        Quaternion target =
            Quaternion.FromToRotation(current * Vector3.up, up) * current;

        transform.rotation = Quaternion.Slerp(current, target, Time.deltaTime * 6f);
    }
}
