using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to control functionality of the Deadlift equipment
public class DeadliftPlatform : Equipment
{
    private float deadliftMaxHeight = 5f;

    private float barbellStartHeight = 2.5f;

    private float timer;
    private float liftingProgress=0f;

    Player player;

    private float liftingDecrease;

    [SerializeField] ProgressBarUI timerBarUI;
    [SerializeField] ProgressBarUI progressBarUI;

    [SerializeField] private bool competition;

    private void Awake()
    {
        inputField.OnLiftTextEntered += InputField_OnLiftTextEntered;
    }

    private void Start()
    {
        state= State.Idle;
        timer = 0;
        liftingProgress = 0;
    }

    private void Update()
    {
        //While the player is lifting, update the animator, and control the lifting progress to change lift difficulty
        if (state == State.Lifting)
        {
            playerLiftingAnimator.DeadliftAnimator(barbell.transform.position.y);

            if (liftingProgress > 0)
            {
                liftingProgress -= liftingDecrease;
            }
            if(liftingProgress < 0)
            {
                liftingProgress = 0;
            }

            CallOnProgressChange(liftingProgress);

            //When the player has finished the lift, set up environment variables for the Player finishing with their lift so Player can lift at other Equipment
            //This conditional checks if the Player "succeeded" at the lift
            if (liftingProgress >= progressAmountMax)
            {
                state = State.LiftSuccess;
                player.SetIsLifting(false);
                player.transform.position = playerLastPosition;
                liftingProgress = 0;
                CallOnPlayerLeave();

                LiftingGameManager.isLifting = false;

                timerBarUI.SetPlayerLeave(true);
                progressBarUI.SetPlayerLeave(true);

                if (player != null)
                {
                    player.IncrementDeadliftCount();
                }
                if (player.GetDeadliftProgression() < 100)
                {
                    if (-0.0049f * player.GetDeadliftProgression() - 0.000269972f * barbell.GetWeight() + 0.549945f > 0.0009f * player.GetDeadliftProgression() + 0.0000495868f * barbell.GetWeight() - 0.0891736f)
                    {
                        player.SetDeadliftProgression(player.GetDeadliftProgression() + (-0.0049f * player.GetDeadliftProgression() - 0.000269972f * barbell.GetWeight() + 0.549945f) * player.GetEnergy());
                    }
                    else
                    {
                        player.SetDeadliftProgression(player.GetDeadliftProgression() + (0.0009f * player.GetDeadliftProgression() + 0.0000495868f * barbell.GetWeight() - 0.0891736f) * player.GetEnergy());
                    }
                    if ((player.GetDeadliftProgression() > 100))
                    {
                        player.SetDeadliftProgression(100);
                    }
                }
                playerLiftingAnimator.Reset();
                player = null;

                CallOnAnyLiftComplete(barbell.GetWeight(), "Deadlift", true, competition);

                
            }

            //This conditional checks if the Player "failed" the lift
            if(timer >= progressTimerMax)
            {
                state = State.LiftFail;
                player.SetIsLifting(false);
                player.transform.position = playerLastPosition;
                liftingProgress = 0;
                CallOnPlayerLeave();
                timerBarUI.SetPlayerLeave(true);
                progressBarUI.SetPlayerLeave(true);
                LiftingGameManager.isLifting = false;

                playerLiftingAnimator.Reset();
                player = null;
                CallOnAnyLiftComplete(barbell.GetWeight(), "Deadlift", false, competition);
            }
            timer += Time.deltaTime;

            CallOnTimerChange(timer);
        }
        Vector3 oldBarbellPosition = barbell.transform.position;
        barbell.transform.position = new Vector3(oldBarbellPosition.x, liftingProgress / progressAmountMax * deadliftMaxHeight + barbellStartHeight, oldBarbellPosition.z);
    }

    //Function to control interaction, beginning the setup process for the lift
    public override void Interact(Player currPlayer)
    {
        if (!currPlayer.IsLifting())
        {
            player = currPlayer;
            player.SetIsLifting(true);
            player.SetIsWalking(false);
            SetUpLift();
        }
    }

    //Function to control interaction while changing the weight
    public override void InteractAndEnter(Player currPlayer)
    {
        if (!currPlayer.IsLifting())
        {
            player = currPlayer;
            player.SetIsLifting(true);
            player.SetIsWalking(false);
            CallOnAnyLiftStart("Deadlift");
        }
    }

    //Function controlling interactions while the lift is going on
    public override void InteractAlternate(Player player)
    {
        if (state == State.Lifting)
        {
            liftingProgress += 1f;

            CallOnProgressChange(liftingProgress);
        }
    }

    //Function controlling the set up of the environemnt variables for the lift
    public void SetUpLift()
    {

        LiftingGameManager.isLifting = true;
        timerBarUI.SetPlayerLeave(false);
        progressBarUI.SetPlayerLeave(false);

        playerLastPosition = player.transform.position;

        player.transform.position = playerHoldPoint.position;

        liftingProgress = 0;
        timer = 0;
        state = State.Lifting;

        liftingDecrease = 0.0000970873786408f * (barbell.GetWeight() - 700f) + .125f;
        if (barbell.GetWeight() > 700)
        {
            liftingDecrease = 0.125f;
        }
        progressTimerMax = 3.5f + player.GetDeadliftProgression() * .13f;
    }

    //Event Listener to trigger when Player enters in new weight
    public void InputField_OnLiftTextEntered(object sender, PlayerInputField.OnLiftTextEnteredEventArgs e)
    {
        if (player != null)
        {
            barbell.SetWeight(e.weight);
            SetUpLift();
        }
    }
}
