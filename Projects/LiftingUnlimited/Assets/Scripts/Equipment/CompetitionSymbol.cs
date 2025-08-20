using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to control functionality of the CompetitionSymbol that the Player interacts with to start a Competition
public class CompetitionSymbol : InteractableObject
{
    [SerializeField] LiftingManagerUI lM;

    public event EventHandler<OnCompetitionStartEventArgs> OnCompetitionStart;
    public class OnCompetitionStartEventArgs{
        public Player thisPlayer;
    }

    private Player player;

    private Vector3 CompetitionPlayerPosition = new Vector3(-100, 0, 40);

    private Vector3 CompetitionCameraPosition = new Vector3(-100, 30, -20);

    //Both Interact methods call the same function
    public override void Interact(Player currPlayer)
    {
        InteractThis(currPlayer);
    }
    public override void InteractAndEnter(Player currPlayer)
    {
        InteractThis(currPlayer);
    }

    //Function to run whenever interacted with. Sets up parts of competition environment and calls methods and triggers events for rest of environment setup to occur
    public void InteractThis(Player currPlayer)
    {
        if (!currPlayer.IsLifting() && !currPlayer.GetInjuryStatus()&&lM.GetBestTotal()>=1000&&!LiftingGameManager.isWindowOpen)
        {
            if (lM.GetBestSquat() > 0&&lM.GetBestBench()>0&&lM.GetBestDeadlift()>0)
            {
                player = currPlayer;
                player.SetIsWalking(false);
                
                SetUp();
                CallOnInteractThisAction(CompetitionCameraPosition);

                LiftingGameManager.isCompeting = true;

                OnCompetitionStart?.Invoke(this, new OnCompetitionStartEventArgs
                {
                    thisPlayer = currPlayer
                });
            }

        }
    }

    //Function to set up the Player's position in the competition
    public override void SetUp()
    {
        player.transform.position = CompetitionPlayerPosition;
    }
}
