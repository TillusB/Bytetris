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
    }

    private InputAction blockAction;

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

        switch (action)
        {
            case 0:
                break;
            case InputAction.MoveLeft:
                currentBlock.Move(Vector2.left);
                break;
            case InputAction.MoveRight:
                currentBlock.Move(Vector2.right);
                break;
            case InputAction.Rotate:
                currentBlock.Rotate();
                break;
            case InputAction.Drop:
                currentBlock.Drop();
                break;
            case InputAction.SoftDrop:
                currentBlock.Move(Vector2.down);
                break;
        }
    }
}
