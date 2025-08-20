using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class controlling the behavior of the DoctorSymbol, which the Player interacts with the take them to the Doctor's office
public class DoctorsSymbol : InteractableObject
{
    private Player player;

    private Vector3 DoctorPlayerPosition = new Vector3(100, 0, 0);

    private Vector3 DoctorCameraPosition = new Vector3(100, 30, -20);

    //Functions to control interactions
    public override void Interact(Player currPlayer)
    {
        InteractThis(currPlayer);
    }
    public override void InteractAndEnter(Player currPlayer)
    {
        InteractThis(currPlayer);
    }

    //Function to run if Player does either Interact method with DoctorSymbol, sets up environment for Doctor's office
    public void InteractThis(Player currPlayer)
    {
        if (!currPlayer.IsLifting() && currPlayer.GetInjuryStatus())
        {
            player = currPlayer;
            player.SetIsWalking(false);

            SetUp();
            CallOnInteractThisAction(DoctorCameraPosition);
        }
    }

    //Function to change the player's position to the Doctor's office
    public override void SetUp()
    {
        player.transform.position = DoctorPlayerPosition;
    }
}
