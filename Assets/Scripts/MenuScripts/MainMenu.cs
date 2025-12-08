using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MainMenu : MonoBehaviour
{
    public const string LEVEL_KEY = "level";
    public const int FINAL_LEVEL = 2;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject levelButtonsContainer;
    [SerializeField] private TMP_Text menuSwapText;

    private int level;
    private Button[] levelButtons;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        level = PlayerPrefs.HasKey(LEVEL_KEY) ? PlayerPrefs.GetInt(LEVEL_KEY) : 1;

        levelButtons = levelButtonsContainer.GetComponentsInChildren<Button>();

        foreach (Button button in levelButtons)
        {
            string buttonText = Regex.Match(button.name, @"[0-9]+").Value;

            if (int.Parse(buttonText) <= level)
            {
                button.enabled = true;
                button.GetComponentInChildren<TMP_Text>().text = buttonText;
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene($"level_{level}");
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
