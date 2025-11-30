using UnityEngine;

public class VictoryFlag : MonoBehaviour
{
    [SerializeField] private GameObject container;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            container.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
