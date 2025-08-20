using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

//Class to control functionality of UI element that displays the Player's best lifts and totals
public class LiftingManagerUI : MonoBehaviour, IDataPersistence
{
    public event EventHandler<OnNewLiftSetEventArgs> NewLiftSet;
    public class OnNewLiftSetEventArgs
    {
        public LiftComplete lC;
    }

    [SerializeField] private TextMeshProUGUI liftText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI successOrFailText;

    [SerializeField] private TextMeshProUGUI squatText;
    [SerializeField] private TextMeshProUGUI benchText;
    [SerializeField] private TextMeshProUGUI deadliftText;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private Image background;

    [SerializeField] private CompetitionSymbol cS;
    [SerializeField] private CompetitionManager cM;
    [SerializeField] private PauseScreenUI pSUI;

    float bestSquat;
    float bestBench;
    float bestDeadlift;
    float bestTotal;

    float bestCompSquat;
    float bestCompBench;
    float bestCompDeadlift;
    float bestCompTotal;

    //Struct to be created whenever a lift is complete, with information about the lift
    public struct LiftComplete
    {
        public float bestLift;
        public float currLift;
        public float minLift;
        public bool isCompetition;
    }
    public void LoadData(GameData data)
    {
        this.bestSquat = data.bestSquat;
        this.bestBench = data.bestBench;
        this.bestDeadlift = data.bestDeadlift;
        this.bestSquat = data.bestSquat;

        squatText.text = "Squat: " + bestSquat;

        benchText.text = "Bench: " + bestBench;
        deadliftText.text = "Deadlift: " + bestDeadlift;

        bestTotal = bestSquat + bestBench + bestDeadlift;
        totalText.text = "Total: " + bestTotal;
    }
    public void SaveData(ref GameData data)
    {
        data.bestSquat = this.bestSquat;
        data.bestBench = this.bestBench;
        data.bestDeadlift = this.bestDeadlift;
        data.bestSquat = this.bestSquat;
    }
    private void Awake()
    {
        Show();

        Equipment.OnAnyLiftComplete += Equipment_OnAnyLiftComplete;
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;
        pSUI.OnMainMenuOpen += PauseScreenUI_OnMainMenuOpen;
    }

    
    private void PauseScreenUI_OnMainMenuOpen(object sender, EventArgs e)
    {
        Equipment.OnAnyLiftComplete -= Equipment_OnAnyLiftComplete;
        cS.OnCompetitionStart -= CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish -= CompetitionManager_OnCompetitionFinish;

    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        //Re-update UI information whenever a competition is finished
        squatText.text = "Squat: " + bestSquat;

        benchText.text = "Bench: " + bestBench;
        deadliftText.text = "Deadlift: " + bestDeadlift;

        bestTotal = bestSquat + bestBench + bestDeadlift;
        totalText.text = "Total: " + bestTotal;
    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        //Change UI to temporarily display the current competition's information
        bestCompSquat = 0;
        bestCompBench = 0;
        bestCompDeadlift = 0;
        bestCompTotal =0;

        squatText.text = "Squat: ";
        benchText.text = "Bench: ";
        deadliftText.text = "Deadlift: ";
        totalText.text = "Total: ";
    }
    private void Equipment_OnAnyLiftComplete(object sender, Equipment.OnAnyLiftCompleteEventArgs e)
    {
        //Update information based on if the lift was a competition or if the lift set a new record
        LiftComplete LC = new LiftComplete();
        LC.currLift = e.lift.weight;
        LC.isCompetition = e.lift.compete;
        if (e.lift.name == "Squat")
        {
            LC.bestLift = bestSquat;
            LC.minLift = 135f;
        }
        else if (e.lift.name == "Bench Press")
        {
            LC.bestLift = bestBench;
            LC.minLift = 95f;
        }
        else
        {
            LC.bestLift = bestDeadlift;
            LC.minLift = 185f;
        }
        NewLiftSet?.Invoke(this, new OnNewLiftSetEventArgs
        {
            lC = LC
        });

        liftText.text = "Last Lift: " + e.lift.name;
        weightText.text = "Weight: " + e.lift.weight;
        if (e.lift.success) {
            successOrFailText.text = "Success";
        }
        else
        {
            successOrFailText.text = "Fail";
        }
        if (e.lift.success)
        {
            if (e.lift.compete)
            {
                if (e.lift.name == "Squat" && e.lift.weight > bestCompSquat)
                {
                    squatText.text = "Squat: " + e.lift.weight;
                    bestCompSquat = e.lift.weight;

                    if (e.lift.weight > bestSquat)
                    {
                        bestSquat = bestCompSquat;
                    }
                }
                else if (e.lift.name == "Bench Press" && e.lift.weight > bestCompBench)
                {
                    benchText.text = "Bench: " + e.lift.weight;
                    bestCompBench = e.lift.weight;
                    if (e.lift.weight > bestBench)
                    {
                        bestBench = bestCompBench;
                    }
                }
                else if (e.lift.name == "Deadlift" && e.lift.weight > bestCompDeadlift)
                {
                    deadliftText.text = "Deadlift: " + e.lift.weight;
                    bestCompDeadlift = e.lift.weight;
                    if (e.lift.weight > bestDeadlift)
                    {
                        bestDeadlift = e.lift.weight;
                    }
                }
                bestCompTotal = bestCompDeadlift + bestCompSquat + bestCompBench;
                if (bestCompTotal > bestTotal)
                {
                    bestTotal = bestCompTotal;
                }
                totalText.text = "Total: " + bestCompTotal;
            }
            else
            {
                if (e.lift.name == "Squat" && e.lift.weight > bestSquat)
                {
                    squatText.text = "Squat: " + e.lift.weight;
                    bestSquat = e.lift.weight;
                }
                else if (e.lift.name == "Bench Press" && e.lift.weight > bestBench)
                {
                    benchText.text = "Bench: " + e.lift.weight;
                    bestBench = e.lift.weight;
                }
                else if (e.lift.name == "Deadlift" && e.lift.weight > bestDeadlift)
                {
                    deadliftText.text = "Deadlift: " + e.lift.weight;
                    bestDeadlift = e.lift.weight;
                }
                bestTotal = bestDeadlift + bestSquat + bestBench;
                totalText.text = "Total: " + bestTotal;
            }
        }
    }

    public float GetBestSquat()
    {
        return bestSquat;
    }
    public float GetBestBench()
    {
        return bestBench;
    }
    public float GetBestDeadlift()
    {
        return bestDeadlift;
    }

    public float GetBestTotal()
    {
        return bestTotal;
    }
    public float GetBestCompTotal()
    {
        return bestCompTotal;
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
