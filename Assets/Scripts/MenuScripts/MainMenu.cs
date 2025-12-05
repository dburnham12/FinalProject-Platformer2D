using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject levelButtonsContainer;
    [SerializeField] private TMP_Text menuSwapText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("level_1"); // later use playerPref to get level_#
    }

    public void LevelSelect(int level)
    {
        SceneManager.LoadScene($"level_{level}");
    }

    public void MenuSwap()
    {
        playButton.SetActive(!playButton.activeSelf);
        levelButtonsContainer.SetActive(!levelButtonsContainer.activeSelf);
        menuSwapText.text = playButton.activeSelf ?
            "Level Select" : "Back";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
