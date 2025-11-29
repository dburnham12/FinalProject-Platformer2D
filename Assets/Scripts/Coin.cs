using UnityEngine;
using TMPro;
public class Coin : MonoBehaviour
{

    public TextMeshProUGUI coinText;
    [SerializeField] private int coinsToGive = 1;

    private void Start()
    {
        coinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.coins += coinsToGive;
            coinText.text = player.coins.ToString();
            Destroy(gameObject);
        }
    }
}
