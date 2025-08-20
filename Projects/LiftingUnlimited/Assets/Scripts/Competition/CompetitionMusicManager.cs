using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages the music during a competition
public class CompetitionMusicManager : MonoBehaviour
{
    [SerializeField] CompetitionSymbol cS;
    [SerializeField] CompetitionManager cM;

    //Audio file of the background music during a competition
    AudioSource backgroundMusic;

    private void Awake()
    {
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;
    }
    private void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
    }

    //Event Listener to stop the music when a competition finishes
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        StopMusic();
    }

    //Event Listener to start the music when a competition starts
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        StartMusic();
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
