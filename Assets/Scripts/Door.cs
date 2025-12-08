using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool keyCollected = false;
    private SpriteRenderer spriteRenderer;
    private KeyUI keyUI;

    private void Start()
    {
        keyUI = FindFirstObjectByType<KeyUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void CollectKey()
    {
        keyCollected = true;
    }

    private void Update()
    {
        if (!spriteRenderer.enabled)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && keyCollected)
        {
            keyUI.RemoveKey(spriteRenderer.color);
            GetComponent<BoxCollider2D>().enabled = false;
            SoundManager.Instance.PlaySFX("DOOR", 0.5f);
            GetComponent<Animator>().Play("Door");
        }
    }
}
