using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovingPlatform : MonoBehaviour
{
    public Transform[] movementNodes;

    private int nextNode;

    // 
    public enum PlatformType { Looping, Alternating, Reactive, ReactiveTwoWay };

    public PlatformType platformType;

    public float platformSpeed;

    private bool reversing = false;

    // Reactive platform variables
    private bool reactiveMoving = false;
    private bool isOnPlatform = false;
    private bool isAtSpawn = true;
    public float reactiveLagTimeToRespawn = 5f;
    private float reactiveLagTimeCounter = 0;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        nextNode = 1;
        
        if(movementNodes.Length < 2)
        {
            print($"{transform.parent.name} has less than one movement node, must have at least 2");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (movementNodes.Length < 2)
        {
            return;
        }

        if (platformType == PlatformType.Looping
            || platformType == PlatformType.Alternating
            || platformType == PlatformType.Reactive && reactiveMoving
            || platformType == PlatformType.ReactiveTwoWay && reactiveMoving)
            transform.position = Vector2.MoveTowards(transform.position, movementNodes[nextNode].position, Time.deltaTime * platformSpeed);

        if (platformType == PlatformType.Reactive && !isOnPlatform && !isAtSpawn)
        {
            reactiveLagTimeCounter += Time.deltaTime;

            if (reactiveLagTimeCounter >= reactiveLagTimeToRespawn)
            {
                transform.position = movementNodes[0].position;
                reactiveMoving = false;
                isAtSpawn = true;
                nextNode = 1;
            }
        }

        if (Vector2.Distance(transform.position, movementNodes[nextNode].position) == 0)
        {
            if (platformType == PlatformType.Looping)
            {
                if (nextNode < movementNodes.Length - 1)
                {
                    nextNode++;
                }
                else
                {
                    nextNode = 0;
                }
            }
            else if (platformType == PlatformType.Alternating)
            {
                if (nextNode == movementNodes.Length - 1)
                {
                    reversing = true;
                }
                if (nextNode == 0)
                {
                    reversing = false;
                }
                if (reversing && nextNode > 0)
                {
                    nextNode--;
                }
                else if (!reversing && nextNode < movementNodes.Length)
                {
                    nextNode++;
                }
            }
            else if (platformType == PlatformType.Reactive && reactiveMoving)
            {
                if (nextNode < movementNodes.Length - 1)
                {
                    nextNode++;
                }
                else
                {
                    reactiveMoving = false;
                }
            }
            else if (platformType == PlatformType.ReactiveTwoWay && reactiveMoving)
            {
                if (nextNode == movementNodes.Length - 1)
                {
                    reversing = true;
                    reactiveMoving = false;
                }
                if (nextNode == 0)
                {
                    reversing = false;
                    reactiveMoving = false;
                }
                if (reversing && nextNode > 0)
                {
                    nextNode--;
                }
                else if (!reversing && nextNode < movementNodes.Length)
                {
                    nextNode++;
                }
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
                reactiveMoving = true;
                reactiveLagTimeCounter = 0.0f;
                isOnPlatform = true;
                isAtSpawn = false;
            }
            if (platformType == PlatformType.ReactiveTwoWay)
            {
                reactiveMoving = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            if (platformType == PlatformType.Reactive)
            {
                isOnPlatform = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(movementNodes.Length > 0)
        {
            movementNodes[0].position = transform.position;
        }
        for (int i = 0; i < movementNodes.Length - 1; i++)
        {
            Gizmos.DrawLine(movementNodes[i].position, movementNodes[i + 1].position);
        }
        if (platformType == PlatformType.Looping)
        {
            Gizmos.DrawLine(movementNodes[movementNodes.Length - 1].position, movementNodes[0].position);
        }
    }
}