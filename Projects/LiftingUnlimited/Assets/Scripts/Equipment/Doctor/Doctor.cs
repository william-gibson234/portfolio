using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class controlling behavior of the Doctor for when Player is "injured"
public class Doctor : InteractableObject
{
    private Player player;
    private float timer;

    [SerializeField] ProgressBarUI timerBarUI;

    private void Start()
    {
        //Setup Local Variables
        progressTimerMax = 20f;
        timer = 0;

        state = State.Idle;
    }

    private void Update()
    {
        if (state == State.Lifting)
        {
            //Incremenet the timer
            timer += Time.deltaTime;

            //Alerting the visual that the timer has changed
            CallOnTimerChange(timer);

            if (timer >= progressTimerMax)
            {
                //Timer has finished, reset to state before the player interacted with Doctor
                player.SetIsLifting(false);

                timerBarUI.SetPlayerLeave(false);

                CallOnPlayerLeave();

                state = State.Idle;

                player.SetInjuryStatus(false);

                LiftingGameManager.isLifting = false;
            }
        }
    }

    //Functions to control the Player interacting with the Doctor
    public override void Interact(Player currPlayer)
    {
        InteractThis(currPlayer);
    }
    public override void InteractAndEnter(Player currPlayer)
    {
        InteractThis(currPlayer);
    }

    //Function to run if Player does either Interact method with Doctor, sets up the environment to begin a Doctor's visit
    private void InteractThis(Player currPlayer)
    {
        LiftingGameManager.isLifting = true;
        player = currPlayer;

        player.SetIsLifting(true);
        player.SetIsWalking(false);

        timerBarUI.SetPlayerLeave(false);

        timer = 0;

        state = State.Lifting;


    }
}
