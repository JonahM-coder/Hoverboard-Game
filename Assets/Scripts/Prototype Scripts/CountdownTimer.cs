using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    //Ready Sprite variables
    public GameObject readySprite;
    private bool isReadyVisible = false;
    private float readyTimer = 0f;
    private float readyVisibilityDuration = 3f;

    //Go Sprite variables
    public GameObject goSprite;
    private bool isGoVisible = false;
    private float goTimer = 0f;
    private float goVisibilityDuration = 0.8f;

    public GameObject countdownSprite_3sec;
    public GameObject countdownSprite_2sec;
    public GameObject countdownSprite_1sec;

    // Start is called before the first frame update
    void Start()
    {
        readySprite.SetActive(true);
        goSprite.SetActive(false);
        StartCoroutine(Countdown());
    }

    private void Update()
    {
        //Update Ready time
        if (isReadyVisible)
        {
            readyTimer += Time.deltaTime;
            if (readyTimer >= readyVisibilityDuration)
            {
                readyTimer = 0f;
                isReadyVisible = false;
                readySprite.SetActive(false);
            }
        }

        //Update Go time
        if (isGoVisible)
        {
            goTimer += Time.deltaTime;
            if (goTimer >= goVisibilityDuration)
            {
                goTimer = 0f;
                isGoVisible = false;
                goSprite.SetActive(false);
            }
        }

    }

    IEnumerator Countdown()
    {
        int count = 3;

        while (count > 0)
        {
            if (count < 4)
            {
                countdownSprite_3sec.SetActive(true);
                countdownSprite_2sec.SetActive(false);
                countdownSprite_1sec.SetActive(false);
            }

            if (count < 3)
            {
                countdownSprite_3sec.SetActive(false);
                countdownSprite_2sec.SetActive(true);
                countdownSprite_1sec.SetActive(false);
            }

            if (count < 2)
            {
                countdownSprite_3sec.SetActive(false);
                countdownSprite_2sec.SetActive(false);
                countdownSprite_1sec.SetActive(true);
            }

            yield return new WaitForSeconds(1);
            count--;
        }

        readySprite.SetActive(false);
        countdownSprite_1sec.SetActive(false);

        goSprite.SetActive(true);
        isGoVisible = true;
        yield return new WaitForSeconds(1);

    }

}
