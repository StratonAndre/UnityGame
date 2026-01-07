using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player stats")]
    private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchHeight;
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

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip sprintClip;

    [Header("Other")]
    private Vector3 velocity;
    private static float gravity = -9.81f;
    //private bool isGrounded;

    private CharacterController characterController;
    [SerializeField] private LayerMask collisionLayer;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

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
        bool isGrounded = Physics.CheckBox(transform.position, new Vector3(0.3f, 0.05f, 0.3f), transform.rotation, collisionLayer);
        if (isGrounded == true && velocity.y < 0f) { velocity.y = 0f; }
        else { velocity.y += gravity * Time.deltaTime; }

        Vector3 topPosition = transform.position + new Vector3(0f, characterController.height * transform.localScale.y, 0f);
        bool hitCeiling = Physics.CheckBox(topPosition, new Vector3(0.3f, 0.05f, 0.3f), transform.rotation, collisionLayer);
        if (hitCeiling == true && velocity.y > 0f) { velocity.y = 0f; }

        characterController.Move(velocity * Time.deltaTime);

        // Camera
        cameraTransform.position = transform.position + new Vector3(0f, 1.75f * transform.localScale.y, 0f);
        cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, transform.eulerAngles.y, 0f);

        // Movement
        speed = normalSpeed;

        if (crouchInput == true) { Crouch(); }
        else { Uncrouch(); }

        if (sprintInput == true) { Sprint(); }

        if (jumpInput == true)
        {
            if (isGrounded == true && crouchInput == false)
            {
                Jump();
            }
        }

        Movement();
        Rotation();

        // Sound
        if (movementInput != Vector2.zero)
        {
            if (sprintInput) { audioSource.clip = sprintClip; }
            else { audioSource.clip = moveClip; }

            if (audioSource.isPlaying == false) { audioSource.Play(); }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Movement()
    {
        Vector3 forwardMovement = transform.forward * movementInput.y * speed * Time.deltaTime;
        Vector3 rightMovement = transform.right * movementInput.x * speed * Time.deltaTime;
        characterController.Move(forwardMovement + rightMovement);
    }

    private void Rotation()
    {
        transform.eulerAngles += new Vector3(0f, lookInput.x, 0f) * mouseSensitivity * Time.deltaTime;

        float newAngle = cameraTransform.eulerAngles.x + lookInput.y * invertedMouseY * mouseSensitivity * Time.deltaTime;
        if (90f < newAngle && newAngle <= 180f) { newAngle = 90f; }
        else if (180f < newAngle && newAngle < 270f) { newAngle = 270f; }
        cameraTransform.eulerAngles = new Vector3(newAngle, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }

    private void Crouch()
    {
        speed = crouchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight / 2f, transform.localScale.z);
        //characterController.Move(new Vector3(0f, -1f * crouchHeight / 2f, 0f));
    }

    private void Uncrouch()
    {
        //characterController.Move(new Vector3(0f, crouchHeight / 2f, 0f));
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }

    private void Sprint()
    {
        if (crouchInput == true) { speed = crouchSprintSpeed; }
        else { speed = sprintSpeed; }
    }
}
