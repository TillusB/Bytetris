using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private float timeSinceMove = 0;
    private Grid gridInstance;
    private bool activeBlock = true;
    private Vector2[] previousPos = new Vector2[4];

    public float moveDelay = 0.5f;

	// Use this for initialization
	void Start () {
        gridInstance = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>(); //TODO: Maybe go through manager

        SetPreviousPos();
	}
	
	// Update is called once per frame
	void Update () {
        if (activeBlock)
        {
            timeSinceMove += Time.deltaTime;
            if(timeSinceMove >= moveDelay)
            {
                timeSinceMove = 0;
                Move(Vector2.down);

                if (!NextPosAvailable(Vector2.down))
                {
                    activeBlock = false;
                }
            }
        }
	}

    private void Move(Vector2 movementV)
    {
        if (NextPosAvailable(movementV))
        {
            transform.position = (Vector2)transform.position + movementV;

            UpdatePositionToGrid();
            SetPreviousPos();
        }
    }

    private void UpdatePositionToGrid()
    {
        Transform childTransform;
        for (int childI = 0; childI < previousPos.Length; childI++)
        {
            gridInstance.grid[(int)previousPos[childI].x, (int)previousPos[childI].y] = null;

            childTransform = transform.GetChild(childI);
            gridInstance.grid[(int)childTransform.position.x, (int)childTransform.position.y] = childTransform;
        }
    }

    private bool NextPosAvailable(Vector2 movementV)
    {
        bool available = true;
        int newX;
        int newY;

        foreach (Transform child in transform)
        {
            newX = (int)(child.position.x + movementV.x);
            newY = (int)(child.position.y + movementV.y);

            if (newX > gridInstance.width || newX < 0 || newY > gridInstance.height || newY < 0) return false;

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
