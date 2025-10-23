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
    public GameObject QuitButton;
    public GameObject winTextObject;
    public GameObject DarkTint;

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
        RetryButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
        DarkTint.gameObject.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        RetryButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        DarkTint.gameObject.SetActive(false);
        StartCoroutine(PauseCooldown());
    }

    IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseCooldown); // uses real time, not scaled
        canPause = true;
    }
}
