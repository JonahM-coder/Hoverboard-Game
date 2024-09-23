using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public Leaderboard[] leaderboards; // Array to hold multiple leaderboards
    public Button previousButton; // UI Button for previous leaderboard
    public Button nextButton; // UI Button for next leaderboard

    private int currentLeaderboardIndex;

    private void Start()
    {
        currentLeaderboardIndex = 0;
        ShowCurrentLeaderboard();

        // Add listeners for button clicks
        previousButton.onClick.AddListener(ShowPreviousLeaderboard);
        nextButton.onClick.AddListener(ShowNextLeaderboard);
    }

    private void ShowPreviousLeaderboard()
    {
        currentLeaderboardIndex--;
        if (currentLeaderboardIndex < 0)
        {
            currentLeaderboardIndex = leaderboards.Length - 1;
        }
        ShowCurrentLeaderboard();
    }

    private void ShowNextLeaderboard()
    {
        currentLeaderboardIndex++;
        if (currentLeaderboardIndex >= leaderboards.Length)
        {
            currentLeaderboardIndex = 0;
        }
        ShowCurrentLeaderboard();
    }

    private void ShowCurrentLeaderboard()
    {
        for (int i = 0; i < leaderboards.Length; i++)
        {
            leaderboards[i].gameObject.SetActive(i == currentLeaderboardIndex);
        }
    }

    // Method to add time to the current active leaderboard
    public void AddTimeToLeaderboard(float time)
    {
        if (leaderboards != null && leaderboards.Length > 0)
        {
            leaderboards[currentLeaderboardIndex].AddTime(time);
        }
    }
}
