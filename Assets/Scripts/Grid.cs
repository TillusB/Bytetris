using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public int width = 10;
    public int height = 20;
    public Transform[,] grid;

	// Use this for initialization
	void Start () {
        grid = new Transform[width, height];
        ClearGrid();
	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawLine(new Vector2(0, 0), new Vector2(width, 0));
        Debug.DrawLine(new Vector2(0, 0), new Vector2(0, height));
        

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y]) Gizmos.color = Color.red;
                else Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(x, y, 0), new Vector3(.5f, .5f, .5f));
            }
        }
    }
#endif

    public void ClearGrid()
    {
        Transform block;
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                block = grid[x, y];
                if (block != null)
                {
                    Destroy(block);
                    block = null;
                }
            }
        }
    }

    internal void CheckLineFull(int yCoordinate)
    {
        bool isfull = true;
        for(int x = 0; x < width; x++)
        {
            if (!grid[x, yCoordinate])
            {
                isfull = false;
            }
        }

        if (isfull)
        {
            ClearLine(yCoordinate);
        }
    }

    private void ClearLine(int yCoordinate)
    {
        Transform clearBlock;
        for(int x = 0; x < width; x++)
        {
            clearBlock = grid[x, yCoordinate];
            if (clearBlock)
            {
                grid[x, yCoordinate] = null;
                Destroy(clearBlock.gameObject);
            }
        }

        DropLinesAbove(yCoordinate);
        IncreaseSpeed();
    }

    private void DropLinesAbove(int yCoordinate) // Drop all lines above this height. Used after lines are cleared
    {
        Transform moveBlock;
        for (int y = yCoordinate; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                moveBlock = grid[x, y];
                if (moveBlock) // There is a block here
                {
                    grid[x, y] = null;
                    grid[x, y - 1] = moveBlock;
                    moveBlock.position = new Vector3(x, y - 1, 0);
                }
            }
        }
    }

    private void IncreaseSpeed()
    {
        Block.moveDelay -= Block.moveDelay / 10;
    }

}
