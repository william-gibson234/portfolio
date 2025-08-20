using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Class that controls functionality of the Countdown, which is displayed in between the user entering the weight and the lift starting
public class CountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;


    private bool isCountingDown;

    private float timer = 1.5f;

    private void Awake()
    {
        Hide();
        isCountingDown = false;
    }
    private void Update()
    {
        if (isCountingDown)
        {
            //Functionality handles the 3 second timer whenever it should be counting down
            timer -= Time.deltaTime;
            countdownText.text = ((int)(timer*2f + 1f)).ToString();
            if (timer <= 0f)
            {
                timer = 1.5f;
                isCountingDown = false;
                Hide();
            }
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        LiftingGameManager.isWindowOpen = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        LiftingGameManager.isWindowOpen = false;
    }
    public void StartCountdown()
    {
        Show();
        countdownText.text = 3.ToString();
        isCountingDown = true;
    }
}
