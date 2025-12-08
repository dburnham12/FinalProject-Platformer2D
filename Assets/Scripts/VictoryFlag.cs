using UnityEngine;
using static MainMenu;
using static PlayerController;

public class VictoryFlag : MonoBehaviour
{
    [SerializeField] private GameObject container;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX("VICTORY", 1f);

            if (PlayerPrefs.HasKey(LEVEL_KEY))
            {
                if (Level + 1 > PlayerPrefs.GetInt(LEVEL_KEY))
                    PlayerPrefs.SetInt(LEVEL_KEY, Level + 1);
            }
            else
                PlayerPrefs.SetInt(LEVEL_KEY, Level + 1);

            container.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
