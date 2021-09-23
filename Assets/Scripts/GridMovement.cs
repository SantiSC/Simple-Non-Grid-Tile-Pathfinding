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
/// Simple class for grid-based movement that checks collision with overlap circle and tile check
/// </summary>

using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    // X and Y axis mov
    private float movX;
    private float movY;

    // Collision layer used to check for collisions
    public LayerMask collisionLayer;

    // Player movement speed
    public float movSpeed = 5f;

    // Next point for grid movement
    public Transform movePoint;

    // Test tilemap
    public Tilemap tileMap;

    void Start()
    {
        movePoint.parent = null;
    }

    void Update()
    {
        // Get the x and y axis
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            // TODO: Separate overlapcircle check and tile check.

            // Horizontal movement
            if (Mathf.Abs(movX) == 1f)
            {
                // Check for collisions
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(movX, 0f, 0f), 0.2f, collisionLayer))
                {
                    // Get a vector3 int (because to get a tile on a given position this type it's needed) on the step position
                    Vector3Int colPos = Vector3Int.FloorToInt(movePoint.position) + Vector3Int.FloorToInt(new Vector3(movX, 0f, 0f));

                    // Get the tile at the checked position and check if its a collision tile (scriptable object)
                    CollisionTile nextTile = tileMap.GetTile<CollisionTile>(colPos);

                    // If the tile isn't null, and it has collision
                    if (nextTile != null && nextTile.hasCollision == true)
                    {
                        // Handle collision here.
                    }
                    // Otherwise, move.
                    else
                    {
                        movePoint.position += new Vector3(movX, 0f, 0f);
                    }
                }

                // Animation here
                if (movX == 1f)
                {

                }
                else if (movX == -1f)
                {

                }
            }
            // Vertical movement
            else if (Mathf.Abs(movY) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, movY, 0f), 0.2f, collisionLayer))
                {
                    // Get a vector3 int (because to get a tile on a given position this type it's needed) on the step position
                    Vector3Int colPos = Vector3Int.FloorToInt(movePoint.position) + Vector3Int.FloorToInt(new Vector3(0f, movY, 0f));

                    // Get the tile at the checked position and check if its a collision tile (scriptable object)
                    CollisionTile nextTile = tileMap.GetTile<CollisionTile>(colPos);

                    // If the tile isn't null, and it has collision
                    if (nextTile != null && nextTile.hasCollision == true)
                    {
                        // Handle collision here.
                    }
                    // Otherwise, move.
                    else
                    {
                        movePoint.position += new Vector3(0f, movY, 0f);
                    }
                }

                // Animation here
                if (movY == 1f)
                {

                }
                else if (movY == -1f)
                {

                }
            }
        }
    }
}