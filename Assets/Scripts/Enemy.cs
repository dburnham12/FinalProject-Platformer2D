using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;          // horizontal speed
    public float moveDistance = 1f;   // patrol distance from start position

    private Rigidbody2D rb;
    private float leftBoundary;
    private float rightBoundary;
    private int direction = 1;        // 1 = right, -1 = left

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float startX = transform.position.x;
        leftBoundary = startX - moveDistance;
        rightBoundary = startX + moveDistance;
    }

    void Update()
    {
        // Move horizontally only
        rb.linearVelocity = new Vector2(direction * speed, 0f);
        // Flip sprite based on direction
        transform.localScale = new Vector3(-direction, 1, 1);

        // Flip direction at boundaries
        if (direction > 0 && transform.position.x >= rightBoundary)
        {
            transform.position = new Vector2(rightBoundary, transform.position.y);
            direction = -1;
        }
        else if (direction < 0 && transform.position.x <= leftBoundary)
        {
            transform.position = new Vector2(leftBoundary, transform.position.y);
            direction = 1;
        }
    }
}
