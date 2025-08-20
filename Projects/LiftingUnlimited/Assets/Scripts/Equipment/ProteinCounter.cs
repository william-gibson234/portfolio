using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Class to control functionality of the ProteinCounter, which Player interacts with to get more Energy
public class ProteinCounter : InteractableObject
{
    public event EventHandler OnPlayerEat;

    [SerializeField] SoundManager sM;

    public override void Interact(Player currPlayer)
    {
        currPlayer.SetEnergyToOne();
        sM.OnPlayerDrink();
        OnPlayerEat?.Invoke(this, EventArgs.Empty);
    }
    public override void InteractAndEnter(Player currPlayer)
    {
        currPlayer.SetEnergyToOne();
        sM.OnPlayerDrink();
        OnPlayerEat?.Invoke(this, EventArgs.Empty);
    }
}
