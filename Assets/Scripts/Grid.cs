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
		
	}

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
}
