using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//Class to control functionality of the UI element that displays when the Player Pauses the game
public class PauseScreenUI : MonoBehaviour
{
    public event EventHandler OnOptionsButtonPress;

    [SerializeField] private LiftingGameManager lgM;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private SoundManager sM;

    public event EventHandler OnMainMenuOpen;

    private void Awake() 
    {
        //Adding listeners to each of the buttons on the Pause screen
        mainMenuButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            DataPersistenceManager.instance.SaveGame();
            OnMainMenuOpen?.Invoke(this, EventArgs.Empty);
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        resumeButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            lgM.TogglePauseGame();
        });
        optionsButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            OnOptionsButtonPress?.Invoke(this, EventArgs.Empty);
        });

        lgM.OnGamePaused += LiftingGameManager_OnGamePaused;
        lgM.OnGameUnpaused += LiftingGameManager_OnGameUnpaused;
    }
    private void Start()
    {
        Hide();
    }
private void LiftingGameManager_OnGamePaused(object sender, EventArgs e)
{
    //Ensures that the player is not in the middle of a lift or has a different screen open
    if (!LiftingGameManager.isCompeting && !LiftingGameManager.isLifting&&!LiftingGameManager.isWindowOpen)
    {
        Show();
    }
}
private void LiftingGameManager_OnGameUnpaused(object sender, EventArgs e)
{
    Hide();
}
    private void Show()
    {
        gameObject.SetActive(true);
        LiftingGameManager.isWindowOpen = true;
    }
    private void Hide()
    {
        LiftingGameManager.isWindowOpen = false;
        gameObject.SetActive(false);

    }
}
