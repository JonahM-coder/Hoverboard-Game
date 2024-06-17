using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class AudioManager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("UI Audio clips")]
    public AudioClip countdownClip;
    public AudioClip goClip;
    public AudioClip gateClip;
    public AudioClip boostRefuelClip;
    public AudioClip checkpointClip;
    public AudioClip goalClip;

    // TODO: create and import .mp3 sound files
    //public AudioClip HbAcceleration;
    //public AudioClip HbBoost;
    //public AudioClip HbStrafe;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

        // TODO: add music piece
        //musicSource.clip = background;
        //musicSource.Play();

    }


}
