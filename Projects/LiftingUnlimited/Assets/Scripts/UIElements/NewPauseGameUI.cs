using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Class to control functionality of the screen that displays at game start, where user can select to start a new game or continue a game
public class NewPauseGameUI : MonoBehaviour
{
    public event EventHandler<OnGameButtonClickedEventArgs> OnGameButtonClick;
    public class OnGameButtonClickedEventArgs
    {
        public bool newGame;
    }

    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueGameButton;
    [SerializeField] private SoundManager sM;



    private bool isThisWindowOpen;

    [SerializeField] private DataPersistenceManager dataPersistenceManager;
    private void Awake()
    {
        isThisWindowOpen = true;
        NewGameButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            OnGameButtonClick?.Invoke(this, new OnGameButtonClickedEventArgs
            {
                newGame = true
            });
            LiftingGameManager.isWindowOpen = false;
            isThisWindowOpen = false;
            Hide();
        });
        ContinueGameButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            OnGameButtonClick?.Invoke(this, new OnGameButtonClickedEventArgs
                {
                    newGame = false
                });
                LiftingGameManager.isWindowOpen = false;
            isThisWindowOpen = false;
            Hide();
        });
        Show();
        LiftingGameManager.isWindowOpen = true;
    }
    private void Update()
    {
        if (isThisWindowOpen)
        {
            LiftingGameManager.isWindowOpen = true;

        }
    }
    public void Hide()
    {
        LiftingGameManager.isInNewPauseGameUI = false;
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        LiftingGameManager.isInNewPauseGameUI = true;
    }
}
