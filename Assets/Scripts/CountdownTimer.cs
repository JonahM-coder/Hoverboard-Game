using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    public Text countdownText;

    //Ready Sprite variables
    public GameObject readySprite;
    private bool isReadyVisible = false;
    private float readyTimer = 0f;
    private float readyVisibilityDuration = 3f;

    //Go Sprite variables
    public GameObject goSprite;
    private bool isGoVisible = false;
    private float goTimer = 0f;
    private float goVisibilityDuration = 0.5f;

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
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }

        countdownText.text = "GO!";
        isGoVisible = true;
        yield return new WaitForSeconds(1);

    }

}
