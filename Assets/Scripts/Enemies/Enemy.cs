using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;          // horizontal speed
    [SerializeField] private float moveDistance = 1f;   // patrol distance from start position
    [SerializeField] private bool moveVertically = false;

    // Components
    private new Rigidbody2D rigidbody;

    private float leftBoundary;
    private float rightBoundary;

    private float topBoundary;
    private float bottomBoundary;

    private int direction = 1;        // 1 = right, -1 = left

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        float startX = transform.position.x;
        leftBoundary = startX - moveDistance;
        rightBoundary = startX + moveDistance;

        float startY = transform.position.y;
        topBoundary = startY + moveDistance;
        bottomBoundary = startY - moveDistance;
    }

    private void Update()
    {
        // Flip direction at boundaries
        if (moveVertically)
        {
            // Move vertically only
            rigidbody.linearVelocity = new Vector2(0f, direction * moveSpeed);

            if (direction > 0 && transform.position.y >= topBoundary)
            {
                transform.position = new Vector2(transform.position.x, topBoundary);
                direction = -1;
            }
            else if (direction < 0 && transform.position.y <= bottomBoundary)
            {
                transform.position = new Vector2(transform.position.x, bottomBoundary);
                direction = 1;
            }
        }
        else
        {
            // Move horizontally only
            rigidbody.linearVelocity = new Vector2(direction * moveSpeed, 0f);

            // Flip sprite based on direction
            transform.localScale = new Vector3(-direction, 1, 1);

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
}
