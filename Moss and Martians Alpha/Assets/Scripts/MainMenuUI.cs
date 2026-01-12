using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;

    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("0");
    }
    public void LoadLvl1()
    {
        SceneManager.LoadScene("1");
    }
    public void LoadLvl2()
    {
        SceneManager.LoadScene("2");
    }
    public void LoadLvl3()
    {
        SceneManager.LoadScene("3");
    }
    public void LoadLvl4()
    {
        SceneManager.LoadScene("4");
    }
    public void LoadLvl5()
    {
        SceneManager.LoadScene("5");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}