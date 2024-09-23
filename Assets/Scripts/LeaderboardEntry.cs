using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{

    public float time;
    public GameObject image;

    public LeaderboardEntry(float t, GameObject img)
    {
        image = img;
        time = t;
    }

    // Set leaderboard entry method
    public void UpdateDisplay(float newTime, GameObject newImage)
    {
        time = newTime;
        image = newImage;
    }
}
