using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [Header("Head Bob Settings")]
    public float walkBobSpeed = 10f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 14f;
    public float runBobAmount = 0.1f;
    public float crouchBobSpeed = 6f;
    public float crouchBobAmount = 0.03f;
    public float aimBobSpeed = 4f;
    public float aimBobAmount = 0.02f;
    public float idleBobSpeed = 2f;
    public float idleBobAmount = 0.01f;

    [Header("Transition Settings")]
    public float transitionSpeed = 10f;
    public float landingDescentAmount = 0.5f;
    public float landingDuration = 0.2f;

    private float defaultYPos;
    private float timer;
    private CharacterController characterController;
    private PlayerController playerMovement;
    private bool wasGrounded;
    private bool isLandingImpact;
    private float landingTimer = 0f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float dampingSpeed = 1.0f;
    private Vector3 initialPosition;

    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerController>();
        defaultYPos = transform.localPosition.y;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            if (wasGrounded != characterController.isGrounded)
            {
                isLandingImpact = true;
                landingTimer = 0f;
            }

            if (isLandingImpact)
            {
                float descentAmount = Mathf.Lerp(0f, landingDescentAmount, landingTimer / landingDuration);
                transform.localPosition = new Vector3(transform.localPosition.x, defaultYPos - descentAmount, transform.localPosition.z);

                landingTimer += Time.deltaTime;

                if (landingTimer >= landingDuration)
                {
                    isLandingImpact = false;
                    landingTimer = 0f;
                }
            }
            else
            {
                if (playerMovement.isRunning)
                {
                    ApplyHeadBob(runBobSpeed, runBobAmount);
                }
                else if (playerMovement.isCrouching)
                {
                    ApplyHeadBob(crouchBobSpeed, crouchBobAmount);
                }
                else if (playerMovement.isAiming)
                {
                    ApplyHeadBob(aimBobSpeed, aimBobAmount);
                }
                else if (playerMovement.isIdling)
                {
                    ApplyHeadBob(idleBobSpeed, idleBobAmount);
                }
                else
                {
                    ApplyHeadBob(walkBobSpeed, walkBobAmount);
                }
            }
        }
        else
        {
            ResetHeadPosition();
        }

        wasGrounded = characterController.isGrounded;
    }

    private void ApplyHeadBob(float bobSpeed, float bobAmount)
    {
        timer += Time.deltaTime * bobSpeed;
        float bobbingAmount = Mathf.Sin(timer) * bobAmount;
        float targetYPos = defaultYPos + bobbingAmount;

        float newYPos = Mathf.Lerp(transform.localPosition.y, targetYPos, Time.deltaTime * transitionSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, newYPos, transform.localPosition.z);
    }


    private void ResetHeadPosition()
    {
        Vector3 currentPosition = transform.localPosition;
        currentPosition.y = Mathf.Lerp(currentPosition.y, defaultYPos, Time.deltaTime * transitionSpeed);
        transform.localPosition = currentPosition;
    }

    void FixedUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
}
