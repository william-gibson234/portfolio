using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

//Class to manage the background music
public class MusicManager : MonoBehaviour
{
    [SerializeField] CompetitionSymbol cS;
    [SerializeField] CompetitionManager cM;
    [SerializeField] OptionsScreenUI oS;


    [SerializeField] LiftingGameManager lgM;

    AudioSource backgroundMusic;
    private void Awake()
    {
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;

        lgM.OnGamePaused += LiftingGameManager_OnGamePaused;
        lgM.OnGameUnpaused += LiftingGameManager_OnGameUnpaused;

    }
    private void LiftingGameManager_OnGamePaused(object sender, EventArgs e)
    {
        StopMusic();
    }
    private void LiftingGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        backgroundMusic.volume = oS.GetVolume()/10;
        if (oS.GetMusic())
        {
            StartMusic();
        }
    }
    private void Start()
    {
       backgroundMusic = GetComponent<AudioSource>();
    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        StartMusic();
    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        StopMusic();
    }
    private void StartMusic()
    {
        backgroundMusic.Play();
    }
    private void StopMusic()
    {
        backgroundMusic.Stop();
    }
}
