using UnityEngine;

public class TextBounce : MonoBehaviour
{
    private Vector2 initialPos;
    private Vector2 endPos;
    private float cycleDistance = 10f;
    private float cycleTimer = 0;
    private float cycleTime = .5f;
    private int direction = 1;
    void Start()
    {
        initialPos = gameObject.transform.position;
        endPos = new Vector2(transform.position.x, transform.position.y + cycleDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if(cycleTimer < cycleTime)
        {
            cycleTimer += Time.deltaTime;
            transform.position = direction == 1 ? 
                Vector2.Lerp(initialPos, endPos, cycleTimer / cycleTime) : 
                Vector2.Lerp(endPos, initialPos, cycleTimer / cycleTime);
        }
        else
        {
            direction = -direction;
            cycleTimer = 0;
        }
    }
}
