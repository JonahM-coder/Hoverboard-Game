using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    public float leanSpeed = 2f;
    public float leanSmoothness = 5f;
    public float maxLeanAngle = 30f;
    //private float currentLeanAngle = 0f; **Requires Board Lean function

    [Header("Extra")]
    public float boostDuration = 3f;
    public bool isBoosting = false;
    public float boostMeterDrain = 25f;
    public float boostMeter = 10000f;
    public float currentBoostMeter;
    public float boostMeterFill = 250f;
    private float boostTimer = 0f;
    
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

    // Countdown Text
    public Text countdownText;

    private void Awake()
    {
        //Hoverboard hover & initial position setup
        hb = GetComponent<Rigidbody>();
        currentBoostMeter = boostMeter;

    }

    private void Start()
    {
        StartCoroutine(Countdown());
        currentGravity = gravity;
        isMoving = false;
        isFalling = false;

        // Store the initial rotation values for each axis (X, Y, Z)
        initialRotationX = transform.localRotation.eulerAngles.x;
        initialRotationY = transform.localRotation.eulerAngles.y;
        initialRotationZ = transform.localRotation.eulerAngles.z;
    }

    private void FixedUpdate()
    {

        currentPosition = transform.position;
        currentRotation = transform.rotation;

        //Update where the front of the hoverboard is facing
        initialRotationY = transform.localRotation.eulerAngles.y;

        // In-game hoverboard mechanics, controls, and physics
        if (isMoving && !hasReachedGoal)
        {
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
            if (Input.GetKey(KeyCode.R))
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
                if(distanceToGround < hoverHeight)
                {
                    hb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                }

            }
            else //Apply Gravity
            {

                // Increase gravity over time
                currentGravity += gravityIncreaseRate * Time.deltaTime;
                currentGravity = Mathf.Min(currentGravity, gravity * maxGravityMultiplier);

                Vector3 gravityForce = Vector3.down * currentGravity;
                hb.AddForce(gravityForce, ForceMode.Acceleration);
            }

            // Apply forward force
            if (Input.GetKey(KeyCode.W))
            {

                hb.AddForce(transform.right * forwardForce, ForceMode.Acceleration);

                // Apply turning
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    float turnInput = Input.GetAxis("Horizontal");

                    Quaternion targetRotation = Quaternion.Euler(turnInput * maxTurnAngle, 0f, 0f);
                    hb.AddTorque(Vector3.up * turnInput * turnSpeed, ForceMode.Acceleration);
                }


            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //Apply leaning
            {
                float turnInput = Input.GetAxis("Horizontal");

                Quaternion targetRotation = Quaternion.Euler(turnInput * maxTurnAngle, 0f, 0f);
                hb.AddTorque(Vector3.up * turnInput * turnSpeed, ForceMode.Acceleration);
            }

            // Apply braking force
            if (Input.GetKey(KeyCode.S))
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
            if (Input.GetKey(KeyCode.Space) && !isBoosting && currentBoostMeter != 0)
            {
                //Decreases current BoostMeter fuel
                currentBoostMeter -= boostMeterDrain;
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
        isBoosting = false;
        boostTimer = 0f;
        boostForce = 0f;
    }

    
    private void RefillBoostMeter()
    {
        if (currentBoostMeter < boostMeter)
        {
            UpdateBoostMeter(5000f);
        }
    }
    

    private void UpdateBoostMeter(float fillAmount)
    {
        if (boostMeterFill < boostMeter)
        {
            currentBoostMeter += fillAmount;
        }

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
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }

        countdownText.text = "GO!";
        countdownText.gameObject.SetActive(false);

        isMoving = true;
        countdownText.enabled = false;

    }

}

/* LEANING FUNCTION HERE
 
             else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //Apply leaning
            {
             
                // Calculate lean angle based on the input
                float targetLeanAngle = Input.GetAxis("Horizontal") * maxLeanAngle;

                // Smoothly interpolate to the target lean angle
                currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, Time.deltaTime * leanSpeed);

                // Apply the leaning effect (rotation around the X-axis)
                Quaternion leanRotation = Quaternion.Euler(-currentLeanAngle, 0f, 0f);
                transform.localRotation = leanRotation;

                // Apply turning effect (rotation around the Y-axis)
                transform.Rotate(0f, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0f, Space.Self);
            }
 
 */