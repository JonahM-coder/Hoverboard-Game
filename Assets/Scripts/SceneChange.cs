using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public void ResumeScene()
    {
        Time.timeScale = 1f;
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {

        // If in the Unity Editor, stop the play mode
        UnityEditor.EditorApplication.isPlaying = false;

        // If running a built game, quit the application
        Application.Quit();
    }

}
