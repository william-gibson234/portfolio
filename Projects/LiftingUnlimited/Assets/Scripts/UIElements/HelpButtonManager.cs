using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to control functionality of the HelpButton
public class HelpButtonManager : MonoBehaviour
{
    public event EventHandler OnHelpButtonClicked;

    public void OnHelpButtonPress()
    {
        if(!LiftingGameManager.isCompeting)
        OnHelpButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
