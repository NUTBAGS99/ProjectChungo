using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Health Settings")]
    public int health = 100;

    [Header("Camera Settings")]
    public float rotationSpeed = 150f;
    public float verticalRotationSpeed = 100f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 3f, -6f);
    public float cameraSmoothSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentVerticalAngle = 10f;
    private float currentYaw = 0f;
    private Vector3 inputDirection = Vector3.zero;
    private float gravity = -9.81f;

    private Transform cameraPivot;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Create a pivot for smoother camera movement
        cameraPivot = new GameObject("Camera Pivot").transform;
        cameraPivot.position = transform.position;
        cameraTransform.SetParent(cameraPivot);
    }

    void Update()
    {
        HandleMouseLook();
        HandleInput();
        HandleMovement();
        UpdateCamera();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        inputDirection = (forward * vertical + right * horizontal).normalized;
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = inputDirection * speed;
        controller.Move(move * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * rotationSpeed * Time.deltaTime;
        currentVerticalAngle -= mouseY * verticalRotationSpeed * Time.deltaTime;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    void UpdateCamera()
    {
        if (cameraTransform == null || cameraPivot == null) return;

        cameraPivot.position = transform.position;
        Quaternion cameraRotation = Quaternion.Euler(currentVerticalAngle, currentYaw, 0);
        Vector3 targetPosition = cameraPivot.position + cameraRotation * cameraOffset;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSmoothSpeed);
        cameraTransform.LookAt(cameraPivot.position + Vector3.up * 1.5f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Player died");
        }
    }
}
