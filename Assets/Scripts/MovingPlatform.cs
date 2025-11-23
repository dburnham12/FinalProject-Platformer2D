using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private PlatformType platformType;
    [SerializeField] private Transform[] movementNodes;
    [SerializeField] private float platformSpeed = 5f;
    [SerializeField] private float reactiveLagTimeToRespawn = 5f;

    private int targetNode = 1;
    private bool isMoving;
    private bool isReversing = false;

    // Reactive platform variables
    private bool isOnPlatform = false;
    private bool isAtSpawn = true;
    private float reactiveLagTimeCounter = 0;

    private void Awake()
    {
        if (movementNodes.Length < 2)
        {
            print($"{transform.parent.name} has less than one movement node, must have at least 2");
            enabled = false;
        }

        GameObject movementNodeOne = new GameObject("MovementNode-1");
        movementNodeOne.transform.position = transform.position;

        movementNodes[0] = movementNodeOne.transform;
        
        isMoving = platformType switch
        {
            PlatformType.Looping or PlatformType.Alternating => true,
            _ => false
        };
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
            transform.position = Vector2.MoveTowards(transform.position, movementNodes[targetNode].position, Time.deltaTime * platformSpeed);

        if (platformType == PlatformType.Reactive && !isOnPlatform && !isAtSpawn)
        {
            reactiveLagTimeCounter += Time.deltaTime;

            if (reactiveLagTimeCounter >= reactiveLagTimeToRespawn)
            {
                transform.position = movementNodes[0].position;
                isMoving = false;
                isAtSpawn = true;
                targetNode = 1;
            }
        }

        if (Vector2.Distance(transform.position, movementNodes[targetNode].position) == 0 && isMoving)
        {
            switch (platformType, isMoving)
            {
                case (PlatformType.Looping, true):
                    targetNode = targetNode < movementNodes.Length - 1 ? targetNode + 1 : 0;
                    break;
                case (PlatformType.Alternating, true):
                    if (targetNode == movementNodes.Length - 1)
                        isReversing = true;
                    
                    if (targetNode == 0)
                        isReversing = false;
                    
                    targetNode += !isReversing ? 1 : -1;
                    break;
                case (PlatformType.Reactive, true):
                    if (targetNode < movementNodes.Length - 1)
                        targetNode++;
                    else
                        isMoving = false;
                    break;
                case (PlatformType.ReactiveTwoWay, true):
                    if (targetNode == movementNodes.Length - 1)
                    {
                        isReversing = true;
                        isMoving = false;
                    }

                    if (targetNode == 0)
                    {
                        isReversing = false;
                        isMoving = false;
                    }

                    targetNode += !isReversing ? 1 : -1;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            if (platformType == PlatformType.Reactive)
            {
                isMoving = true;
                reactiveLagTimeCounter = 0.0f;
                isOnPlatform = true;
                isAtSpawn = false;
            }
            if (platformType == PlatformType.ReactiveTwoWay)
            {
                isMoving = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);

            if (platformType == PlatformType.Reactive)
                isOnPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < movementNodes.Length - 1; i++)
        {
            Gizmos.DrawLine(movementNodes[i].position, movementNodes[i + 1].position);
        }

        if (platformType == PlatformType.Looping)
            Gizmos.DrawLine(movementNodes[movementNodes.Length - 1].position, movementNodes[0].position);
    }
}

// Looping: A platform will traverse all movement nodes and then return to the first node completing a looping cycle
// Alternating : A platform will traverse all movement nodes and then follow the same path backwards
// Reactive: Will only be triggered when the player interacts with the platform, the platform will move to the final node and respawn at the start
// ReactiveTwoWay: Will only be triggered when the player interacts with the platform, the platform will move to the final node and wait for another player interaction
public enum PlatformType
{
    Looping,
    Alternating,
    Reactive,
    ReactiveTwoWay
};
