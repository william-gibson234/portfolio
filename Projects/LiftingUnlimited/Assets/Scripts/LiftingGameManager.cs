using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to manage differnt game functions that various classes may need to access
public class LiftingGameManager : MonoBehaviour
{
    public event EventHandler OnGamePaused;

    public event EventHandler OnGameUnpaused;


    public static bool isWindowOpen = false;
    public static bool isCompeting = false;
    public static bool isLifting = false;

    public static bool isInNewPauseGameUI = false;

    [SerializeField] GameInput gameInput;
    [SerializeField] Player player;

    public bool isGamePaused= false;

    private void Awake()
    {
        gameInput.OnPauseAction += GameInput_OnPauseAction;
    }
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if (!isLifting && !isWindowOpen && !isCompeting)
        {
            //Ensure that the pause screen can actually be opened
            TogglePauseGame();
        }
    }
    public void TogglePauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            //Following line pauses all game functions, such as the energy countdown and others
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        isGamePaused = !isGamePaused;
    }
}
