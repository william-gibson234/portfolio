using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Displays poster on the wall that identifies the correct competition
public class CompetitionPosterManager : MonoBehaviour
{
    [SerializeField] private PlayerCompDropdown pcD;
    [SerializeField] private GameObject[] posters;

    private void Awake()
    {
        pcD.OnCorrectMeetEntered += PlayerCompDropdown_OnCorrectMeetEntered;

        ResetPosters();
    }

    //Event triggers when the Player enters the Correct Meet and a new Competition starts
    //PlayerCompDropdown.OnCorrectMeetEnteredEventArgs e contains a struct OnCorrectMeetEnteredEventArgsStruct, which contains an int for MeetType, 1 - 4 
    //1 stands for local meet, 2 for state meet, 3 for national meet, 4 for international meet
    private void PlayerCompDropdown_OnCorrectMeetEntered(object sender, PlayerCompDropdown.OnCorrectMeetEnteredEventArgs e)
    {
        ResetPosters();

        //selects correct poster from list based on the meet type
        posters[e.oMM.meetType-1].gameObject.SetActive(true);
    }

    //function to change every poster to not visible in order to display only the one correct poster
    private void ResetPosters()
    {
        for (int i = 0; i < 4; i++)
        {
            posters[i].gameObject.SetActive(false);
        }
    }
}
