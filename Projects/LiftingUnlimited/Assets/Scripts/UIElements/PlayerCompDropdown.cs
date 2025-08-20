using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Class to control the UI Element that appears immediately when Player starts competition, prompting Player to select what Type of Competition to Enter
public class PlayerCompDropdown : MonoBehaviour
{
    public event EventHandler<OnCorrectMeetEnteredEventArgs> OnCorrectMeetEntered;
    public class OnCorrectMeetEnteredEventArgs
    {
        public OnCorrectMeetEnteredEventArgsStruct oMM;
    }

    public struct OnCorrectMeetEnteredEventArgsStruct
    {
        public int meetType;
        public Player meetPlayer;
    }

    [SerializeField] LiftingManagerUI liftingManagerUI;
    [SerializeField] TextMeshProUGUI errorText;

    [SerializeField] TMP_Dropdown dropdown;

    [SerializeField] CompetitionSymbol cS;

    [SerializeField] private SoundManager sM;

    private Player p;

    public void HandleInputData(int val)
    {
        if (val != 0)
        {
            sM.OnButtonClick();
            CheckInput(val);
        }
    }
    
    private void Awake()
    {
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
    }
    private void Start()
    {
        Hide(); 
    }

    //Event Listener to handle when the Player starts a Competition by interacting with the CompetitionSymbol
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
            p = e.thisPlayer;

            dropdown.value = 0;

            Show();
    }

    //Function to determine whether or not Player is "eligibile" (has the required powerlifting total) to compete in a certain type of competition
    private void CheckInput(int val)
    {
        float playerTotal = liftingManagerUI.GetBestTotal();
        int MeetType = val;
        bool canCompete  =true;
        if(val == 1)
        {
            if(playerTotal < 1000)
            {
                canCompete = false;
                errorText.text = "Too inexperienced for Local Meet";
            }
        }
        if (val == 2)
        {
            if (playerTotal < 1500)
            {
                canCompete = false;
                errorText.text = "Too inexperienced for State Meet";
            }

        }
        if (val == 3)
        {
            if (playerTotal < 2000)
            {
                canCompete = false;
                errorText.text = "Too inexperienced for National Meet";
            }
        }
        if (val == 4)
        {
            if (playerTotal < 2500)
            {
                canCompete = false;
                errorText.text = "Too inexperienced for International Meet";
            }

        }
        if (canCompete)
        {

            //Player is "eligible" to compete in the level of Competition they selected, so start the Competition
            Hide();
            OnCorrectMeetEnteredEventArgsStruct onCorrectMeetEnteredEventArgsStruct = new OnCorrectMeetEnteredEventArgsStruct();
            onCorrectMeetEnteredEventArgsStruct.meetPlayer = p;
            onCorrectMeetEnteredEventArgsStruct.meetType = MeetType;
            OnCorrectMeetEntered?.Invoke(this, new OnCorrectMeetEnteredEventArgs
            {
                oMM = onCorrectMeetEnteredEventArgsStruct
            });
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
