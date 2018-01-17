using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Block[] availableBlocks;

    private Grid gridInstance;
    private Block nextBlock;
    private Vector2 spawnPoint;
    private InputManager input;

    private Block currentBlock;
    public Block CurrentBlock
    {
        get { return currentBlock; }
        private set {
            currentBlock = value;
            input.CurrentBlock = currentBlock;
        }
    }

	void Start () {
        gridInstance = GetComponent<Grid>();
        input = GetComponent<InputManager>();

        spawnPoint = new Vector2(Mathf.RoundToInt(gridInstance.width / 2), gridInstance.height - 4);

        nextBlock = availableBlocks[Random.Range(0, availableBlocks.Length - 1)];
    }

    void FixedUpdate () {
		if(currentBlock == null || !currentBlock.active)
        {
            CurrentBlock = Instantiate(nextBlock, spawnPoint, Quaternion.identity);
            nextBlock = availableBlocks[Random.Range(0, availableBlocks.Length - 1)];
        }
	}
    private void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            for(int i = 0; i < availableBlocks.Length; i++)
            {
                availableBlocks[i] = availableBlocks[0];
            }
            nextBlock = availableBlocks[0];
            Destroy(currentBlock.gameObject);
            CurrentBlock = Instantiate(nextBlock, spawnPoint, Quaternion.identity);
        }
    }
}
