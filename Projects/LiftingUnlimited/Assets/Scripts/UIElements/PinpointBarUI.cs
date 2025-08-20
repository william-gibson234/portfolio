using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Class to control functionality of the UI element that displays when a Player performs a Squat
//White bar moves across the UI element, Player must click space to stop bar at correct Squat "depth"
public class PinpointBarUI : MonoBehaviour
{
    public event EventHandler<OnProgressStoppedEventArgs> OnProgressStopped;
    public class OnProgressStoppedEventArgs
    {
        public float percentFromPerfect;
    }

    public event EventHandler<OnBarProgressChangedEventArgs> OnBarProgressChanged;
    public class OnBarProgressChangedEventArgs
    {
        public float barProgress;
    }
    public event EventHandler OnStoppedLifting;

    [SerializeField] private SquatRack squatRack;

    [SerializeField] private float greenLength;
    [SerializeField] private float yellowLength;
    [SerializeField] private float redLength;

    private float barSpeed;

    [SerializeField] private Image whiteBar;
    [SerializeField] private Image[] redBars;
    [SerializeField] private Image[] yellowBars;
    [SerializeField] private Image greenBar;
    [SerializeField] private Image background;


    [SerializeField] private TextMeshProUGUI barNameText;


    private float backgroundStart;
    private float backgroundEnd;

    private const float GREEN_BAR_START = 3.5f;
    private const float GREEN_BAR_HEIGHT = 2f;

    private const float END = 8f;

    private const float BAR_OFFSET_Y = 1f;

    private bool isBarMoving;

    private float barDirection;

    private void Awake()
    {
        barDirection = 1;

        barNameText.text = "Pinpoint";
        
        squatRack.OnStopProgress += SquatRack_OnStopProgress;
        squatRack.OnStartProgress += SquatRack_OnStartProgress;

    }
    private void Start()
    {
        Hide();

        isBarMoving = false;

        backgroundStart = background.rectTransform.anchoredPosition.x - (background.rectTransform.rect.width / 2);
        backgroundEnd = background.rectTransform.anchoredPosition.x + (background.rectTransform.rect.width / 2);

        whiteBar.rectTransform.anchoredPosition = new Vector2(backgroundStart, whiteBar.rectTransform.anchoredPosition.y);

        if(greenBar.rectTransform.anchoredPosition.x + greenBar.rectTransform.rect.width > backgroundEnd)
        {
            Debug.LogError("GreenBar too big!");
        }
    }
    private void Update()
    {
        //When bar is moving across UI element must update the position of the white bar and trigger an event to tell listeners that the bar progress has changed
        if (whiteBar.rectTransform.anchoredPosition.x<= backgroundEnd && whiteBar.rectTransform.anchoredPosition.x >= backgroundStart && isBarMoving) {
            whiteBar.rectTransform.anchoredPosition += new Vector2(barSpeed * Time.deltaTime*barDirection, 0);
            OnBarProgressChanged?.Invoke(this, new OnBarProgressChangedEventArgs
            {
                barProgress = whiteBar.rectTransform.anchoredPosition.x +10f
            });
        }
        else if(whiteBar.rectTransform.anchoredPosition.x > backgroundEnd)
        {
            //Player never clicked spacebar and the bar traveled to the end of the UI element
            isBarMoving = false;
            OnProgressStopped?.Invoke(this, new OnProgressStoppedEventArgs
            {
                percentFromPerfect = 0f
            });
            OnStoppedLifting?.Invoke(this, EventArgs.Empty);
            Hide();

        }
        else if(whiteBar.rectTransform.anchoredPosition.x < backgroundStart)
        {
            //Bar has traveled down and back across the UI element so lift has finished
            isBarMoving = false;
            OnStoppedLifting?.Invoke(this, EventArgs.Empty);
            Hide();
        }
    }

    private void SquatRack_OnStopProgress(object sender, EventArgs e)
    {
        //Switch direction
        barDirection = -1;
        float currentPFP;
        if (whiteBar.rectTransform.anchoredPosition.x < 0)
        {
            currentPFP = 0f;
        }
        else
        {
            //Mathematical formula to normalize the percent that the Player reached from the perfect spot in the PinpointBarUI
            currentPFP = 1f - Math.Abs((Math.Abs(whiteBar.rectTransform.anchoredPosition.x) - Math.Abs(greenBar.rectTransform.anchoredPosition.x) - (greenLength / 2f)) / background.rectTransform.rect.width);
        }

        OnProgressStopped?.Invoke(this, new OnProgressStoppedEventArgs
        {
            percentFromPerfect = currentPFP
        });
    }
    private void SquatRack_OnStartProgress(object sender, EventArgs e)
    {
        Show();

        float squatProgression = squatRack.GetPlayer().GetSquatProgression();
        float weight = squatRack.barbell.GetWeight();

        //When the lift starts, determine how fast the bar should move based on the player progression
        //Utilizes mathematical formula estimating weightlifting progression
        if ((5f*weight)/ 273f - (3f*squatProgression)/ 20f + 1140f / 91f > weight / 91f - squatProgression / 20f + 1230f / 91f) {
            barSpeed = (5f * weight) / 273f - (3f * squatProgression) / 20f + 1140f / 91f;
         }
        else
        {
            barSpeed = weight / 91f - squatProgression / 20f + 1230f / 91f;
        }
        barDirection = 1;

        whiteBar.rectTransform.anchoredPosition = new Vector2(backgroundStart, whiteBar.rectTransform.anchoredPosition.y);

        isBarMoving = true;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public float GetBarDirection()
    {
        return barDirection;
    }

    public float GetBarSpeed()
    {
        return barSpeed;
    }
}
