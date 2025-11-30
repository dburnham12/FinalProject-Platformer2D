using System.Collections;
using UnityEngine;

public class TakingDamage : MonoBehaviour
{
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private float knockbackForce = 5f;

    // Detect collision with the Player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the colliding object is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized; // direction that the knockback will be applied

            // Access the PlayerHealth script and apply damage
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();

            StartCoroutine(KnockbackCoroutine(
                direction, collision.gameObject.GetComponent<Rigidbody2D>(),
                collision.gameObject.GetComponent<PlayerController>()));
        }
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, Rigidbody2D playerRb, PlayerController playerControls)
    {
        Vector3 force = direction * knockbackForce; // Set up force for knockback
        playerRb.AddForce(force, ForceMode2D.Impulse); // add a force in a specified direction 
        playerControls.enabled = false; // temporarily disable the player controller to prevent movement and allow knockback
        yield return new WaitForSeconds(knockbackDuration); // wait for the amount of time needed to apply knockback
        playerControls.enabled = true; // re-enable player controller to allow for player to resume control
    }
}