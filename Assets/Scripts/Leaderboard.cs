using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    
    // List to store the recorded times
    private List<float> recordedTimes = new List<float>();

    // Reference to the UI Text component to display the leaderboard
    public Text leaderboardText;

    // Default record times
    public float ThirdPlaceTime = 90f;
    public float SecondPlaceTime = 80f;
    public float FirstPlaceTime = 75f;


    // Example of adding a recorded time (this should be called when a new time is recorded)
    public void AddTime(float time)
    {
        recordedTimes.Add(time);
        UpdateLeaderboard();
    }

    public void ClearTimes()
    {
        recordedTimes.Clear();
        UpdateLeaderboard();
    }

    // Update the leaderboard display
    private void UpdateLeaderboard()
    {
        if (leaderboardText == null)
        {
            return;
        }

        recordedTimes.Sort();

        string Leaderboard = "";
        for (int i = 0; i < recordedTimes.Count; i++)
        {
            Leaderboard += $"{i + 1}.  {FormatTime(recordedTimes[i])}\n\n";
        }

        leaderboardText.text = Leaderboard;
    }

    // Convert float time to "0:00" format
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    // Set default record times
    private void Start()
    {
        AddTime(ThirdPlaceTime); // Third Place
        AddTime(SecondPlaceTime);  // Second Place
        AddTime(FirstPlaceTime);   // First Place
    }
}
