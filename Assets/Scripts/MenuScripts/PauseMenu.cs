using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;
     // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("mainMenu");
    }
}
