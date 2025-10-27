using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public string[] unpausableScenes;   // scenes where you cant pause
    private bool isPaused = false;
    private bool canPause = true;
    private float pauseCooldown = 1f;   // 1 second cooldown
    public GameObject RetryButton;
    public GameObject ControlsButton;
    public GameObject QuitButton;
    public GameObject winTextObject;    // only used so that you cant pause after you win or lose 
    public GameObject DarkTint;
    public GameObject ControlsUI;

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool unpausable = System.Array.Exists(unpausableScenes, s => s == currentScene);

        if (unpausable || winTextObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return) && canPause)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        RetryButton.SetActive(true);
        ControlsButton.SetActive(true);
        QuitButton.SetActive(true);
        DarkTint.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        RetryButton.SetActive(false);
        QuitButton.SetActive(false);
        DarkTint.SetActive(false);
        StartCoroutine(PauseCooldown());
    }

    public void OnControlsButtonPressed()
    {
        RetryButton.SetActive(false);
        ControlsButton.SetActive(false);
        QuitButton.SetActive(false);

        ControlsUI.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        // Show main menu buttons again
        RetryButton.SetActive(true);
        ControlsButton.SetActive(true);
        QuitButton.SetActive(true);

        // Hide controls UI
        ControlsUI.SetActive(false);
    }

    IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseCooldown); // uses real time, not scaled
        canPause = true;
    }
}
