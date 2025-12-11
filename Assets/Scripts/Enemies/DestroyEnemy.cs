using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    [SerializeField] private GameObject destroyedObject;
    private float upwardsForce = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!transform.parent)
            Debug.Log($"{transform.name} must have a parent");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            float playerBottom = collision.GetComponent<BoxCollider2D>().bounds.min.y;
            float enemyTop = transform.parent.GetComponent<BoxCollider2D>().bounds.max.y;
            Debug.Log(playerBottom);
            Debug.Log(enemyTop);
            Debug.Log(enemyTop < playerBottom);
            if (playerBottom > enemyTop - 0.05f)
            {

                SoundManager.Instance.PlaySFX("ENEMYDEATH", 0.5f);
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 0;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(upwardsForce * Vector2.up, ForceMode2D.Impulse); // add a force in an upwards direction
                Instantiate(destroyedObject, transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
