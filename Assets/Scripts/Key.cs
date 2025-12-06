using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    private KeyUI keyUI;

    private void Start()
    {
        keyUI = FindFirstObjectByType<KeyUI>();
        GetComponent<SpriteRenderer>().color = door.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            keyUI.AddKey(GetComponent<SpriteRenderer>().color);
            SoundManager.Instance.PlaySFX("KEY", 0.5f);
            door.GetComponent<Door>().CollectKey();
            Destroy(gameObject);
        }
    }
}
