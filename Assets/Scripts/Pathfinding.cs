/*
The MIT License (MIT)

Copyright (c) 2021 SantiSC

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

/// <summary>
/// Simple pathfinding based on grid-movement that doesn't actually require a grid.
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    // Destination point
    public Transform movePoint;

    // Point to check the step
    public Transform checkPoint;

    // The goal destination of the entity, in this case, the player.
    public Transform goalDestTransform;

    // Allowed directions
    private Vector2[] allowedDirections;

    // Closer step available
    private Vector2 closerStep;

    // Movement speed of the entity
    public float movSpeed = 5f;

    // Flag to declare wether the pathfinding uses a overlap cicrcle or a check tile method to check collisions.
    public bool usesCollisionTiles = false;

    // Closer distance checked
    private float closerDistance;

    private Queue<Vector2> lastPositions = new Queue<Vector2>();

    // Flag to set if it can move or not wether if it has arrived to the current step destination.
    bool nextStep = false;

    void Awake()
    {
        // Set allowed directions. (Remove any direction to disallow it.)
        allowedDirections = new Vector2[]
        {
            // Right
            new Vector2(1, 0),
            
            // Up
            new Vector2(0, 1),

            // Left
            new Vector2(-1, 0),

            // Down
            new Vector2(0, -1)
        };
    }

    void Start()
    {
        // Set the go point parent as null to move
        movePoint.parent = null;
    }

    void Update()
    {
        // For testing purposes, press L to start the pathfind
        if (Input.GetKeyDown(KeyCode.L))
        {
            FindStep();
        }
    }

    private void FixedUpdate()
    {
        // If can walk the next step
        if (nextStep)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= 0)
            {
                nextStep = false;
                FindStep();
            }
        }
    }

    /// <summary>
    /// Finds the next step based on distance and cost
    /// </summary>
    private void FindStep()
    {
        // Set closer distance to infinity so the 1st calculated distance is the closer one.
        closerDistance = Mathf.Infinity;

        // Check next position
        foreach (Vector2 direction in allowedDirections)
        {
            // 0 -> (1, 0)
            // 1 -> (0, 1)
            // 2 -> (-1, 0)
            // 3 -> (0, -1)

            Vector2 step = (Vector2)checkPoint.position + direction;

            // Check if there's a collision on the step
            if (!usesCollisionTiles)
            {
                if (!Physics2D.OverlapCircle(step, 0.1f))
                {
                    // Check for collisions on step + every direction.
                    if (!Physics2D.OverlapCircle(step + direction, 0.1f) || !Physics2D.OverlapCircle(step + allowedDirections[1], 0.1f) || !Physics2D.OverlapCircle(step + allowedDirections[3], 0.1f))
                    {
                        // Calculate the distance between player position and step position.
                        // If the lastPositions queue contains this step, add cost to it to penalize returning to the same spot
                        float totalCost = Vector2.Distance(step, goalDestTransform.position) + (lastPositions.Contains(step) ? 1 : 0);

                        if (totalCost < closerDistance)
                        {
                            // Set the total cost as the closer distance
                            closerDistance = totalCost;

                            // Set the current step as the closer step
                            closerStep = step;
                        }
                    }
                }
            }
            else
            {
                // TODO: Add collision check with collision tiles
            }

        }

        movePoint.position = closerStep;

        // Add the last position to the queue
        EnqueuePosition(transform.position);

        nextStep = true;
    }

    /// <summary>
    /// Enqueues the given position
    /// </summary>
    /// <param name="position">Position to be enqueued</param>
    private void EnqueuePosition(Vector2 position)
    {
        // Limit the queue to 3 positions
        if (lastPositions.Count >= 3)
        {
            lastPositions.Dequeue();
        }

        // Enqueue a new position
        lastPositions.Enqueue(position);
    }
}