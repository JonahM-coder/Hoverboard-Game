using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HbThruster : MonoBehaviour
{

    //Particle System Objects
    public ParticleSystem accelerateThruster;
    public ParticleSystem boostThruster;

    //Boost Emission Duration
    public float boostDuration = 3.0f;
    private bool isBoosting = false;

    //Level Trigger Objects
    public GameObject killbox;
    public GameObject goal;
    public TimeLeft timer;
    public NewHbController controller;

    // Gamepad button commands
    private bool isPlayerBoosting = false;
    private bool isBraking = false;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<TimeLeft>();
        controller = FindObjectOfType<NewHbController>();

        StartCoroutine(Countdown());

        accelerateThruster.Stop();
        boostThruster.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        bool timeRunning = timer.timeIsRunning;
        float currentPower = controller.currentBoostMeter;

        isPlayerBoosting = InputSystem.GetDevice<Gamepad>().xButton.isPressed;
        isBraking = InputSystem.GetDevice<Gamepad>().bButton.isPressed;

        if (timeRunning)
        {
            if (isMoving)
            {

                if (!accelerateThruster.isPlaying)
                {
                    accelerateThruster.Play();
                }

                if (isBraking)
                {
                    accelerateThruster.Stop();
                    accelerateThruster.Clear();
                }


                if (isPlayerBoosting && !isBoosting)
                {
                    if (currentPower != 0)
                    {
                        isBoosting = true;
                        boostThruster.Play();
                        StartCoroutine(StopBoostAfterDuration());
                    }
                }

            }
        }
        else
        {
            accelerateThruster.Stop();
            boostThruster.Stop();

            accelerateThruster.Clear();
            boostThruster.Clear();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "SpeedBoost")
        {
            accelerateThruster.Stop();
            accelerateThruster.Clear();

            isBoosting = true;
            boostThruster.Play();
            StartCoroutine(StopBoostAfterDuration());
        }

        if (other.transform.tag == "Goal")
        {
            isMoving = false;
            
            accelerateThruster.Stop();
            boostThruster.Stop();

            accelerateThruster.Clear();
            boostThruster.Clear();
        }

        if (other.transform.tag == "Killbox")
        {
            isMoving = false;
            
            accelerateThruster.Stop();
            boostThruster.Stop();

            accelerateThruster.Clear();
            boostThruster.Clear();
        }
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        yield return new WaitForSeconds(0);
        isMoving = true;
    }

    private IEnumerator StopBoostAfterDuration()
    {
        yield return new WaitForSeconds(boostDuration);

        isPlayerBoosting = false;
        isBoosting = false;
        boostThruster.Stop();
        boostThruster.Clear();
        accelerateThruster.Play();
    }


}
