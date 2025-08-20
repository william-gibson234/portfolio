using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Parent class of all Equipment the player uses
//Has every event, container function, has the information about the current Lift
//Equipment class is never instantiated
public class Equipment : InteractableObject
{
    public struct Lift
    {
        public string name;
        public float weight;
        public bool success;
        public bool compete;
    }

    public static event EventHandler<OnAnyLiftCompleteEventArgs> OnAnyLiftComplete;
    public class OnAnyLiftCompleteEventArgs
    {
        public Lift lift;
    }

    public static event EventHandler PlayerCheckInjury;

    //Event triggers when Player's progress through the current lift changes
    public event EventHandler<OnProgressChangeEventArgs> OnProgressChange;
    public class OnProgressChangeEventArgs
    {
        public float progressNormalized;
    }

    public static event EventHandler<OnAnyLiftStartEventArgs> OnAnyLiftStart;
    public class OnAnyLiftStartEventArgs
    {
        public string liftName;
    }

    //Variables each subclass should have and utilize
    [SerializeField] public PlayerLiftingAnimator playerLiftingAnimator;

    [SerializeField] public Transform playerHoldPoint;

   protected float progressAmountMax = 6;

    [SerializeField] public string equipmentName;

    [SerializeField] public Barbell barbell;

    [SerializeField] public PlayerInputField inputField;

    [SerializeField] private SoundManager sM;

    public bool hasPlayer;


    public Vector3 playerLastPosition;

    //Next 4 functions should never be called, since Equipment is a parent class that should never get Instantiated, so the log errors if for some reason they are called
    public override void Interact(Player player)
    {
        Debug.LogError("Base Equipment was interacted with");
    }
    public override void InteractAndEnter(Player player)
    {
        Debug.LogError("Base Equipment was interacted/entered with");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.LogError("Base Equipment was interacted alternate with");
    }
    public override void SetUp()
    {

        Debug.LogError("Base Equipment was setup");
    }


    public void CallOnProgressChange(float progress)
    {
        OnProgressChange?.Invoke(this, new OnProgressChangeEventArgs
        {
            progressNormalized = progress / progressAmountMax
        });
    }

    public bool HasPlayer()
    {
        return hasPlayer;
    }

    public void CallOnAnyLiftComplete(float weight, string name, bool success, bool compete)
    {
        sM.OnLiftComplete();
        Lift l = new Lift();

        l.weight = weight;
        l.name = name;
        l.success = success;
        l.compete = compete;

        OnAnyLiftComplete?.Invoke(this, new OnAnyLiftCompleteEventArgs
        {
            lift = l
        });
        PlayerCheckInjury?.Invoke(this, EventArgs.Empty);
    }

    public void CallOnAnyLiftStart(string lN)
    {
       OnAnyLiftStart?.Invoke(this, new OnAnyLiftStartEventArgs
        {
            liftName = lN
        });
    }
}
