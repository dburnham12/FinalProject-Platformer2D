using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : PauseMenu
{
    public void RestartButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
