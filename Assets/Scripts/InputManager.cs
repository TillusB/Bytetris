using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    enum InputAction
    {
        None = 0,
        MoveLeft = 1,
        MoveRight = 2,
        Rotate = 3,
        Drop = 4,
        SoftDrop = 5,
        Pause = 6
    }

    private InputAction blockAction;
    private GameStateManager managerInstance;

    private Block currentBlock;
    public Block CurrentBlock
    {
        get
        {
            return currentBlock;
        }

        set
        {
            currentBlock = value;
        }
    }

    void Start () {
        managerInstance = gameObject.GetComponent<GameStateManager>();
	}
	
	void Update () {
        if (Input.GetButtonDown("Left"))
        {
            blockAction = InputAction.MoveLeft;
        }
        else
        if (Input.GetButtonDown("Right"))
        {
            blockAction = InputAction.MoveRight;
        }
        else
        if (Input.GetButtonDown("Rotate"))
        {
            blockAction = InputAction.Rotate;
        }
        else
        if (Input.GetButtonDown("Drop"))
        {
            blockAction = InputAction.Drop;
        }
        else
        if (Input.GetButtonDown("SoftDrop"))
        {
            blockAction = InputAction.SoftDrop;
        }
        else
        if (Input.GetButtonDown("Pause"))
        {
            blockAction = InputAction.Pause;
        }
        else
        {
            blockAction = InputAction.None;
        }

        if (blockAction != InputAction.None)
        {
            PerformBlockAction(blockAction);
        }
    }

    private void PerformBlockAction(InputAction action)
    {
        if (!currentBlock)
        {
            return;
        }

        switch ((int)action)
        {
            case 0:
                break;
            case 1:
                currentBlock.Move(Vector2.left);
                break;
            case 2:
                currentBlock.Move(Vector2.right);
                break;
            case 3:
                currentBlock.Rotate();
                break;
            case 4:
                currentBlock.Drop();
                break;
            case 5:
                currentBlock.Move(Vector2.down);
                break;
            case 6:
                managerInstance.ToggleGame();
                break;
        }
    }
}
