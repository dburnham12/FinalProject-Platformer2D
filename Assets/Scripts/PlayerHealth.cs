using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    // Starting health value for the Player
    [SerializeField] private int health = 100;
    [SerializeField] private Image healthImage; //Reference to Health Bar

    // Amount of damage the Player takes when hit
    [SerializeField] private int damageAmount = 25;

    [SerializeField] private float deathDepth = -5f;

    // Reference to the Player's SpriteRenderer (used for flashing red)
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get the SpriteRenderer component attached to the Player
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthBar();      //Update Health Bar at start
    }

    //Update Health Bar function
    private void UpdateHealthBar()
    {
        if (healthImage != null)
            healthImage.fillAmount = health / 100f;
    }

    private void Update()
    {
        if(transform.position.y < deathDepth)
            Die();
    }

    // Reload the scene when the Player dies
    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to reduce health when damage is taken
    public void TakeDamage()
    {
        health -= damageAmount; // subtract damage amount
        UpdateHealthBar();      //Update Health Bar each frame
        SoundManager.Instance.PlaySFX("HIT", 1f); // Play the hit sound
        StartCoroutine(BlinkRed()); // briefly flash red

        // If health reaches zero or below, call Die()
        if (health <= 0)
            Die();
    }

    // Coroutine to flash the Player red for 0.1 seconds
    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
