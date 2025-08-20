using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Class to control functionality of UI element that Player enters the weight they want to lift into
public class PlayerInputField : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] CompetitionSymbol cS;
    [SerializeField] CompetitionManager cM;
    [SerializeField] LiftingManagerUI lM;

    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI attemptText;

    [SerializeField] private PauseScreenUI pSUI;

    [SerializeField] private CountdownUI countdownUI;


    private string inputString;
    private int inputNum;

    private bool isCompeting;

    private string currentEquipmentName;

    public event EventHandler<OnLiftTextEnteredEventArgs> OnLiftTextEntered;
    public class OnLiftTextEnteredEventArgs
    {
       public float weight;
    }

    private bool isTyping;
    private bool isEntered;

    public void Awake()
    {
        gameInput.OnEnterTextAction += GameInput_OnEnterTextAction;

        Equipment.OnAnyLiftStart += Equipment_OnAnyLiftStart;
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;
        pSUI.OnMainMenuOpen += PauseScreenUI_OnMainMenuOpen;
    }


    private void PauseScreenUI_OnMainMenuOpen(object sender, EventArgs e)
    {
        gameInput.OnEnterTextAction -= GameInput_OnEnterTextAction;

        Equipment.OnAnyLiftStart -= Equipment_OnAnyLiftStart;
        cS.OnCompetitionStart -= CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish -= CompetitionManager_OnCompetitionFinish;
        pSUI.OnMainMenuOpen -= PauseScreenUI_OnMainMenuOpen;
    }


    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        isCompeting = false;
    }
    public void Start()
    {
        Hide();
        isTyping = false;
        isEntered = false;
        isCompeting= false;
    }
    
    public void Update()
    {
        //Conditionals to determine which text to display based on the type of lift
        if (statusText.text == "")
        {
            if (isCompeting)
            {
                statusText.text = "Competition ";
            }
            statusText.text += currentEquipmentName;
            if (currentEquipmentName == "Bench Press")
            {
                statusText.text += ", Best: " + lM.GetBestBench();
            }
            else if (currentEquipmentName == "Squat Rack")
            {
                statusText.text += ", Best: " + lM.GetBestSquat();
            }
            else
            {
                statusText.text += ", Best: " + lM.GetBestDeadlift();
            }
        }
        if (isCompeting)
        {
            if (currentEquipmentName == "Bench Press")
            {
                int count = cM.GetBenchCount() ;
                attemptText.text = "Attempt " + count;
            }
            else if (currentEquipmentName == "Squat Rack")
            {
                int count = cM.GetSquatCount() + 1;
                attemptText.text = "Attempt " + count;
            }
            else
            {
                int count = cM.GetDeadliftCount() ;
                attemptText.text = "Attempt " + count;
            }
        }
        else
        {
            attemptText.text = "";
        }

        //Conditional runs when Player has entered the text of the weight they want
        if (isTyping&&isEntered)
        {
            isEntered = false;

            bool isCorrectText = false;

            inputString = inputField.text;

            //Try to parse String input as an integer
            if (int.TryParse(inputString, out inputNum))
            {
                //String of conditionals to ensure that Player's input is within the bounds before starting the lift
                if (inputNum%5 == 0&&(currentEquipmentName == "Bench Press" && inputNum >= 95 && inputNum <= 1000)||(currentEquipmentName == "Squat Rack" && inputNum >= 135 && inputNum <= 1500)|| (currentEquipmentName == "Deadlift" && inputNum >= 185 && inputNum <= 2000)) {
                    CompetitionManager.thisCompetition tC = cM.GetCurrComp();
                    if (isCompeting)
                    {
                        if(currentEquipmentName =="Squat Rack")
                        {
                            if (tC.squat.Count == 0)
                            {
                                if(inputNum <= lM.GetBestSquat() + 20)
                                {
                                    isCorrectText = true;
                                }
                            }
                            else
                            {
                                if(inputNum <= (float)(tC.squat[tC.squat.Count - 1]) + 20)
                                {
                                    isCorrectText=true;
                                }
                            }
                        }
                        else if(currentEquipmentName == "Bench Press")
                        {
                           
                            if (tC.bench.Count == 0)
                            {
                                if (inputNum <= lM.GetBestBench() + 10)
                                {
                                    isCorrectText = true;
                                }
                            }
                            else
                            {
                                if (inputNum <= (float)(tC.bench[tC.bench.Count - 1]) + 10)
                                {
                                    isCorrectText = true;
                                }
                            }
                        }
                        else
                        {
                            if (tC.deadlift.Count == 0)
                            {
                                if (inputNum <= lM.GetBestDeadlift() + 30)
                                {
                                    isCorrectText = true;
                                }
                            }
                            else
                            {
                                if (inputNum <= (float)(tC.deadlift[tC.deadlift.Count - 1]) + 30)
                                {
                                    isCorrectText = true;
                                }
                            }
                        }
                        //Update the error texts to alert Player of what they did wrong
                        if (!isCorrectText)
                        {
                            errorText.text = "Cannot have large weight jumps";
                            inputField.text = "";
                        }
                    }
                    else
                    {
                        isCorrectText = true;
                    }
                }
                else
                {
                    if (inputNum % 5 != 0)
                    {
                        errorText.text = "Weight must be in multiples of 5";
                        inputField.text = "";
                    }
                    else
                    {
                        errorText.text = "Weight must be in range";
                        inputField.text = "";
                    }
                }
            }
            else
            {
                errorText.text = "Must be a number";
                inputField.text = "";
            }
            if (isCorrectText)
            {
                //The text has passed all of the checks and the lift can now start
                countdownUI.StartCountdown();
                StartCoroutine(waiter());
            }
        }
    }
    //Utilized to hold program for a certain amount of time
    //Allows player be given time before entering their text and the lift starting
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1.5f);

        statusText.text = "";

        OnLiftTextEntered?.Invoke(this, new OnLiftTextEnteredEventArgs
        {
            weight = (float)inputNum
        });

        isTyping = false;
        errorText.text = "";
        inputField.text = "";

        Hide();

    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        isCompeting = true;
    }
    private void Equipment_OnAnyLiftStart(object sender, Equipment.OnAnyLiftStartEventArgs e)
    {
        Show();

        currentEquipmentName = e.liftName;

        isTyping = true;
    }
    private void GameInput_OnEnterTextAction(object sender, EventArgs e)
    {
        if (isTyping)
        {
            isEntered = true;
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        LiftingGameManager.isWindowOpen = true;
    }
    public void Hide()
    {
        LiftingGameManager.isWindowOpen = false;
        gameObject.SetActive(false);
    }
}
