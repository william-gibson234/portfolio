using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//Class to control the UI functionality of the Help Screen
public class HelpPageUI : MonoBehaviour
{
    [SerializeField] HelpButtonManager helpButtonManager;
    [SerializeField] Button openWindowButton;

    [SerializeField] CompetitionSymbol cS;
    [SerializeField] CompetitionManager cM;
    [SerializeField] private SoundManager sM;

    private void Awake()
    {
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;
    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        //Make sure that user can open the Help Page after the competition ends
        openWindowButton.gameObject.SetActive(true);
    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        //Make sure that user cannot open the Help Page after the competition starts
        openWindowButton.gameObject.SetActive(false);
    }
    private void Start()
    {
        Hide();
    }

    public void OnKeybindButtonClicked()
    {
        Show();
        sM.OnButtonClick();
    }

    public void OnCloseButtonPress()
    {
        Hide();
        sM.OnButtonClick();
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
