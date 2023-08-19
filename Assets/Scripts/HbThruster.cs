using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HbThruster : MonoBehaviour
{

    //Particle System Objects
    public ParticleSystem defaultThruster;
    public ParticleSystem accelerateThruster;
    public ParticleSystem boostThruster;

    //Boost Emission Duration
    public float boostDuration = 3.0f;
    private bool isBoosting = false;

    //Level Trigger Objects
    public GameObject killbox;
    public GameObject goal;
    public TimeLeft timer;
    public HbController controller;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<TimeLeft>();
        controller = FindObjectOfType<HbController>();

        StartCoroutine(Countdown());

        defaultThruster.Stop();
        accelerateThruster.Stop();
        boostThruster.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        bool timeRunning = timer.timeIsRunning;
        float currentPower = controller.currentBoostMeter;

        if(timeRunning)
        {
            if (isMoving)
            {

                if (Input.GetKey(KeyCode.W))
                {
                    if (!accelerateThruster.isPlaying)
                    {
                        accelerateThruster.Play();
                    }
                }
                else
                {
                    accelerateThruster.Stop();
                    accelerateThruster.Clear();
                }
                
                if (defaultThruster.isPlaying)
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        defaultThruster.Stop();
                        defaultThruster.Clear();

                        accelerateThruster.Stop();
                        accelerateThruster.Clear();
                    }
                }
                else
                {
                    defaultThruster.Play();
                }

                if (Input.GetKey(KeyCode.Space) && !isBoosting)
                {
                    if (currentPower != 0)
                    {
                        accelerateThruster.Stop();
                        accelerateThruster.Clear();

                        isBoosting = true;
                        boostThruster.Play();
                        StartCoroutine(StopBoostAfterDuration());
                    }
                }

            }
        }
        else
        {
            defaultThruster.Stop();
            accelerateThruster.Stop();
            boostThruster.Stop();

            defaultThruster.Clear();
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
            
            defaultThruster.Stop();
            accelerateThruster.Stop();
            boostThruster.Stop();

            defaultThruster.Clear();
            accelerateThruster.Clear();
            boostThruster.Clear();
        }

        if (other.transform.tag == "Killbox")
        {
            isMoving = false;
            
            defaultThruster.Stop();
            accelerateThruster.Stop();
            boostThruster.Stop();

            defaultThruster.Clear();
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
        defaultThruster.Play();


    }

    private IEnumerator StopBoostAfterDuration()
    {
        yield return new WaitForSeconds(boostDuration);

        isBoosting = false;
        boostThruster.Stop();
        boostThruster.Clear();
        accelerateThruster.Play();
    }


}
