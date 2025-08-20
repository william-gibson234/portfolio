using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

//Class to control functionality of the UI element that displays the Player's energy and injury status
public class EnergyInjuryManagerUI : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] TextMeshProUGUI injuryText;
    [SerializeField] Player player;
    [SerializeField] ProteinCounter proteinCounter;

    [SerializeField] private CompetitionSymbol cS;
    [SerializeField] private CompetitionManager cM;

    private void Awake()
    {
        Show();

        player.OnEnergyChange += Player_OnEnergyChange;
        player.OnInjuryChange += Player_OnInjuryChange;
        proteinCounter.OnPlayerEat += ProteinCounter_OnPlayerEat;

        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart; 
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;
    }
    private void Update()
    {
        injuryText.text = "Injured: ";
        if (player.GetInjuryStatus())
        {
            injuryText.text += "Yes";
        }
        else
        {
            injuryText.text += "No";
        }

    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        Show();
    }

        private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        Hide();
    }
    private void Player_OnInjuryChange(object sender, Player.OnInjuryChangeEventArgs e)
    {
        //Update injured text whenever the Player;s energy status changes
        injuryText.text = "Injured: ";
        if (e.isPlayerInjured)
        {
            injuryText.text += "Yes";
        }
        else
        {
            injuryText.text += "No";
        }
    }

    private void ProteinCounter_OnPlayerEat(object sender, EventArgs e)
    {
        bar.fillAmount = 1f;
    }
    private void Player_OnEnergyChange(object sender, Player.OnEnergyChangeEventArgs e)
    {
        bar.fillAmount = e.energyNormalized;
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
