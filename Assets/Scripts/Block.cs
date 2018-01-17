using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private float timeSinceMove = 0;
    private Grid gridInstance;
    private Vector2[] previousPos = new Vector2[4];
    
    public float moveDelay = 0.5f;
    public bool active = true;

	void Start () {
        gridInstance = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>(); //TODO: Maybe go through manager

        SetPreviousPos();
	}
	
	void Update () {
        if (active)
        {
            timeSinceMove += Time.deltaTime;
            if(timeSinceMove >= moveDelay)
            {
                timeSinceMove = 0;
                Move(Vector2.down);

                if (!NextPosAvailable(Vector2.down))
                {
                    active = false;

                    foreach (Transform child in transform)
                    {
                        gridInstance.CheckLineFull(Mathf.RoundToInt(child.position.y));
                    }
                }
            }
        }
	}


    public void Move(Vector2 movementV)
    {
        if (NextPosAvailable(movementV))
        {
            transform.position = (Vector2)transform.position + movementV;

            UpdatePositionToGrid();
            SetPreviousPos();
        }
    }

    public void Rotate()
    {
        
        transform.Rotate(new Vector3(0, 0, -90));
        
        UpdatePositionToGrid();
        SetPreviousPos();
    }

    internal void Drop()
    {
        throw new NotImplementedException();
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
