using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject RetryButton;
    public GameObject QuitButton;
    public GameObject WinTextObject;
    public GameObject DarkTint;

    [Header("Pause Settings")]
    public string[] unpausableScenes;
    private float pauseCooldown = 0.75f;

    private bool isPaused = false;
    private bool canPause = true;

    //private void Start()
    //{
    //    RetryButton = GameObject.Find("RetryButton");
    //    QuitButton = GameObject.Find("QuitButton");
    //    DarkTint = GameObject.Find("DarkTint");
    //}


    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

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
        StartCoroutine(PauseCooldown());
    }

    private void SetPauseUI(bool state)
    {
        if (RetryButton) RetryButton.SetActive(state);
        if (QuitButton) QuitButton.SetActive(state);
        if (DarkTint) DarkTint.SetActive(state);
    }

    private IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseCooldown);
        canPause = true;
    }

    public bool IsPaused => isPaused;
}
