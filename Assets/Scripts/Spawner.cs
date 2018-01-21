using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Spawner class Instantiates new blocks whenever the current one is set down.
/// </summary>
public class Spawner : MonoBehaviour {
    public Block[] availableBlocks;

    private Grid gridInstance;
    private Block nextBlock;
    private Block holdBlock;
    private InputManager inputInstance;

    public Text nextUpText;
    public Vector2 spawnPoint;

    private Block currentBlock;
    public Block CurrentBlock // Readonly in case we have to get the CurrentBlock from outside this class
    {
        get { return currentBlock; }
        private set
        {
            currentBlock = value;
        }
    }

	void Start () {
        gridInstance = GetComponent<Grid>();
        inputInstance = GetComponent<InputManager>();

        spawnPoint = new Vector2(Mathf.RoundToInt(gridInstance.width / 2), gridInstance.height-3); // Spawnpoint low enough for any block to spawn within the grids' bounds

        nextBlock = availableBlocks[Random.Range(0, availableBlocks.Length)];
    }

    void FixedUpdate () { //we don't need to check on our block every frame
		if(currentBlock == null || !currentBlock.active)
        {
            CurrentBlock = Instantiate(nextBlock, spawnPoint, Quaternion.identity);
            GetNextBlock(); 
            
        }
    }

    /// <summary>
    /// Next block is picked randomly and then displayed in the UI
    /// </summary>
    private void GetNextBlock()
    {
        nextBlock = availableBlocks[Random.Range(0, availableBlocks.Length)];
        nextUpText.text = nextBlock.name;
    }
}
