using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera camera;
    
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float maxPitch = 85f;
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = -20f;
    
    [SerializeField] private float laserRange = 60f;
    [SerializeField] private LayerMask laserLayerHits;

    [SerializeField] private GameObject missile;
    [SerializeField] private float missileOffset = 1f;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private Vector3 velocity;
    private float pitch;
    
    private Vector3 laserPoint;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        InputActionAsset asset = inputActions != null ? inputActions : InputSystem.actions;
        if (asset == null)
        {
            Debug.LogError($"{nameof(FirstPersonController)}: no InputActionAsset assigned and no project-wide actions set.", this);
            enabled = false;
            return;
        }

        moveAction = asset.FindAction("Player/Move", throwIfNotFound: true);
        lookAction = asset.FindAction("Player/Look", throwIfNotFound: true);
        jumpAction = asset.FindAction("Player/Jump", throwIfNotFound: true);
        attackAction = asset.FindAction("Player/Attack", throwIfNotFound: true);
    }

    private static void CaptureCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        if (lockCursor)
        {
            CaptureCursor();
        }
    }

    private void OnDisable() => ReleaseCursor();

    private void Update()
    {
        HandleCursorToggle();
        HandleLook();
        HandleMove();
        HandleLaser();
        HandleClick();
    }

    private void HandleCursorToggle()
    {
        if (!lockCursor) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ReleaseCursor();
        }
        else if (Cursor.lockState != CursorLockMode.Locked && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CaptureCursor();
        }
    }

    private void HandleLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked && lockCursor) return;

        Vector2 look = lookAction.ReadValue<Vector2>();
        float yawDelta = look.x * mouseSensitivity;
        float pitchDelta = look.y * mouseSensitivity;

        transform.Rotate(Vector3.up, yawDelta, Space.World);

        pitch = Mathf.Clamp(pitch - pitchDelta, -maxPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 horizontalMove = transform.right * input.x + transform.forward * input.y;
        horizontalMove.Normalize();
        horizontalMove *= moveSpeed;

        bool grounded = controller.isGrounded;
        if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
        
        if (grounded && jumpAction.WasPressedThisFrame())
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight); // Initial velocity formula: https://math.stackexchange.com/a/222585
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 motion = (horizontalMove + Vector3.up * velocity.y) * Time.deltaTime;
        controller.Move(motion);
    }

    private void HandleLaser()
    {
        var ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        laserPoint = Physics.Raycast(ray, out RaycastHit hit, laserRange, laserLayerHits.value, QueryTriggerInteraction.Ignore) ? hit.point : ray.GetPoint(laserRange);
    }

    public Vector3 GetLaserPoint()
    {
        return laserPoint;
    }

    private void HandleClick()
    {
        if (missile.activeInHierarchy || !attackAction.WasPressedThisFrame()) return;
        
        missile.SetActive(true);
        missile.transform.position = transform.position + transform.forward * missileOffset;
        missile.transform.rotation = transform.rotation;
    }

    public bool CanHit(GameObject collided)
    {
        return (laserLayerHits.value & (1 << collided.layer)) != 0;
    }

}
