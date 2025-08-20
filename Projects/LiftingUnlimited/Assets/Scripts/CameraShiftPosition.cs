using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to shift the Cameras position when Player goes to Doctor or Gym, which required the camera to shift
public class CameraShiftPosition : MonoBehaviour
{
    [SerializeField] private DoctorsSymbol doctorsSymbol;
    [SerializeField] private CompetitionManager cM;

    [SerializeField] private PauseScreenUI pSUI;

    private void Awake()
    {
        InteractableObject.OnInteractThisAction += InteractableObject_OnInteractThisAction;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;

        pSUI.OnMainMenuOpen += PauseScreenUI_OnMainMenuOpen;
    }
    private void PauseScreenUI_OnMainMenuOpen(object sender, EventArgs e)
    {
        InteractableObject.OnInteractThisAction -= InteractableObject_OnInteractThisAction;
        cM.OnCompetitionFinish -= CompetitionManager_OnCompetitionFinish;

    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {

        this.transform.position = e.newCamPosition;
    }
    private void InteractableObject_OnInteractThisAction(object sender, InteractableObject.OnInteractThisActionEventArgs e)
    {
        this.transform.position = e.newCameraPosition;
    }
}
