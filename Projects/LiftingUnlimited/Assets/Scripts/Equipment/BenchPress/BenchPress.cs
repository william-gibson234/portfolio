using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Child class of Equipment, handles all functionality of the Bench Press 
public class BenchPress : Equipment
{

    public event EventHandler OnBenchProgressStopped;
    public event EventHandler OnBenchProgressStarted;

    [SerializeField] CircleProgressBarUI circleProgressBarUI;
    [SerializeField] BenchPressVisual benchPressVisual;
    [SerializeField] private PauseScreenUI pSUI;

    public Player player;
    public float xRotate = 90f;
    public bool visualDoneMoving = false;
    public bool playerDoneLifting = false;

    public float averageScore;

    public float averageScoreNecessary;

    public float benchProgression;

    [SerializeField] bool isCompetition;

    private void Awake()
    {

        inputField.OnLiftTextEntered += InputField_OnLiftTextEntered;
        circleProgressBarUI.OnPlayerDoneBench += CircleProgressBarUI_OnPlayerDoneBench;
        benchPressVisual.OnBarVisualStopped += BenchPressVisual_OnBarVisualStopped;


        pSUI.OnMainMenuOpen += PauseScreenUI_OnMainMenuOpen;
    }
    private void PauseScreenUI_OnMainMenuOpen(object sender, EventArgs e)
    {
        inputField.OnLiftTextEntered -= InputField_OnLiftTextEntered;
        circleProgressBarUI.OnPlayerDoneBench -= CircleProgressBarUI_OnPlayerDoneBench;
        benchPressVisual.OnBarVisualStopped -= BenchPressVisual_OnBarVisualStopped;
    }
    private void Start()
    {
        averageScoreNecessary = 0.9f;
    }

    private void Update()
    {
        //Utilizes a state machine to determine where the Player is in their Bench Press process
        if(state == State.Lifting)
        {
            //sends the current barbell position to the animator
            playerLiftingAnimator.BenchAnimator(barbell.transform.position.y);
        }
        if((visualDoneMoving&& playerDoneLifting&&state!= State.Idle)||(visualDoneMoving&&circleProgressBarUI.GetCount()==1))
        {
            //conditions satisfied meaning the player is finished lifting
            LiftingGameManager.isLifting = false;
            if ((visualDoneMoving && circleProgressBarUI.GetCount() == 1))
            {
                circleProgressBarUI.SetIsCircleClosing(false);
            }

            //Reset player position
            player.transform.position = playerLastPosition;
            player.SetIsLifting(false);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            
            //This condition indicates that the player "succeeded" at the lift
            if (averageScore > averageScoreNecessary)
            {
                //Mathematical formulas to determine rate of progression to model real weightlifting progression
                if ((0.0009818655f * barbell.GetWeight() + 0.0088858832f * benchProgression - 1f) / -1.8134455465f > (0.0011117974f * barbell.GetWeight() + 0.0100617665f * benchProgression - 1f) / 11.1797405806f)
                {
                    player.SetBenchProgression(player.GetBenchProgression()+((0.0009818655f * barbell.GetWeight() + 0.0088858832f * benchProgression - 1f) / -1.8134455465f)*player.GetEnergy());
                }
                else
                {
                    player.SetBenchProgression(player.GetBenchProgression() + ((0.0011117974f * barbell.GetWeight() + 0.0100617665f * benchProgression - 1f) / 11.1797405806f)*player.GetEnergy());
                }
                if ((player.GetBenchProgression() > 100))
                {
                    player.SetBenchProgression(100);
                }
                state = State.LiftSuccess;

                if (player != null)
                {
                    player.IncrementBenchCount();
                }
            }
            else
            {
                state = State.LiftFail;
            }

            //Resetting all variables/visuals to before the lift started
            visualDoneMoving = false;
            playerDoneLifting = false;
            playerLiftingAnimator.Reset();
            state = State.Idle;
            player = null;
            CallOnAnyLiftComplete(barbell.GetWeight(), "Bench Press", averageScore > averageScoreNecessary, isCompetition);
        }
    }

    //Event Listener to Hold Player on the Bench Press until the visual is finished moving so Player does not exit bench press in the middle of an animation
    public virtual void BenchPressVisual_OnBarVisualStopped(object sender, EventArgs e)
    {
        visualDoneMoving = true;
    }

    //Event Listener to trigger when UI Element controlling lift finishes, and allows the player to exit the lift
    public virtual void CircleProgressBarUI_OnPlayerDoneBench(object sender, CircleProgressBarUI.OnPlayerDoneBenchEventArgs e)
    {
        playerDoneLifting = true;
        averageScore = e.averageScore;
        
    }

    //When the Player interacts with the Bench Press to start a lift
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

    //When the Player interacts with the Bench Press to start a lift and change the weight via text input
    public override void InteractAndEnter(Player currPlayer)
    {
        if (!currPlayer.IsLifting())
        {
            player = currPlayer;

            player.SetIsLifting(true);
            player.SetIsWalking(false);
            CallOnAnyLiftStart("Bench Press");
        }
    }

    //While the Player is lifting and needs to interact with the bench press
    public override void InteractAlternate(Player player)
    {
        if (state == State.Lifting)
        {
            OnBenchProgressStopped?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual Player GetPlayer()
    {
        return player;
    }

    //Function to set variables and environment
    public virtual void SetUpLift()
    {

        LiftingGameManager.isLifting = true;
        playerLastPosition = player.transform.position;

        player.transform.position = playerHoldPoint.position;
        player.transform.eulerAngles = new Vector3(xRotate, 0, 0);

        state = State.Lifting;

        OnBenchProgressStarted?.Invoke(this, EventArgs.Empty);

        visualDoneMoving = false;
        playerDoneLifting = false;


        averageScoreNecessary = 0.94f - 0.07f * (float)Math.Exp(-1f * ((barbell.GetWeight() - 95f) / 100f));
    }

    //Event Listener to trigger when the player enters a new weight
    public virtual void InputField_OnLiftTextEntered(object sender, PlayerInputField.OnLiftTextEnteredEventArgs e)
    {
        if (player != null)
        {
            barbell.SetWeight(e.weight);
            SetUpLift();
        }
    }
}
