using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    enum InputAction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Rotate = 3,
        Drop = 4,
        SoftDrop = 5,
        Pause = 6
    }

    private InputAction blockAction;
    private IEnumerator ActionHoldRoutine;
    private GameStateManager managerInstance;
    private Spawner spawnerInstance; // Instance of spawner to get the latest block from

    public float actionDelay = .2f; // Delay on a repeating action when holding down a key

    void Start () {
        managerInstance = gameObject.GetComponent<GameStateManager>();
        spawnerInstance = gameObject.GetComponent<Spawner>();
	}
	
    /// <summary>
    /// Get Inputs and decide action accordingly
    /// </summary>
	void Update () {
        if (Input.GetButtonDown("Left"))
        {
            blockAction = InputAction.Left;
        }
        else
        if (Input.GetButtonDown("Right"))
        {
            blockAction = InputAction.Right;
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

        if (blockAction != InputAction.None)
        {
            PerformBlockAction(blockAction);
            blockAction = InputAction.None;
        }
    }

    private void PerformBlockAction(InputAction action)
    {
        if (!spawnerInstance.CurrentBlock)
        {
            return;
        }

        switch ((int)action)
        {
            case 0:
                break;
            case 1:
                spawnerInstance.CurrentBlock.Move(Vector2.left);
                break;
            case 2:
                spawnerInstance.CurrentBlock.Move(Vector2.right);
                break;
            case 3:
                spawnerInstance.CurrentBlock.Rotate();
                break;
            case 4:
                spawnerInstance.CurrentBlock.Drop();
                break;
            case 5:
                spawnerInstance.CurrentBlock.Move(Vector2.down);
                break;
            case 6:
                managerInstance.ToggleGame();
                break;
        }

        // Stop previous repeating action and check if the new key is being held down
        if (ActionHoldRoutine != null)
        {
            StopCoroutine(ActionHoldRoutine);
        }
        ActionHoldRoutine = HoldKey(action);
        StartCoroutine(ActionHoldRoutine);
    }

    private IEnumerator HoldKey(InputAction action)
    {
        while (Input.GetButton(action.ToString()))
        {
            blockAction = action;
            yield return new WaitForSeconds(actionDelay);
        }
    }
}
