using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && Time.timeScale > 0f)
        {
            container.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }
}
