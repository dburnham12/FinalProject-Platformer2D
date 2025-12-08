using UnityEngine;
using UnityEngine.SceneManagement;
using static MainMenu;
using static PlayerController;

public class VictoryMenu : MonoBehaviour
{
    public void ContinueButton()
    {
        Time.timeScale = 1f;

        if (Level != FINAL_LEVEL)
            SceneManager.LoadScene($"level_{Level + 1}");
        else
            SceneManager.LoadScene("MainMenu");
    }
}
