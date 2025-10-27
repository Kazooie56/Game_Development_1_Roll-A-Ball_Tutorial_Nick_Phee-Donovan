using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject RetryButton;
    public GameObject ControlsButton;
    public GameObject QuitButton;
    public GameObject WinTextObject;
    public GameObject DarkTint;
    public GameObject ControlsUI;
    

    [Header("Pause Settings")]
    public string[] unpausableScenes;

    private bool isPaused = false;
    private bool canPause = true;

    private void Update()
    {
        HandlePauseInput();
    }

    private void HandlePauseInput()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool unpausable = System.Array.Exists(unpausableScenes, s => s == currentScene);

        if (unpausable || (WinTextObject != null && WinTextObject.activeSelf)) return;

        if (Input.GetKeyDown(KeyCode.Return) && canPause)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        SetPauseUI(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SetPauseUI(false);

        ControlsUI.SetActive(false);

        StartCoroutine(PauseCooldown());
    }

    private void SetPauseUI(bool state)
    {
        if (RetryButton) RetryButton.SetActive(state);
        if (ControlsButton) ControlsButton.SetActive(state);
        if (QuitButton) QuitButton.SetActive(state);
        if (DarkTint) DarkTint.SetActive(state);
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

    private IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(0.25f); // this is how long you need to wait before pausing again
        canPause = true;
    }

    public bool IsPaused => isPaused;
}
