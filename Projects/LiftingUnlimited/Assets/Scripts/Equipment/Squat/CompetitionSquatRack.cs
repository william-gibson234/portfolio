using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class controlling behavior of a Squat Rack, specifically during a Competition
public class CompetitionSquatRack : SquatRack
{

    private void Awake()
    {
        pinpointBarUI.OnProgressStopped += PinpointBarUI_OnProgressStopped;
        pinpointBarUI.OnBarProgressChanged += PinpointBarUI_OnBarProgressChanged;
        pinpointBarUI.OnStoppedLifting += PinpointBarUI_OnStoppedLifting;

        inputField.OnLiftTextEntered += InputField_OnLiftTextEntered;
    }

    private void Start()
    {
        barbellOriginalPosition = barbell.transform.position;
        state = State.Idle;

        barbellSpeed = 0.005f;
    }
    private void Update()
    {
        //While Player is Lifting, activate the animator, set the barbell speed correctly
        if (state == State.Lifting)
        {
            playerLiftingAnimator.SquatAnimator(barbell.transform.position.y);
            barbellSpeed = pinpointBarUI.GetBarSpeed() * 0.00035f;
        }
    }

    //Event Listener to trigger when the lift finishes, reset the environemnt variables, determine if the Lift was a success or failure
    public override void PinpointBarUI_OnStoppedLifting(object sender, EventArgs e)
    {
        playerLiftingAnimator.Reset();
        player.SetIsLifting(false);

        player.transform.position = playerLastPosition;
        state = State.Idle;

        barbell.transform.position = barbellOriginalPosition;

        if (playerPercentageCorrect > playerPercentageNecessary)
        {
            state = State.LiftSuccess;

            float squatProgression = player.GetSquatProgression();

            if (-0.000358974f * barbell.GetWeight() - 0.0049f * squatProgression + 0.548462f > 0.0000659341f * barbell.GetWeight() + 0.0009f * squatProgression - 0.0889011f)
            {
                player.SetSquatProgression(player.GetSquatProgression() + (-0.000358974f * barbell.GetWeight() - 0.0049f * squatProgression + 0.548462f) * player.GetEnergy());
            }
            else
            {
                player.SetSquatProgression(player.GetSquatProgression() + (0.0000659341f * barbell.GetWeight() + 0.0009f * squatProgression - 0.0889011f) * player.GetEnergy());
            }

            if (player.GetSquatProgression() > 100)
            {
                player.SetSquatProgression(100f);
            }

            if (player != null)
            {
                player.IncrementSquatCount();
            }
        }
        else
        {
            state = State.LiftFail;
        }

        player = null;
        CallOnAnyLiftComplete(barbell.GetWeight(), "Squat", playerPercentageCorrect > playerPercentageNecessary, true);
        LiftingGameManager.isLifting = false;


    }

    //Event Listener to trigger when the progress on the Progress bar changes
    public override void PinpointBarUI_OnBarProgressChanged(object sender, PinpointBarUI.OnBarProgressChangedEventArgs e)
    {
        barbell.transform.position = new Vector3(barbell.transform.position.x, barbell.transform.position.y - pinpointBarUI.GetBarDirection() * e.barProgress * barbellSpeed, barbell.transform.position.z);
    }

    //Event Listener to trigger when the progress has stopped, meaning the Player has reached the full extension of the Squat
    public override void PinpointBarUI_OnProgressStopped(object sender, PinpointBarUI.OnProgressStoppedEventArgs e)
    {
        playerPercentageCorrect = e.percentFromPerfect;

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

            CallOnAnyLiftStart("Squat Rack");
        }


    }

    //Function controlling the set up of the environemnt variables for the lift
    public override void SetUpLift()
    {
        LiftingGameManager.isLifting = true;
        playerLastPosition = player.transform.position;

        player.transform.position = playerHoldPoint.position;

        state = State.Lifting;

        CallOnStartProgress();

        barbell.transform.position = new Vector3(barbell.transform.position.x, barbell.transform.position.y, player.transform.position.z - 1f);

        playerPercentageNecessary = 0.98f - 0.045f * (float)Math.Exp(-1f * (barbell.GetWeight() - 135f) / 400f);
    }

    //Function controlling interactions while the lift is going on
    public override void InteractAlternate(Player player)
    {
        if (state == State.Lifting)
        {
            CallOnStopProgress();
        }
    }

    public override Player GetPlayer()
    {
        return player;
    }

    //Event Listener to trigger when Player enters in new weight
    public override void InputField_OnLiftTextEntered(object sender, PlayerInputField.OnLiftTextEnteredEventArgs e)
    {
        if (player != null)
        {
            barbell.SetWeight(e.weight);
            SetUpLift();
        }
    }
}
