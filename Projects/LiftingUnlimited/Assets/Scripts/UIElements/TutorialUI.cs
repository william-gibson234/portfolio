using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to control the UI element that displays the Tutorial UI element when Player starts a new game to teach the user the controls
public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private NewPauseGameUI nPGUI;

    [SerializeField] private SoundManager sM;
    private void Awake()
    {

        closeButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Hide();
        });

        nPGUI.OnGameButtonClick += NewPauseGameUI_OnGameButtonClick;
        Hide();
    }

    //Event Listener to trigger when a new game is started, which should display this UI
    private void NewPauseGameUI_OnGameButtonClick(object sender, NewPauseGameUI.OnGameButtonClickedEventArgs e)
    {
        if (e.newGame)
        {
            Show();
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        LiftingGameManager.isWindowOpen = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        LiftingGameManager.isWindowOpen = false;
    }
}
