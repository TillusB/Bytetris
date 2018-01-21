using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public int width = 10;
    public int height = 20;
    public float difficultyIncrement = 0.01f; // Difficulty values are public for in-editor balancing
    public float minDelay = 0.03f;

    public Transform[,] grid;
    public Material gridMat;

	// Use this for initialization
	void Start () {
        grid = new Transform[width, height];
        ClearGrid();

        ShowGrid();
	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawLine(new Vector2(0, 0), new Vector2(width, 0));
        Debug.DrawLine(new Vector2(0, 0), new Vector2(0, height));
        

    }

    /// <summary>
    /// Spawn static cubes on each slot of the grid as a backdrop. A sprite would work too of course.
    /// </summary>
    private void ShowGrid()
    {
        GameObject block;
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.parent = transform;
                block.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                block.GetComponent<Renderer>().material = gridMat;
                Destroy(block.GetComponent<Collider>());
                block.isStatic = true;
                block.transform.position = new Vector2(x, y);
            }
        }
    }

//#if UNITY_EDITOR //Only for debug purposes
//    private void OnDrawGizmos()
//    {
//        for (int y = 0; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                if (grid[x, y]) Gizmos.color = Color.red;
//                else Gizmos.color = Color.green;
//                Gizmos.DrawCube(new Vector3(x, y, 0), new Vector3(.5f, .5f, .5f));
//            }
//        }
//    }
//#endif

    /// <summary>
    /// Set all grid slots to free. Destroy any objects that are on the slots
    /// </summary>
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
                    Destroy(block.gameObject);
                    block = null;
                }
            }
        }
    }

    /// <summary>
    /// Check if the line on the specified height is filled. If so go ahead and clear it.
    /// </summary>
    public void CheckLineFull(int yCoordinate)
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

    /// <summary>
    /// Clear line, free up the grid slots, drop all lines above and increase difficulty
    /// </summary>
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

    /// <summary>
    /// Drop all lines above this height. Used after lines are cleared
    /// </summary>
    private void DropLinesAbove(int yCoordinate)
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

    /// <summary>
    /// Increse the speed of falling blocks incrementally.
    /// If we hit the specified maximum difficulty (minimum delay) keep it there.
    /// </summary>
    private void IncreaseSpeed()
    {
        Block.moveDelay -= difficultyIncrement;
        if(Block.moveDelay <= minDelay)
        {
            Block.moveDelay = minDelay;
        }
    }

}
