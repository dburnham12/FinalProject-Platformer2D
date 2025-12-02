using UnityEngine;
using TMPro;
public class Coin : MonoBehaviour
{
    [SerializeField] private int coinsToGive = 1;

    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Coins += coinsToGive;
            coinText.text = player.Coins.ToString();
            SoundManager.Instance.PlaySFX("COIN", 0.5f);
            Destroy(gameObject);
        }
    }
}
