using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HbController : MonoBehaviour
{

    [Header("Overview")]
    Rigidbody hb;
    private Quaternion currentRotation;
    private Vector3 currentPosition;
    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];
    public float multiplier;

    [Header("Hover Physics")]
    public float hoverHeight = 5f;
    public float hoverForce = 10f;
    public float resetOrientationThreshold = 45f;
    public float gravity = 9.8f;
    private float currentGravity;
    public float maxGravityMultiplier = 2f; // Maximum gravity multiplier to prevent excessive increase
    public float gravityIncreaseRate = 1f; // The rate at which gravity increases (adjust as needed)
    public bool isFalling;

    [Header("Speed Stats")]
    public float moveForce;
    public float forwardForce = 50f;
    public float decelerationRate = 1f;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float brakingForce = 20f;

    [Header("Boost Stats")]
    public float boostForceStart = 10f;
    public float boostForce = 20f;

    [Header("Handling Stats")]
    public float turnTorque;
    public float turnSpeed = 20f;
    public float maxTurnAngle = 35f;

    [Header("Balance Stats")]
    public float angularDrag = 1f;
    public float strafeSpeed = 4f;
    public float strafeAcceleration = 2f;
    public float strafeDeceleration = 4f;
    private float currentStrafeSpeed = 0f;
    public float maxStrafeSpeed = 5f;

    [Header("Extra")]
    public float boostDuration = 3f;
    public float boostMeterDrain = 25f;
    public float boostMeter = 10000f;
    public float currentBoostMeter;
    public float boostMeterFill = 250f;
    private float boostTimer = 0f;
    private bool isBoosting = false;

    //Rotation variables
    private float initialRotationX;
    private float initialRotationY;
    private float initialRotationZ;

    // Boost Ring variables
    public Text boostText;   
    public GameObject boostPad;

    // Layer variables
    public LayerMask groundLayer;
    public LayerMask boostLayer;
    public LayerMask speedBoostLayer;

    // Cutscene variables
    private bool isMoving;
    public bool hasReachedGoal = false;
    private Vector3 targetPoint;
    public float stopPointSpeed = 20f;

    // Boostmeter Bar GameObject
    public BoostmeterBar boostMeterBar;

    // Pause Sprite variables
    public GameObject pauseSprite;

    // Gamepad button commands
    private bool isAccelerating = false;
    private bool isRefreshing = false;
    private bool isPlayerBoosting = false;
    private bool isStrafingLeft = false;
    private bool isStrafingRight = false;
    private bool isBraking = false;
    private bool isPaused = false;
    

    private void Awake()
    {
        //Hoverboard hover & initial position setup
        hb = GetComponent<Rigidbody>();
        hb.angularDrag = angularDrag;
        hb.freezeRotation = true;

    }

    private void Start()
    {

        //Set up start position
        StartCoroutine(Countdown());
        currentGravity = gravity;
        isMoving = false;
        isFalling = false;
        pauseSprite.SetActive(false);

        // Set up boost meter
        currentBoostMeter = boostMeter;
        boostMeterBar.SetMaxMeter(boostMeter);

        // Store the initial rotation values for each axis (X, Y, Z)
        initialRotationX = transform.localRotation.eulerAngles.x;
        initialRotationY = transform.localRotation.eulerAngles.y;
        initialRotationZ = transform.localRotation.eulerAngles.z;
    }

    private void FixedUpdate()
    {

        //Pause Menu
        if (InputSystem.GetDevice<Gamepad>().startButton.isPressed)
        {
            isPaused = !isPaused;
            pauseSprite.SetActive(true);
            Time.timeScale = isPaused ? 0f : 1f;
        }

        if (!isPaused)
        {
            //Disable pause menu
            pauseSprite.SetActive(false);

            currentPosition = transform.position;
            currentRotation = transform.rotation;

            //Update where the front of the hoverboard is facing
            initialRotationY = transform.localRotation.eulerAngles.y;

            // In-game hoverboard mechanics, controls, and physics
            if (isMoving && !hasReachedGoal)
            {

                //Gamepad Stick and Button Commands are active
                Vector2 moveInput = InputSystem.GetDevice<Gamepad>().leftStick.ReadValue();
                isAccelerating = InputSystem.GetDevice<Gamepad>().aButton.isPressed;
                isPlayerBoosting = InputSystem.GetDevice<Gamepad>().xButton.isPressed;
                isRefreshing = InputSystem.GetDevice<Gamepad>().yButton.isPressed;
                isBraking = InputSystem.GetDevice<Gamepad>().bButton.isPressed;
                isStrafingLeft = InputSystem.GetDevice<Gamepad>().leftShoulder.isPressed;
                isStrafingRight = InputSystem.GetDevice<Gamepad>().rightShoulder.isPressed;

                //Gamepad movement
                Vector3 movement = transform.forward * moveInput.y * moveInput * Time.deltaTime;
                hb.MovePosition(hb.position + movement);

                //Gamepad rotation
                float rotationInput = moveInput.x;
                Quaternion rotation = Quaternion.Euler(0, rotationInput * turnTorque * Time.deltaTime, 0);
                hb.MoveRotation(hb.rotation * rotation);

                //Strafing
                if (isStrafingLeft)
                {
                    StrafeLeft();
                }

                if (isStrafingRight)
                {
                    StrafeRight();
                }


                if (currentBoostMeter < 0)
                {
                    currentBoostMeter = 0;
                }
                else if (currentBoostMeter > boostMeter)
                {
                    currentBoostMeter = boostMeter;
                }

                int totalBoost = (int)currentBoostMeter;

                if (boostText != null)
                {
                    totalBoost = Mathf.Max(totalBoost, 0);
                    boostText.text = totalBoost.ToString();
                }


                // Check if the hoverboard is tilted beyond the threshold angle
                if (isRefreshing)
                {
                    StopBoost();
                    ResetOrientation();
                    return;
                }

                Ray ray = new Ray(transform.position, transform.up);
                RaycastHit hit;


                for (int i = 0; i < 4; i++)
                {
                    ApplyForce(anchors[i], hits[i]);

                }


                hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.right * acceleration);
                hb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);

                // Check if the ray intersects with the ground layer
                if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity, groundLayer))
                {
                    isFalling = false;
                    float distanceToGround = hit.distance;

                    Vector3 groundPosition = hit.point + Vector3.up * hoverHeight;
                    transform.position = groundPosition;

                    float proportionalHeight = (hoverHeight - distanceToGround) / hoverHeight;
                    Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                    hb.AddForce(appliedHoverForce, ForceMode.Acceleration);

                    hb.constraints = RigidbodyConstraints.FreezePositionY;
                    if (distanceToGround < hoverHeight)
                    {
                        hb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                    }

                }
                else //Apply Gravity
                {
                    isFalling = true;
                    // Increase gravity over time
                    currentGravity += gravityIncreaseRate * Time.fixedDeltaTime;
                    currentGravity = Mathf.Min(currentGravity, gravity * maxGravityMultiplier);

                    Vector3 gravityForce = Vector3.down * currentGravity;
                    hb.AddForce(gravityForce, ForceMode.Acceleration);
                }

                // Apply forward force
                if (isAccelerating)
                {

                    hb.AddForce(transform.right * forwardForce, ForceMode.Acceleration);

                }

                // Apply braking force
                if (isBraking)
                {
                    Vector3 brakingVelocity = -hb.velocity.normalized * brakingForce;
                    hb.AddForce(brakingVelocity, ForceMode.Impulse);
                }
                else // Apply autoacceleration
                {

                    Vector3 forwardForceVector = transform.right * forwardForce;
                    hb.AddForce(forwardForceVector, ForceMode.Acceleration);
                }


                // Speed boost input
                if (isPlayerBoosting && !isBoosting && currentBoostMeter != 0)
                {
                    //Decreases current BoostMeter fuel
                    currentBoostMeter -= boostMeterDrain;
                    boostMeterBar.SetMeter(currentBoostMeter);
                    StartBoost();

                    //Changes the current Player BoostMeter display
                    UpdateBoostMeter(Mathf.Clamp01(boostTimer / boostDuration));
                }

                // Hoverboard boost duration
                if (isBoosting)
                {

                    boostTimer += Time.fixedDeltaTime;

                    if (boostTimer >= boostDuration)
                    {
                        StopBoost();
                    }

                }

                // Apply boost force
                if (isBoosting)
                {
                    hb.AddForce(transform.right * boostForce, ForceMode.Acceleration);
                }

            } //End of player controls

            // When the player reaches the goal
            if (hasReachedGoal)
            {
                // Move the hoverboard towards the targetPoint
                float goalMoveSpeed = 5f; // Adjust this to control the movement speed
                transform.position = Vector3.Lerp(transform.position, targetPoint, goalMoveSpeed * Time.deltaTime);
            }
        }



    }

    //Extra functions

    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Battery")
        {

            RefillBoostMeter();

        }

        if (other.transform.tag == "SpeedBoost")
        {
            StartBoost();
        }
    }
    

    private void ResetOrientation()
    {
        hb.velocity = Vector3.zero;
        hb.angularVelocity = Vector3.zero;

        // Reset the hoverboard's local rotation while keeping the initial rotation values for each axis
        Vector3 newRotation = new Vector3(initialRotationX, initialRotationY, initialRotationZ);
        transform.localRotation = Quaternion.Euler(newRotation);

    }

    private void StartBoost()
    {
        // Apply boost force
        boostForce = boostForceStart;
        isBoosting = true;
        boostTimer = 0f;

    }

    private void StopBoost()
    {
        isPlayerBoosting = false;
        isBoosting = false;
        boostTimer = 0f;
        boostForce = 0f;
    }

    
    private void RefillBoostMeter()
    {
        if (currentBoostMeter < boostMeter)
        {
            UpdateBoostMeter(5000f);
            boostMeterBar.SetMeter(boostMeter);
        }
    }
    

    private void UpdateBoostMeter(float fillAmount)
    {
        if (boostMeterFill < boostMeter)
        {
            currentBoostMeter += fillAmount;
        }

    }

    private void StrafeLeft()
    {
        // Strafe the hoverboard to the left
        Vector3 strafeDirection = transform.forward * strafeSpeed * Time.fixedDeltaTime;
        hb.MovePosition(hb.position + strafeDirection);
    }

    private void StrafeRight()
    {
        // Strafe the hoverboard to the right
        Vector3 strafeDirection = -transform.forward * strafeSpeed * Time.fixedDeltaTime;
        hb.MovePosition(hb.position + strafeDirection);
    }

    private void StartStrafe(float targetSpeed)
    {
        // Gradually accelerate or decelerate to the target strafe speed
        targetSpeed = Mathf.Clamp(targetSpeed, -maxStrafeSpeed, maxStrafeSpeed);
        StartCoroutine(ChangeStrafeSpeed(targetSpeed));
    }

    private void StopStrafe()
    {
        // Gradually decelerate to zero strafe speed
        StartCoroutine(ChangeStrafeSpeed(0.0f));
    }

    private IEnumerator ChangeStrafeSpeed(float targetSpeed)
    {
        float startSpeed = currentStrafeSpeed;
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            currentStrafeSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime);
            elapsedTime += Time.deltaTime * (targetSpeed == 0.0f ? strafeDeceleration : strafeAcceleration);
            yield return null;
        }

        currentStrafeSpeed = targetSpeed;
    }

    public void StopAtPoint(Vector3 stopPoint)
    {
        // Stop the hoverboard at the given stopPoint
        hb.velocity = Vector3.zero;
        hb.angularVelocity = Vector3.zero;
        transform.position = stopPoint;
        isMoving = false;
        hasReachedGoal = true;

        targetPoint = stopPoint + Vector3.right * stopPointSpeed;
    }

    public float GetSpeed()
    {
        return hb.velocity.magnitude;
    }

    public void DeactivateForce()
    {
        isMoving = false;
        hb.velocity = Vector3.zero;
        hb.Sleep();
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        isMoving = true;
    }

}