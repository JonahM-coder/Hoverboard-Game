using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class NewHbController : MonoBehaviour
{

    [Header("Overview")]
    private Rigidbody hb;
    private Quaternion currentRotation;
    private Vector3 currentPosition;
    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];
    public float multiplier = 2.4f;
    public float maxRaycastDistance = 5f;

    [Header("Hover Physics")]
    public float hoverHeight = 5f;
    public float hoverForce = 10f;
    public float resetOrientationThreshold = 45f;
    public float gravity = 9.81f;
    private float currentGravity;
    public float maxGravityMultiplier = 2f; // Maximum gravity multiplier to prevent excessive increase
    public float gravityIncreaseRate = 1f; // The rate at which gravity increases (adjust as needed)

    [Header("Speed Stats")]
    public float forwardForce = 50f; // Top speed
    public float decelerationRate = 1f;
    public float acceleration = 5f;
    public float brakingForce = 20f;

    [Header("Boost Stats")]
    public float boostForceStart = 25f;
    public float boostForce = 35000f;
    public float boostDuration = 3f;
    public float boostMeterDrain = 2000f;
    public float boostMeter = 10000f;
    public float currentBoostMeter = 0f;
    public float boostMeterFill = 500f;

    [Header("Handling Stats")]
    public float turnTorque = 65f;
    public float strafeSpeed = 12f;
    public float strafeAcceleration = 2f;
    public float strafeDeceleration = 4f;
    public float maxStrafeSpeed = 5f;

    [Header("Flight Stats")]
    public float pitchTorque = 30f;
    public float minPitchAngle = -45f; // Adjust as needed
    public float maxPitchAngle = 45f;  // Adjust as needed
    public float airSpeedMultiplier = 2f;
    public float airGravityMultiplier = 2f;

    [Header("Balance Stats")]
    public float jumpForce = 300f;
    public float angularDrag = 1f;
    private float rotationThreshold = 90f;

    [Header("Timer objects")]
    public TimeLeft timeLeftScript;
    public Timer timerScript;

    [Header("Extra")]   
    // Respawn & Checkpoint variables
    public int currentCheckpoint = 0;
    private Transform playerTransform;
    private Checkpoint previousCheckpoint;

    private float currentStrafeSpeed = 0f;
    private float boostTimer = 0f;
    private bool isBoosting = false;

    //Rotation variables
    private float initialRotationX;
    private float initialRotationY;
    private float initialRotationZ;

    // Boost Ring object
    public GameObject boostPad;

    // Layer variables
    public LayerMask groundLayer;
    public LayerMask boostLayer;
    public LayerMask speedBoostLayer;

    // Cutscene variables
    public bool isMoving;
    private Vector3 targetPoint;
    public float stopPointSpeed = 20f;

    // Boostmeter Bar GameObject
    public BoostmeterBar boostMeterBar;

    // Groundcheck 
    public HbGroundCheck groundCheck;

    // Gamepad button commands
    private bool isJumping = false;
    private bool isRefreshing = false;
    private bool isPlayerBoosting = false;
    private bool isStrafingLeft = false;
    private bool isStrafingRight = false;
    private bool isBraking = false;

    // Gate Trigger Script
    public GateTrigger gateTrigger;

    private void Awake()
    {
        //Hoverboard hover & initial position setup
        hb = GetComponent<Rigidbody>();
        hb.angularDrag = angularDrag;
        hb.freezeRotation = false; //Disable rotation

        //Checkpoint system startup
        playerTransform = transform;
        Checkpoint respawnPoint = FindObjectOfType<Checkpoint>();
        if (respawnPoint != null)
        {
            respawnPoint.SetRespawnPosition();
        }

        //Set up start position
        isMoving = false;
        StartCoroutine(Countdown());

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

        //Restricts hovering (USE ONLY FOR PLAYTESTING)
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        //Update where the front of the hoverboard is facing
        initialRotationY = transform.localRotation.eulerAngles.y;

        // In-game hoverboard mechanics, controls, and physics
        if (isMoving)
        {

            //Gamepad Stick and Button Commands are active
            Vector2 moveInput = InputSystem.GetDevice<Gamepad>().leftStick.ReadValue();
            isJumping = InputSystem.GetDevice<Gamepad>().aButton.isPressed;
            isPlayerBoosting = InputSystem.GetDevice<Gamepad>().xButton.isPressed;
            isRefreshing = InputSystem.GetDevice<Gamepad>().yButton.isPressed;
            isBraking = InputSystem.GetDevice<Gamepad>().bButton.isPressed;
            isStrafingLeft = InputSystem.GetDevice<Gamepad>().leftShoulder.isPressed;
            isStrafingRight = InputSystem.GetDevice<Gamepad>().rightShoulder.isPressed;

            // Separate horizontal and vertical movement from Gamepad controls
            Vector3 horizontalMovement = transform.up * moveInput.y * moveInput.magnitude * Time.deltaTime;
            Vector3 verticalMovement = transform.right * moveInput.x * moveInput.magnitude * Time.deltaTime;

            Vector3 rotation = hb.rotation.eulerAngles;

            // Normalize rotation angles to be within range [-180, 180]
            float xRotation = Mathf.Abs(NormalizeAngle(rotation.x));
            float zRotation = Mathf.Abs(NormalizeAngle(rotation.z));

            // Check if rotation on X or Z axis exceeds the threshold
            if (xRotation >= rotationThreshold || zRotation >= rotationThreshold)
            {
                Debug.Log("Exceeded rotation threshold!");
                Respawn();
            }

            if (groundCheck.IsGrounded)
            {
                ApplyHovering(moveInput, maxRaycastDistance);

                ApplyBrakingOrAcceleration(moveInput);

                ApplyJump(moveInput);

                ApplyHorizontalMovement(moveInput);
            }
            else // Apply gravity when hoverboard is not grounded
            {
                ApplyAirboreMovement(moveInput, airSpeedMultiplier, airGravityMultiplier);

                ApplyGravity();

                ApplyHorizontalMovement(moveInput);
            }

            if (!isStrafingLeft && !isStrafingRight)
            {
                StopStrafe();
            }

            // Apply boost meter capacity
            if (currentBoostMeter < 0)
            {
                currentBoostMeter = 0;
            }
            else if (currentBoostMeter > boostMeter)
            {
                currentBoostMeter = boostMeter;
            }

            int totalBoost = (int)currentBoostMeter;

            // Respawn back to checkpoint
            if (isRefreshing)
            {
                hb.velocity = Vector3.zero;
                StopBoost();
                Respawn();
            }


            // Boost input
            if (isPlayerBoosting && !isBoosting && currentBoostMeter != 0)
            {
                //Decreases current BoostMeter fuel
                currentBoostMeter -= boostMeterDrain;
                boostMeterBar.SetMeter(currentBoostMeter);

                //Changes the current Player BoostMeter display
                UpdateBoostMeter(Mathf.Clamp01(boostTimer / boostDuration));

                StartBoost();
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
    }

    private void ApplyHovering(Vector2 moveInput, float maxRaycastDistance)
    {
        // Declare hit variable outside of the if block
        RaycastHit hit;

        int groundedAnchorsCount = 0;

        List<Transform> groundedAnchors = new List<Transform>();

        // Hovering
        Ray ray = new Ray(transform.position, transform.up);
        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(anchors[i].position, -anchors[i].up, out hit, maxRaycastDistance, groundLayer))
            {
                ApplyForce(anchors[i], hit); // Apply force for each anchor and its corresponding hit
                groundedAnchorsCount++;
                groundedAnchors.Add(anchors[i]);
            }
        }

        if (groundedAnchorsCount >= 2)
        {
            // Check if the ray intersects with the ground layer
            if (Physics.Raycast(transform.position, Vector3.up, out hit, maxRaycastDistance, groundLayer))
            {
                Debug.DrawRay(transform.position, Vector3.up * maxRaycastDistance, Color.red);
                float distanceToGround = hit.distance;

                Vector3 groundPosition = hit.point + Vector3.up * hoverHeight;
                transform.position = groundPosition;

                // Only allow front/back movement if the hoverboard is not close to the ground
                if (distanceToGround > hoverHeight)
                {
                    // Calculate the pitch angle based on the vertical movement (front/back)
                    float newPitchAngle = -moveInput.y * pitchTorque * Time.deltaTime; // Invert for consistent direction

                    // Clamp the pitch angle to stay within specified limits
                    newPitchAngle = Mathf.Clamp(newPitchAngle, minPitchAngle, maxPitchAngle);

                    // Create a rotation quaternion for pitch
                    Quaternion newPitchRotation = Quaternion.Euler(newPitchAngle, 0f, 0f);

                    // Apply the pitch rotation to the hoverboard
                    hb.MoveRotation(hb.rotation * newPitchRotation);
                }

                // Freeze hoverboard's position and rotation when close to the ground
                hb.constraints = distanceToGround < hoverHeight ?
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation :
                    RigidbodyConstraints.FreezePositionY;

                ResetHoverForces();
                ResetRaycastState();
            }
        }
        else
        {
            if (groundedAnchorsCount > 0)
            {
                Vector3 averageGroundedAnchorPosition = Vector3.zero;
                foreach (Transform anchor in groundedAnchors)
                {
                    averageGroundedAnchorPosition += anchor.position;
                }
                averageGroundedAnchorPosition /= groundedAnchorsCount;

                transform.position = averageGroundedAnchorPosition;

                hb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                hb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }

    }

    private void ApplyHorizontalMovement(Vector2 moveInput)
    {
        // Horizontal movement
        float rotationInput = moveInput.x;
        Quaternion rotation = Quaternion.Euler(0f, rotationInput * turnTorque * Time.deltaTime, 0f);
        hb.MoveRotation(hb.rotation * rotation);

        //Apply strafing when hoverboard is grounded
        if (groundCheck.IsGrounded)
        {
            //Strafing
            if (isStrafingLeft)
            {
                StrafeLeft();
            }

            if (isStrafingRight)
            {
                StrafeRight();
            }
        }

    }

    private void ApplyBrakingOrAcceleration(Vector2 moveInput)
    {

        // Check if the hoverboard is close to the ground
        RaycastHit hit;
        bool isLanding = Physics.Raycast(transform.position, Vector3.down, out hit, maxRaycastDistance, groundLayer);

        // Apply braking force if landing
        if (isLanding)
        {
            Vector3 brakingVelocity = -hb.velocity.normalized * brakingForce;
            hb.AddForce(brakingVelocity, ForceMode.Impulse);
        }
        else
        {
            // Apply autoacceleration if not landing
            Vector3 forwardForceVector = transform.right * forwardForce;
            hb.AddForce(forwardForceVector, ForceMode.Acceleration);
        }

        // Apply braking force
        if (isBraking)
        {
            Vector3 brakingVelocity = -hb.velocity.normalized * brakingForce;
            hb.AddForce(brakingVelocity, ForceMode.Impulse);
        }
        else
        {
            // Apply autoacceleration
            Vector3 forwardForceVector = transform.right * forwardForce;
            hb.AddForce(forwardForceVector, ForceMode.Acceleration);
        }
    }

    private void ApplyAirboreMovement(Vector2 moveInput, float airSpeedMultiplier, float airGravityMultiplier)
    {
        // Gravity handling
        currentGravity += gravityIncreaseRate * Time.fixedDeltaTime;
        currentGravity = Mathf.Min(currentGravity, gravity * maxGravityMultiplier);
        Vector3 gravityForce = Vector3.down * currentGravity * airGravityMultiplier;

        // Apply autoacceleration
        Vector3 forwardForceVector = transform.right * forwardForce * airSpeedMultiplier;
        hb.AddForce(forwardForceVector, ForceMode.Acceleration);

        // Invert the gravity force if the left stick moves upwards
        if (moveInput.y > 0)
        {
            gravityForce *= -1f; // Invert the force
        }

        hb.AddForce(gravityForce, ForceMode.Acceleration);

        // Pitching
        float pitchInput = -moveInput.y;
        float pitchAngle = pitchInput * pitchTorque * Time.deltaTime;
        pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);
        Quaternion pitchRotation = Quaternion.Euler(0f, 0f, pitchAngle);

        // Horizontal movement
        float rotationInput = moveInput.x;
        Quaternion rotation = Quaternion.Euler(0f, rotationInput * turnTorque * Time.deltaTime, 0f);

        // Apply rotation if there is pitch input
        if (moveInput.y != 0)
        {
            Quaternion combinedRotation = rotation * pitchRotation;
            hb.MoveRotation(hb.rotation * combinedRotation);
        }
    }

    private void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if (hit.collider != null) // Ensure hit.collider is not null to avoid potential errors
        {
            float distance = hit.point.y - anchor.position.y;
            if (distance != 0) // Avoid division by zero
            {
                float force = Mathf.Abs(1 / distance);
                hb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
            }
        }
    }

    private void ApplyJump(Vector2 moveInput)
    {
        if (groundCheck.IsGrounded && isJumping)
        {
            hb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
        else if (!groundCheck.IsGrounded)
        {
            // Apply gravitational force
            hb.AddForce(Physics.gravity * hb.mass);
        }
    }

    public void ApplyGravity()
    {
        hb.AddForce(Physics.gravity * hb.mass);
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

        if (other.transform.tag == "Respawnbox")
        {
            Respawn();
        }

        if (other.transform.tag == "Gate")
        {
            timeLeftScript.AddTime(2f);
            gateTrigger.GateSpriteAppear();
            Destroy(other.gameObject);
        }

        if (other.transform.tag == "Checkpoint")
        {
            timeLeftScript.AddTime(15f);
            gateTrigger.CheckpointSpriteAppear();
            Destroy(other.gameObject);
        }

        if (other.transform.tag == "Goal")
        {
            isMoving = false;
            timerScript.StopTimer();
            timeLeftScript.StopTimer();
            Debug.Log("Goal reached!");
        }

        if (other.transform.tag == "Killbox")
        {
            isMoving = false;
            timerScript.StopTimer();
            timeLeftScript.StopTimer();
            Debug.Log("Killbox collided!");
        }

        if (other.CompareTag("Spawn"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();

            if (checkpoint.checkpointNumber > currentCheckpoint)
            {
                if (previousCheckpoint != null)
                {
                    Destroy(previousCheckpoint.gameObject);
                }
            }
            currentCheckpoint = checkpoint.checkpointNumber;
            previousCheckpoint = checkpoint;
        }

    }

    public void Respawn()
    {
        // Find the checkpoint with the corresponding number and set the respawn position.
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        //Decreases current BoostMeter fuel
        currentBoostMeter -= boostMeterDrain * 0.5f;
        boostMeterBar.SetMeter(currentBoostMeter);
        UpdateBoostMeter(Mathf.Clamp01(boostTimer / boostDuration));

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.checkpointNumber == currentCheckpoint)
            {
                // Set the respawn position to the checkpoint's position and rotation
                playerTransform.position = checkpoint.transform.position;
                playerTransform.rotation = Quaternion.Euler(0, checkpoint.transform.rotation.eulerAngles.y, 0);

                ApplyBrakingOrAcceleration(Vector2.zero);

                break;
            }
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

    private void ResetHoverForces()
    {
        // Reset hover forces
        currentGravity = gravity;
        hb.velocity = Vector3.zero;
        hb.angularVelocity = Vector3.zero;
    }

    private void ResetRaycastState()
    {
        // Reset anchor positions
        for (int i = 0; i < anchors.Length; i++)
        {
            // Reset each anchor's position relative to the hoverboard's position and orientation
            Vector3 anchorLocalPosition = Quaternion.Euler(hb.rotation.eulerAngles) * (anchors[i].localPosition * 1.5f);
            anchors[i].position = hb.position + anchorLocalPosition;
        }
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

    public void RefillBoostMeter()
    {
        float refillAmount = boostMeter - currentBoostMeter; // Calculate the amount needed to reach maximum
        if (refillAmount > 0)
        {
            UpdateBoostMeter(refillAmount); // Update the boost meter with the refill amount
            boostMeterBar.SetMeter(boostMeter); // Set the boost meter UI to the maximum value
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
        StartStrafe(-maxStrafeSpeed);

        // Strafe the hoverboard to the left
        Vector3 strafeDirection = transform.forward * strafeSpeed * Time.fixedDeltaTime;
        hb.MovePosition(hb.position + strafeDirection);
    }

    private void StrafeRight()
    {
        StartStrafe(maxStrafeSpeed);

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

        targetPoint = stopPoint + Vector3.right * stopPointSpeed;
    }

    public float GetSpeed()
    {
        return hb.velocity.magnitude;
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
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