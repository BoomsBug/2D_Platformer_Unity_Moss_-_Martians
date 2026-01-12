using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    [Header("Runtime timer")]
    [SerializeField] private TextMeshProUGUI stageTimerText;
    [SerializeField] private TextMeshProUGUI TimeText;

    [Header("Finish UI")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private TextMeshProUGUI finalTimeText;

    [Header("Death UI")]
    [SerializeField] private GameObject deathPanel;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Optional")]
    [SerializeField] private PauseUI pauseUI;

    private static float timer;
    private bool isRunning = true;

    void Start()
    {
        Time.timeScale = 1f;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (stageTimerText != null)
            stageTimerText.gameObject.SetActive(true);

        if (TimeText != null)
            TimeText.gameObject.SetActive(true);

        isRunning = true;
        if (PlayerManager.checkpointHit == false)
        {
            timer = 0f;
        }
    }

    void Update()
    {
        if ((levelCompletePanel != null && levelCompletePanel.activeSelf) ||
            (deathPanel != null && deathPanel.activeSelf))
        {
            Time.timeScale = 0f;
            return;
        }

        if (!isRunning) return;

        timer += Time.deltaTime;

        if (stageTimerText != null)
            stageTimerText.text = FormatTime(timer);
    }

    public void FinishLevel()
    {
        if (!isRunning) return;

        isRunning = false;
        Time.timeScale = 0f;

        if (stageTimerText != null)
            stageTimerText.gameObject.SetActive(false);
        if (TimeText != null)
            TimeText.gameObject.SetActive(false);

        if (pauseUI != null)
            pauseUI.gameObject.SetActive(false);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        if (finalTimeText != null)
            finalTimeText.text = FormatTime(timer);
        
        PlayerManager.checkpointHit = false; 
        Checkpoint.checkpointHit = false;
    }

    public void PlayerDied()
    {
        if (!isRunning) return;

        isRunning = false;
        Time.timeScale = 0f;

        if (stageTimerText != null)
            stageTimerText.gameObject.SetActive(false);
        if (TimeText != null)
            TimeText.gameObject.SetActive(false);

        if (pauseUI != null)
            pauseUI.gameObject.SetActive(false);

        if (deathPanel != null)
            deathPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        PlayerManager.checkpointHit = false; 
        Checkpoint.checkpointHit = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t % 60f;
        return minutes.ToString("00") + ":" + seconds.ToString("00.00");
    }
}
