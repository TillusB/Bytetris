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

    private Spawner spawner;
    private InputManager input;
    private Text ButtonText;
    private GameState state = GameState.None;
    private float originalMovedelay = Block.moveDelay;

    public GameState State
    {
        get { return state; }
        private set { state = value; }
    }

	// Use this for initialization
	void Start () {
        input = gameObject.GetComponent<InputManager>();
        spawner = gameObject.GetComponent<Spawner>();

        PlayPauseButton.onClick.AddListener(ToggleGame);
        ButtonText = PlayPauseButton.GetComponentInChildren<Text>();
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

        spawner.enabled = false;
        input.enabled = false;
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

        input.enabled = true;
        spawner.enabled = true;
        Block.moveDelay = originalMovedelay;

        ButtonText.text = "Pause";
        state = GameState.Play;
    }

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
        }
    }

}
