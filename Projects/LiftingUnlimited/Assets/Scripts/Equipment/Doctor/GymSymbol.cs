using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class controlling behavior of the GymSymbol, which the Player Interacts with to travel back to the Gym from the Doctor's office
public class GymSymbol : InteractableObject
{
    private Player player;

    private Vector3 GymPlayerPosition = new Vector3(0, 0, 0);

    private Vector3 GymCameraPosition = new Vector3(0, 30, -20);

    //Functions controlling interactions
    public override void Interact(Player currPlayer)
    {
        InteractThis(currPlayer);
    }
    public override void InteractAndEnter(Player currPlayer)
    {
        InteractThis(currPlayer);
    }

    //Function to run if Player does either Interact method with GymSymbol, sets up the environment to return to the Gym
    public void InteractThis(Player currPlayer)
    {
        if (!currPlayer.IsLifting() && !currPlayer.GetInjuryStatus())
        {
            player = currPlayer;
            player.SetIsWalking(false);

            SetUp();
            CallOnInteractThisAction(GymCameraPosition);
        }
    }

    //Function to change the player's position to the Gym
    public override void SetUp()
    {
        player.transform.position = GymPlayerPosition;
    }
}
