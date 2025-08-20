using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to control Visual Appearance of the Bench Press
public class BenchPressVisual : MonoBehaviour
{
    public event EventHandler OnBarVisualStopped;

    [SerializeField] private Barbell barbell;
    [SerializeField] private BenchPress benchPress;
    [SerializeField] private CircleProgressBarUI circleProgressBarUI;

    private float barYMax;
    private bool isMoving;
    private int barDir;

    private float barSpeed;
    private const float BAR_Y_MIN = 8.5f;

    //State Machine to know where Player is at in Bench Press process
    public enum State 
    {
        Idle,
        LiftBeforeOne,
        LiftBeforeTwo,
        LiftBeforeThree,
        LiftAfterThree,
    }
    public State BenchVisualState;

    private void Awake()
    {
        benchPress.OnBenchProgressStarted += BenchPress_OnBenchProgressStarted;
    }

    private void Start()
    {
        //Setup local variables
        isMoving = false;
        barDir = -1;
        barYMax = barbell.transform.position.y;
        BenchVisualState = State.Idle;

    }
    private void Update()
    {
        if (isMoving)
        {
            barSpeed = circleProgressBarUI.GetCircleSpeed();
            if (barbell.transform.position.y < BAR_Y_MIN)
            {
                //Switches direction at bottom of lift
                barDir =1;
            }
            if (barbell.transform.position.y <= barYMax)
            {
                //Change the Player's position based on the variables
                barbell.transform.position = new Vector3(barbell.transform.position.x, barbell.transform.position.y + barSpeed * barDir, barbell.transform.position.z);
            }
            else
            {
                //Ensures barbell does not go above the maximum
                barbell.transform.position = new Vector3(barbell.transform.position.x, barYMax, barbell.transform.position.z);
            }
            if(barbell.transform.position.y == barYMax)
            {
                //Barbell has returned to original position, Player has finished the lift
                isMoving = false;
                OnBarVisualStopped?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    //Event Listener to trigger when the player begins a Bench Press
    private void BenchPress_OnBenchProgressStarted(object sender, EventArgs e)
    {
        isMoving = true;

        barDir =-1;
    }
}
