using UnityEngine;

public class DamagePellet : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    [SerializeField] private float shootSpeed = -2.0f;
    private float shootTimer = 0;
    private float shootTime = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.linearVelocityX = shootSpeed;
        if(shootTimer < shootTime)
            shootTimer += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        if(collision.gameObject.tag != "Damage" && collision.gameObject.layer != 2)
            Destroy(gameObject);
    }
}
