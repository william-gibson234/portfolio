using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

//Class to control the functionality of the UI element that displays above the Equipment during a lift and alerts the Player to their current Progress
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] InteractableObject equipment;
    [SerializeField] Image barImage;
    [SerializeField] Type type;
    [SerializeField] private TextMeshProUGUI barNameText;

    Equipment selectedEquipment;

    bool playerLeave;

    private enum Type
    {
        Timer,
        Progress,
    }

    private void Awake()
    {
        equipment.OnTimerChange += Equipment_OnTimerChange;
        equipment.OnPlayerLeave += Equipment_OnPlayerLeave;
        if (equipment is Equipment)
        {
            Equipment newEquipment = (Equipment)equipment;
            newEquipment.OnProgressChange += Equipment_OnProgressChange;
        }

        barImage.fillAmount = 0f;
        Hide();
    }
    private void Start()
    {
        playerLeave = false;

        if (type == Type.Timer)
        {
            barNameText.text = "Timer";
        }
        else if(type == Type.Progress)
        {
            barNameText.text = "Progress";
        }
    }

    private void Equipment_OnPlayerLeave(object sender, EventArgs e)
    {
        playerLeave = true;
        Hide();
    }

    private void Equipment_OnTimerChange(object sender, Equipment.OnTimerChangeEventArgs e)
    {
        if (type == Type.Timer)
        {
            //Updates the fill amount based on the timer's progress
            barImage.fillAmount = e.timerNormalized;
            if (e.timerNormalized == 0f || e.timerNormalized == 1f||playerLeave)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
    private void Equipment_OnProgressChange(object sender, Equipment.OnProgressChangeEventArgs e)
    {
        if (type == Type.Progress)
        {
            //Updates fill amount based on Player's progress
            barImage.fillAmount = e.progressNormalized;
            if (e.progressNormalized == 0f || e.progressNormalized == 1f)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayerLeave(bool newPlayerLeave)
    {
        playerLeave=newPlayerLeave;
    }
}
