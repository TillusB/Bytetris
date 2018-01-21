using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic block ( Tetromino )
/// Moves down automatically and can be moved around via the InputManager.
/// Methods are virtual for special blocks which inherit Block (e.g. O-Blocks which don't rotate or IBlocks which need some extra setup)
/// </summary>
public class Block : MonoBehaviour {

    // Downward speed/delay and its original value. Static since we need this elsewhere and we don't want to search for one of the dynamically instantiated blocks every time.
    public static float moveDelay = 0.5f;
    private static float standardMoveDelay = moveDelay;
    public static float StandardMoveDelay
    {
        get
        {
            return standardMoveDelay; // No setter because this value is not supposed to change.
        }
    }

    private float timeSinceMove = 0;
    protected Grid gridInstance;
    protected GameStateManager stateManagerInstance;
    protected Vector2[] previousPos = new Vector2[4];
    protected bool lastChance = false;

    public bool active = true;


    protected virtual void Start () {
        gridInstance = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        stateManagerInstance = gridInstance.GetComponent<GameStateManager>();

        SetPreviousPos(); // Register this blocks' start position on the grid

        if (!NextPosAvailable(Vector2.zero)) // If the block is immediately unable to move, the game ends.
        {
            stateManagerInstance.EndGame();
        }
	}

    /// <summary>
    /// 
    /// In Update we wait for the current delay - one "tick" of the game so to say - to move the current block downwards if possible.
    /// If a piece can't move downwards anymore, lastChance gets flagged one tick before the piece gets locked in position for last adjustments by the player.
    /// 
    /// Movedelay is the value that defines the length of a tick. It gets adjusted in the Grid class, whenever a line gets cleared.
    /// This way the pace picks up the better a player is doing.
    /// 
    /// 
    /// </summary>
    protected virtual void Update()
    {
        if (active)
        {
            timeSinceMove += Time.deltaTime;
            if (timeSinceMove >= moveDelay)
            {
                timeSinceMove = 0;

                if (!NextPosAvailable(Vector2.down) && lastChance)
                {
                    active = false;

                    foreach (Transform child in transform)
                    {
                        gridInstance.CheckLineFull(Mathf.RoundToInt(child.position.y));
                    }
                }
                else if (!NextPosAvailable(Vector2.down) & !lastChance)
                {
                    lastChance = true;
                    return;
                }
                else
                {
                    Move(Vector2.down);
                }
            }
        }
    }

    /// <summary>
    /// Move block in a direction if possible
    /// </summary>
    /// <param name="movementV">Direction</param>
    public virtual void Move(Vector2 movementV)
    {
        if (NextPosAvailable(movementV) && active)
        {
            transform.position = (Vector2)transform.position + movementV;

            UpdatePositionToGrid();
            SetPreviousPos();
            //lastChance = false; 

        }
    }

    /// <summary>
    /// Rotate the block by 90 degrees if there is space.
    /// Otherwise rotate it back to its previous orientation.
    /// 
    /// TODO: allow rotation if the block is only obstructed horizontally -> push the block away from the obstacle instead without clipping into another.
    /// </summary>
    public virtual void Rotate()
    {
        if (active)
        {
            transform.Rotate(new Vector3(0, 0, -90));
            if (!NextPosAvailable(Vector3.zero))
            {
                transform.Rotate(new Vector3(0, 0, 90));
                return;
            }
            UpdatePositionToGrid();
            SetPreviousPos();
        }
    }

    /// <summary>
    /// Move the block all the way down in an instant.
    /// </summary>
    public virtual void Drop()
    {
        if (active)
        {
            while (NextPosAvailable(Vector2.down)){
                Move(Vector2.down);
                UpdatePositionToGrid();
                SetPreviousPos();
            }
            lastChance = true;
            timeSinceMove = moveDelay;
        }
    }

    /// <summary>
    /// Tell the grid which slots are taken by the sub-blocks of this tetromino
    /// </summary>
    protected virtual void UpdatePositionToGrid()
    {
        Transform childTransform;
        foreach (Transform t in transform) // First remove all entries to avoid tiles deleting each others' entries on their previous positions
        {
            gridInstance.grid[Mathf.RoundToInt(previousPos[t.GetSiblingIndex()].x), Mathf.RoundToInt(previousPos[t.GetSiblingIndex()].y)] = null;
        }

        for (int childI = 0; childI < previousPos.Length; childI++)
        {
            childTransform = transform.GetChild(childI);
            gridInstance.grid[Mathf.RoundToInt(childTransform.position.x), Mathf.RoundToInt(childTransform.position.y)] = childTransform;
        }
    }

    /// <summary>
    /// Iterate through the child-transforms and check if their new positions will be available
    /// </summary>
    /// <param name="movementV">Vector of desired movement</param>
    protected virtual bool NextPosAvailable(Vector2 movementV)
    {
        bool available = true;
        int newX;
        int newY;

        foreach (Transform child in transform)
        {
            newX = Mathf.RoundToInt(child.position.x + movementV.x);
            newY = Mathf.RoundToInt(child.position.y + movementV.y);

            if (newX >= gridInstance.width || newX < 0 || newY >= gridInstance.height || newY < 0) return false; // cover edge cases

            if (gridInstance.grid[newX, newY] != null && gridInstance.grid[newX, newY].parent != transform) // Second half of this statement verifies that an obstacle is not actually just a block of this tetromino - which will be moved too.
            {
                available = false;
            }
        }

        return available;
    }

    /// <summary>
    /// Keep track of the previous position to clear from the grid it after we move.
    /// </summary>
    protected virtual void SetPreviousPos()
    {
        foreach (Transform child in transform)
        {
            previousPos[child.GetSiblingIndex()] = child.position;
        }
    }
}