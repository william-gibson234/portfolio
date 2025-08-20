using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that controls anything that the Player could interact with, including Equipment, Doctor, and others
public class InteractableObject : MonoBehaviour
{
    //Class stores various information that a class that needs to be interacted with would need to store
    public enum State
    {
        Idle,
        Lifting,
        LiftFail,
        LiftSuccess,
    }
    public State state;

    public float progressTimerMax;

    public static event EventHandler<OnInteractThisActionEventArgs> OnInteractThisAction;
    public class OnInteractThisActionEventArgs
    {
        public Vector3 newCameraPosition;
    }

    public event EventHandler<OnTimerChangeEventArgs> OnTimerChange;
    public class OnTimerChangeEventArgs
    {
        public float timerNormalized;
    }
    public event EventHandler OnPlayerLeave;

    //This class is not instantiated so should not be interacted with
    public virtual void Interact(Player player)
    {
        Debug.LogError("InteractableObject was interacted with");
    }
    public virtual void InteractAndEnter(Player player)
    {
        Debug.LogError("InteractableObject was interacted/entered with");
    }
    public virtual void SetUp()
    {
        Debug.LogError("InteractableObject setup");
    }

    public void CallOnInteractThisAction(Vector3 inputNewCameraPosition)
    {
        OnInteractThisAction?.Invoke(this, new OnInteractThisActionEventArgs
        {
            newCameraPosition = inputNewCameraPosition
        });
    }


    public void CallOnTimerChange(float timer)
    {
        OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs
        {
            timerNormalized = timer / progressTimerMax
        });
    }
    public void CallOnPlayerLeave()
    {
        OnPlayerLeave?.Invoke(this, EventArgs.Empty);
    }
}
