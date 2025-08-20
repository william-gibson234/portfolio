using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Class controlling functionality of UI element that displays the Option screen
//Allows Player to turn on and off music and sound effects as well as change volumne level
public class OptionsScreenUI : MonoBehaviour
{
    public event EventHandler OnMusicChange;

    [SerializeField] private Button backButton;
    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button volumneButton;

    [SerializeField] private TextMeshProUGUI soundEffectButtonText;
    [SerializeField] private TextMeshProUGUI musicButtonText;
    [SerializeField] private TextMeshProUGUI volumneButtonText;

    [SerializeField] private SoundManager sM;

    [SerializeField] private PauseScreenUI pS;

    private bool soundEffects = true;
    private bool music = true;
    private int volume = 10;



    private void Awake()
    {
        Hide();
        pS.OnOptionsButtonPress += PauseScreenUI_OnOptionsButtonPress;
        backButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Hide();
        });
        //Turn sound effects on and off
        soundEffectButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            soundEffects = !soundEffects;
            if (soundEffects)
            {
                soundEffectButtonText.text = "Sound Effects: ON";
            }
            else
            {
                soundEffectButtonText.text = "Sound Effects: OFF";
            }
        });
        //Turn music on and off
        musicButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            music = !music;
            OnMusicChange?.Invoke(this, EventArgs.Empty);
            if (music)
            {
                musicButtonText.text = "Music: ON";
            }
            else
            {
                musicButtonText.text = "Music: OFF";
            }
        });
        volumneButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            IncrementVolume();
            volumneButtonText.text = "Volume: " + volume;
        });
    }
    //Function to control how the volume is increased
    private void IncrementVolume()
    {
        if (volume < 10)
        {
            volume++;
        }
        else if(volume == 10)
        {
            volume = 1;
        }
    }
    public bool GetSoundEffects()
    {
        return soundEffects;
    }
    public bool GetMusic()
    {
        return music;
    }
    public float GetVolume()
    {
        return volume / 10f;
    }
    private void PauseScreenUI_OnOptionsButtonPress(object sender, EventArgs e)
    {
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
