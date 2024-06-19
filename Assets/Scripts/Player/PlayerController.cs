using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController characterController;
    private CameraController _cameraController;
    private PlayerHUDManager _playerHUDManager;
    private InteractionController _interactionController;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float sprintSpeed = 7;
    public float crouchSpeed = 1f;
    public float aimSpeed = 1.5f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float decelerationRate = 10f;

    [Header("Player States")]
    public bool isIdling;
    public bool isWalking;
    public bool isRunning;
    public bool isSprinting;
    public bool isCrouching;
    public bool isAiming;
    public bool isJumping;
    public bool isLanding;
    private bool grounded;

    [Header("Sprint Settings")]
    public float sprintThreshold = 60;
    public float runThreshold = 10;
    private float sprintRegenRate = 20;
    public float sprintMax = 100f;
    public float sprintCostPerSecond = 10f;
    public float sprintAmount;
    private bool isRegenerating = false;

    [Header("Values")]
    private Vector3 currentSpeed;
    private Vector3 velocity;
    private Vector2 moveInput;

    private IEnumerator StartRegenerationAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        isRegenerating = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isWalking = true;

        if (isRegenerating) {
            isRegenerating = false;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        isWalking = false;
    }

    private void OnRunPermormed(InputAction.CallbackContext context) {
        if (!isWalking || sprintAmount <= 0) return;

        if (sprintAmount >= sprintThreshold)
        {
            isSprinting = true;
        }
        else
        {
            isRunning = true;
        }

        isRegenerating = false;
        StartCoroutine(_playerHUDManager.ShowSprintBar());
    }

    private void OnRunCanceled(InputAction.CallbackContext context) {
        if (isRunning || isSprinting) {
            isRunning = false;
            isSprinting = false;
            StartCoroutine(StartRegenerationAfterDelay());

            StartCoroutine(_playerHUDManager.HideSprintBar());
        }
    }

    private void OnFirePerformed(InputAction.CallbackContext context) {
        if (_interactionController.currentGameObject != null) {
            if (_interactionController.currentGameObject.CompareTag("DartsTarget")) {
                if (_interactionController.currentGameObject.GetComponent<DartsTarget>().currentDart == null) return;
                _interactionController.currentGameObject.GetComponent<DartsTarget>().currentDart.GetComponent<Darts>().Launch();
            }
        } 
    }

    private void OnUsePerformed(InputAction.CallbackContext context) {
        if (_interactionController.inViewObject == null) return;
        
        _interactionController.Interacte();
    }

    private void OnMouse(InputAction.CallbackContext context) {
        Vector2 mouseInput = context.ReadValue<Vector2>();

        _cameraController.OnMoveCamera(mouseInput);
    }

    private void OnEscapePerformed(InputAction.CallbackContext context) {
        GameObject gameObject = _interactionController.currentGameObject;

        if (gameObject != null) {
            if (gameObject.CompareTag("DartsTarget")) {
                gameObject.GetComponent<DartsTarget>().OnEscape();
            }

            if (gameObject.CompareTag("Wardrobe")) {
                gameObject.GetComponent<Wardrobe>().OnEscape();
            }
        }
        else {

        }
        
    }

    private void OnThrowPerformed(InputAction.CallbackContext context) {
        if (_interactionController.currentItem == null) return;

        GameObject gameObject = _interactionController.currentItem;

        if (gameObject.CompareTag("Flashlight")) {
            gameObject.GetComponent<Flashlight>().OnThrow();
            _interactionController.currentItem = null;
        }
    }

    private void OnUse2Performed(InputAction.CallbackContext context) {
        if (_interactionController.currentItem == null) return;

        GameObject gameObject = _interactionController.currentItem;

        if (gameObject.CompareTag("Flashlight")) {
            gameObject.GetComponent<Flashlight>().OnUse();
        }
    }

    private float CalculatedSpeed() {
        float targetSpeed = 0;
        if (isSprinting) {
            targetSpeed = sprintSpeed;
        }
        else if (isRunning) {
            targetSpeed = runSpeed;
        }
        else if (isWalking) {
            targetSpeed = walkSpeed;
        }
        else if (isCrouching) {
            targetSpeed = crouchSpeed;
        }
        else if (isAiming) {
            targetSpeed = aimSpeed;
        }

        return targetSpeed;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        _cameraController = GetComponentInChildren<CameraController>();
        _playerHUDManager = GetComponent<PlayerHUDManager>();
        _interactionController = GetComponentInChildren<InteractionController>();
    }

    private void Start()
    {
        var playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMoveCanceled;

        playerInput.Player.Run.started += OnRunPermormed;
        playerInput.Player.Run.canceled += OnRunCanceled;

        playerInput.Player.Crouch.started += ctx => isCrouching = true;
        playerInput.Player.Crouch.canceled += ctx => isCrouching = false;

        playerInput.Player.Jump.started += ctx => isJumping = true;
        playerInput.Player.Jump.canceled += ctx => isJumping = false;

        playerInput.Player.Aim.started += ctx => isAiming = true;
        playerInput.Player.Aim.canceled += ctx => isAiming = false;

        playerInput.Player.Fire.performed += OnFirePerformed;

        playerInput.Player.Use.performed += OnUsePerformed;

        playerInput.Player.Escape.performed += OnEscapePerformed;

        playerInput.Player.Mouse.performed += OnMouse;

        playerInput.Player.Throw.performed += OnThrowPerformed;

        playerInput.Player.Use2.performed += OnUse2Performed;

        sprintAmount = sprintMax;
    }

    private void Update()
    {
        bool wasGrounded = grounded;
        grounded = characterController.isGrounded;

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isSprinting || isRunning)
        {
            if (!isRegenerating) {
                sprintAmount -= sprintCostPerSecond * Time.deltaTime;

                if (sprintAmount <= runThreshold)
                {
                    isSprinting = false;
                    isRunning = false;
                    StartCoroutine(StartRegenerationAfterDelay());
                }
                else if (sprintAmount < sprintThreshold)
                {
                    isSprinting = false;
                    isRunning = true;
                }
                else
                {
                    isSprinting = true;
                    isRunning = false;
                }
            }    
            else {
                isRegenerating = false;
            }
        }

        if (isRegenerating && sprintAmount < sprintMax) {
            sprintAmount += sprintRegenRate * Time.deltaTime;
        }
        else {
            isRegenerating = false;
        }

        Vector3 targetMove = _cameraController.transform.forward * moveInput.y + _cameraController.transform.right * moveInput.x;
        targetMove = Vector3.ClampMagnitude(targetMove, 1f);

        targetMove = transform.TransformDirection(targetMove);

        targetMove *= CalculatedSpeed();

        currentSpeed = Vector3.Lerp(currentSpeed, targetMove, decelerationRate * Time.deltaTime);

        characterController.Move(currentSpeed * Time.deltaTime);

        if (isJumping && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        characterController.height = isCrouching ? crouchHeight : standHeight;

        if (!wasGrounded && grounded)
        {
            isLanding = true;
        }
        else
        {
            isLanding = false;
        }

        isIdling = currentSpeed.magnitude < 0.1f && grounded && !isJumping && !isLanding;
    }

}
