using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The GameStateManager takes note of whether the game should be running, paused or end.
/// Pausing simply suspends blocks and stops new ones from spawning.
/// </summary>
public class GameStateManager : MonoBehaviour {
    
    public enum GameState
    {
        None,
        Play,
        Pause,
        End
    }

    public Button PlayPauseButton;

    private Spawner spawnerInstance;
    private InputManager inputInstance;
    private Grid gridInstance;
    private Text ButtonText;

    private float originalMovedelay = Block.moveDelay;
    private GameState state = GameState.None;

    public GameState State
    {
        get { return state; }
        private set { state = value; }
    }

	// Use this for initialization
	void Start () {
        gridInstance = gameObject.GetComponent<Grid>();
        inputInstance = gameObject.GetComponent<InputManager>();
        spawnerInstance = gameObject.GetComponent<Spawner>();

        PlayPauseButton.onClick.AddListener(ToggleGame); // The UI-Button has multiple purposes depending on the game state
        ButtonText = PlayPauseButton.GetComponentInChildren<Text>(); // The text should always reflect its current purpose
	}

    /// <summary>
    /// disables the spawner
    /// stores the current Block.movedelay for unpausing
    /// sets the delay to infinity, so the block will not move.
    /// </summary>
    public void PauseGame()
    {
        if(state == GameState.Pause)
        {
            return;
        }

        spawnerInstance.enabled = false;
        inputInstance.enabled = false;
        originalMovedelay = Block.moveDelay;
        Block.moveDelay = Mathf.Infinity;

        ButtonText.text = "Continue";

        state = GameState.Pause;
    }

    /// <summary>
    /// enables the spawner and resets the movement delay of blocks to the previously stored value.
    /// </summary>
    public void UnpauseGame()
    {
        if (state == GameState.Play) 
        {
            return;
        }

        inputInstance.enabled = true;
        spawnerInstance.enabled = true;
        Block.moveDelay = originalMovedelay;

        ButtonText.text = "Pause";
        state = GameState.Play;
    }
    /// <summary>
    /// Clear the grid and reset the MovementDelay to default after the game ended. Resume play.
    /// </summary>
    public void StartGame()
    {
        gridInstance.ClearGrid();
        Block.moveDelay = Block.StandardMoveDelay;
        UnpauseGame();
    }

    /// <summary>
    /// stop spawning or moving blocks
    /// </summary>
    public void EndGame()
    {
        inputInstance.enabled = false;
        spawnerInstance.enabled = false;
        Block.moveDelay = Mathf.Infinity;

        ButtonText.text = "Restart";
        state = GameState.End;
    }

    /// <summary>
    /// Toggle the game depending on current state. Restart if the game ended.
    /// </summary>
    public void ToggleGame()
    {
        if(state == GameState.Play)
        {
            PauseGame();
        }else if(state == GameState.Pause)
        {
            UnpauseGame();
        }else if(state == GameState.None)
        {
            UnpauseGame();
        }else if(state == GameState.End)
        {
            StartGame();
        }
    }


}
