using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private float timeSinceMove = 0;
    private Grid gridInstance;
    private Vector2[] previousPos = new Vector2[4];
    private bool lastChance = false;
    
    public static float moveDelay = 0.5f;
    public bool active = true;

	void Start () {
        gridInstance = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

        SetPreviousPos(); // Register this blocks' start position on the grid
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
	void Update () {
        if (active)
        {
            timeSinceMove += Time.deltaTime;
            if(timeSinceMove >= moveDelay)
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
                else if(!NextPosAvailable(Vector2.down) &! lastChance)
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
    public void Move(Vector2 movementV)
    {
        if (NextPosAvailable(movementV))
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
    /// TODO: allow rotation if the block is only obstructed horizontally -> push the block away from the obstacle instead.
    /// </summary>
    public void Rotate()
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

    /// <summary>
    /// Move the block all the way down in an instant.
    /// </summary>
    internal void Drop()
    {
        while (NextPosAvailable(Vector2.down)){
            Move(Vector2.down);
            UpdatePositionToGrid();
            SetPreviousPos();
        }
        lastChance = true;
        timeSinceMove = moveDelay;
    }

    private void UpdatePositionToGrid()
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

    private bool NextPosAvailable(Vector2 movementV)
    {
        bool available = true;
        int newX;
        int newY;

        foreach (Transform child in transform)
        {
            newX = Mathf.RoundToInt(child.position.x + movementV.x);
            newY = Mathf.RoundToInt(child.position.y + movementV.y);

            if (newX >= gridInstance.width || newX < 0 || newY >= gridInstance.height || newY < 0) return false;

            if (gridInstance.grid[newX, newY] != null && gridInstance.grid[newX, newY].parent != transform)
            {
                available = false;
            }
        }

        return available;
    }

    private void SetPreviousPos()
    {
        foreach (Transform child in transform)
        {
            previousPos[child.GetSiblingIndex()] = child.position;
        }
    }
}
