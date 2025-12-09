using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchSprintSpeed;

    [SerializeField] private float jumpForce;

    [Header("Camera")]

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float invertedMouseY;

    [Header("Input")]

    [SerializeField] private InputActionReference movementInputAction;
    [SerializeField] private InputActionReference lookInputAction;
    [SerializeField] private InputActionReference jumpInputAction;
    [SerializeField] private InputActionReference sprintInputAction;
    [SerializeField] private InputActionReference crouchInputAction;

    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool sprintInput;
    private bool crouchInput;

    [Header("Other")]
    private Vector3 velocity;

    private static float gravity = -9.81f;
    //private bool isGrounded;

    private CharacterController characterController;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Input
        movementInput = movementInputAction.action.ReadValue<Vector2>();
        lookInput = lookInputAction.action.ReadValue<Vector2>();
        jumpInput = jumpInputAction.action.WasPerformedThisFrame();
        sprintInput = sprintInputAction.action.IsPressed();
        crouchInput = crouchInputAction.action.IsPressed();

        // Physics
        bool isGrounded = Physics.CheckSphere(transform.position, 0.05f);
        if (isGrounded == true && velocity.y < 0f) { velocity.y = 0f; }
        else { velocity.y += gravity * Time.deltaTime; }

        bool hitCeiling = Physics.CheckSphere(transform.position + new Vector3( 0f, characterController.height * transform.localScale.y, 0f), 0.05f);
        if (hitCeiling == true && velocity.y > 0f) { velocity.y = 0f; }

        characterController.Move(velocity * Time.deltaTime);

        // Movement
        float speed = normalSpeed;

        if (crouchInput == true)
        {
            speed = crouchSpeed;
            transform.localScale = new Vector3(1f, 0.6f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        if (sprintInput == true)
        {
            speed = (crouchInput == true) ? crouchSprintSpeed : sprintSpeed;
        }

        if (jumpInput == true)
        {
            if (isGrounded == true && crouchInput == false)
            {
                velocity.y = jumpForce;
            }
        }

        Vector3 forwardMovement = transform.forward * movementInput.y * speed * Time.deltaTime;
        Vector3 rightMovement = transform.right * movementInput.x * speed * Time.deltaTime;
        characterController.Move(forwardMovement + rightMovement);

        transform.eulerAngles += new Vector3(0f, lookInput.x, 0f) * mouseSensitivity * Time.deltaTime;
        cameraTransform.eulerAngles += new Vector3(lookInput.y, 0f, 0f) * invertedMouseY * mouseSensitivity * Time.deltaTime;
    }

    private void Jump()
    {
        velocity = new Vector3(0f, jumpForce, 0f);
    }
}
