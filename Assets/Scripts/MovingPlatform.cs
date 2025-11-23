using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Serializable fields
    [SerializeField] private PlatformType platformType; // Type of moving platform
    [SerializeField] private Transform[] movementNodes; // Movement nodes, a set of different nodes for the platform to move through
    [SerializeField] private float platformSpeed = 5f; // Platform Speed
    [SerializeField] private float reactiveLagTimeToRespawn = 5f; // Only used for platformType Reactive, creates a delay for which the platform takes to respawn when the player is not interacting with it

    private int targetNode = 1; // Set the initial target node to the second node in the array
    private bool isMoving; // Used to check if the platform is moving as some types do not have constant movement (Reactive, ReactiveTwoWay)
    private bool isReversing = false; // Boolean to check if we are looping backwards instead of forwards through nodes

    // Reactive platform variables
    private bool isOnPlatform = false; // Used in Reactive to check if the player is still on the platform
    private bool isAtSpawn = true; // Used in Reactive to check if the platform is currently at the spawn point
    private float reactiveLagTimeCounter = 0; // Used to create a timer for the Reactive type in order to respawn the platform after a certain amount of time (reactiveLagTimeToRespawn)

    private void Awake()
    {
        // If the amount of movement nodes is less than the needed amount display an error message to the console
        if (movementNodes.Length < 2)
        {
            print($"{transform.parent.name} has less than one movement node, must have at least 2");
            enabled = false;
        }

        // Initialize a movement node representing the platforms initial position, avoiding the need to have a node with position seperate from the platforms initial position
        GameObject movementNodeOne = new GameObject("MovementNode-1");
        movementNodeOne.transform.SetParent(transform.parent);
        movementNodeOne.transform.position = transform.position;

        movementNodes[0] = movementNodeOne.transform;
        
        // Set isMoving based off of the type of platform
        isMoving = platformType switch
        {
            PlatformType.Looping or PlatformType.Alternating => true,
            _ => false
        };
    }

    // Update is called once per frame
    private void Update()
    {
        // If the platform is moving update its position
        if (isMoving)
            transform.position = Vector2.MoveTowards(transform.position, movementNodes[targetNode].position, Time.deltaTime * platformSpeed);

        // If the platform type is Reactive and the player is not on the platform and it is not at its spawn point
        if (platformType == PlatformType.Reactive && !isOnPlatform && !isAtSpawn)
        {
            // Increment the lag timer
            reactiveLagTimeCounter += Time.deltaTime;

            // If the lag timer is greater than the amount required to respawn reset it to start 
            if (reactiveLagTimeCounter >= reactiveLagTimeToRespawn)
            {
                transform.position = movementNodes[0].position;
                isMoving = false;
                isAtSpawn = true;
                targetNode = 1;
            }
        }

        // If we have reached the next node in the path
        if (Vector2.Distance(transform.position, movementNodes[targetNode].position) == 0 && isMoving)
        {
            // Check for the type of platform so we can respond accordingly
            switch (platformType, isMoving)
            {
                // If Platform type is looping and it is moving
                case (PlatformType.Looping, true):
                    // Cycle through the movement nodes, if we have reached the end set the next node to the start
                    targetNode = targetNode < movementNodes.Length - 1 ? targetNode + 1 : 0;
                    break;
                // If the platform type is alternating and it is moving
                case (PlatformType.Alternating, true):
                    // we should not be reversing, if we reach the end reverse movement
                    if (targetNode == movementNodes.Length - 1)
                        isReversing = true;
                    // we should be reversing, if we reach the start reverse movement to forward
                    if (targetNode == 0)
                        isReversing = false;
                    
                    // if we are reversing move forward through the nodes, if not move backwards
                    targetNode += !isReversing ? 1 : -1;
                    break;
                // If the platform type is reactive and it is moving
                case (PlatformType.Reactive, true):
                    // move forward through the list until we reach the end then stop moving
                    if (targetNode < movementNodes.Length - 1)
                        targetNode++;
                    else
                        isMoving = false;
                    break;
                // If the platform is reactive two way and it is moving
                case (PlatformType.ReactiveTwoWay, true):
                    // if we are going forward and the target node is the last node, reverse and stop moving
                    if (targetNode == movementNodes.Length - 1)
                    {
                        isReversing = true;
                        isMoving = false;
                    }
                    // if we are going backwards and the target node is the first node, reverse and stop moving
                    if (targetNode == 0)
                    {
                        isReversing = false;
                        isMoving = false;
                    }

                    // if we are going forward cycle forward through nodes, if we are going backwards cycle backwards
                    targetNode += !isReversing ? 1 : -1;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if the player has interacted with the platform
        if (collision.collider.CompareTag("Player"))
        {
            // if so set the parent to the platform so the player will move with it
            collision.transform.SetParent(transform);
            // if the platform is Reactive start movement and set up values to allow for respawning if players stops interacting with it
            if (platformType == PlatformType.Reactive)
            {
                isMoving = true;
                reactiveLagTimeCounter = 0.0f;
                isOnPlatform = true;
                isAtSpawn = false;
            }
            // if the platform is ReactiveTwoWay start moving
            if (platformType == PlatformType.ReactiveTwoWay)
                isMoving = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Once the player leaves the platform
        if (collision.collider.CompareTag("Player"))
        {
            // Set the players parent to null to stop moving with the platform
            collision.transform.SetParent(null);

            // If Reactive check if we have left the platform so that it can respawn
            if (platformType == PlatformType.Reactive)
                isOnPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Used to draw lines for visual component, allowing developers to see the path that a platform will move on
        for (int i = 0; i < movementNodes.Length - 1; i++)
            Gizmos.DrawLine(movementNodes[i].position, movementNodes[i + 1].position);

        if (platformType == PlatformType.Looping)
            Gizmos.DrawLine(movementNodes[movementNodes.Length - 1].position, movementNodes[0].position);
    }
}

// Looping: A platform will traverse all movement nodes and then return to the first node completing a looping cycle
// Alternating : A platform will traverse all movement nodes and then follow the same path backwards
// Reactive: Will only be triggered when the player interacts with the platform, the platform will move to the final node and respawn at the start after a timer
// ReactiveTwoWay: Will only be triggered when the player interacts with the platform, the platform will move to the final node and wait for another player interaction
public enum PlatformType
{
    Looping,
    Alternating,
    Reactive,
    ReactiveTwoWay
};
