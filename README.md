# Simple-Non-Grid-Tile-Pathfinding

## What is this?
A simple pathfinding based on grid movement that doesn't requires a grid.

## How does it works?
Instead of finding the entire path at the start, each time it moves, it checks for available steps on the allowed directions and chooses the closer one to the goal position.

## What is a collision tile?
A simple scritptable object included in the code that allows to create custom tiles with the 'hasCollision' property.

## How do I use it?
1. Create two child objects inside of the object you want to move, one for movement and one for checking.
2. Add the script to the object you want to move.
3. Set the required variables (Move Point -> movement child. Check Point = checking child. Dest Goal Transform = Destination goal.)
4. Choose if you want to check collisions by overlapping a circle or by checking the tile (requires collision tiles)

## I have a question that isn't here
Either create an issue in github and I'll try to answer that or check the code, it has comments for almost everything.
