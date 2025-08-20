using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CompetitionManager : MonoBehaviour
{
    public event EventHandler<OnCompetitionFinishEventArgs> OnCompetitionFinish;
    public class OnCompetitionFinishEventArgs{
        public Vector3 newCamPosition;
    }

    public struct thisCompetition
    {
        public ArrayList squat;

        public ArrayList bench;

        public ArrayList deadlift;
    }

    //[SerializeField] CompetitionSymbol cS;
    [SerializeField] PlayerCompDropdown pCD;
    [SerializeField] CompetitionPlacementManager cpM;
    [SerializeField] Player player;
    [SerializeField] RankingManagerUI rankingManagerUI;


    [SerializeField] CompetitionSquatRack squatRack;

    [SerializeField] BenchPress benchPress;
    [SerializeField] DeadliftPlatform deadliftPlatform;
    private int squatCount = 0;
    private int benchCount = 0;
    private int deadliftCount = 0;

    private int currentSquat;
    private int bestSquat;

    private int currentMeetType;

    private thisCompetition currentCompetition;

    private void Awake()
    {
        pCD.OnCorrectMeetEntered += PlayerCompDropdown_OnCorrectMeetEntered;

        //cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;

        Equipment.OnAnyLiftComplete += Equipment_OnAnyLiftComplete;
    }
    private void Start()
    {
        //for(int i= 1; i <= 4; i++)
        //{
        //cpM.SetUpRankings(1);
        //}

        currentCompetition = new thisCompetition();
    }
    private void PlayerCompDropdown_OnCorrectMeetEntered(object sender, PlayerCompDropdown.OnCorrectMeetEnteredEventArgs e)
    {
        //Debug.Log(e.oMM.meetType);

        player = e.oMM.meetPlayer;
        currentMeetType = e.oMM.meetType;

        squatCount = 0;
        benchCount = 0;
        deadliftCount = 0;

        currentCompetition = new thisCompetition();

        currentCompetition.squat = new ArrayList();
        currentCompetition.bench = new ArrayList();
        currentCompetition.deadlift = new ArrayList();

        player.SetSelectedObject(squatRack);
        squatRack.InteractAndEnter(player);


        LiftingGameManager.isLifting = true;
    }

    private void Equipment_OnAnyLiftComplete(object sender, Equipment.OnAnyLiftCompleteEventArgs e)
    {
        if (e.lift.compete)
        {
            if (e.lift.name == "Squat")
            {
                //Debug.Log("5");
                if (squatCount < 2)
                {
                    squatCount++;
                    player.SetSelectedObject(squatRack);
                    squatRack.InteractAndEnter(player);

                    currentCompetition.squat.Add(e.lift.weight);

                }
                else
                {
                    benchCount++;
                    player.SetSelectedObject(benchPress);
                    benchPress.InteractAndEnter(player);
                }
            }
            else if (e.lift.name == "Bench Press")
            {
                if (benchCount < 3)
                {
                    benchCount++;
                    player.SetSelectedObject(benchPress);
                    benchPress.InteractAndEnter(player);

                }
                else
                {
                    deadliftCount++;
                    player.SetSelectedObject(deadliftPlatform);
                    deadliftPlatform.InteractAndEnter(player);
                }
            }
            else
            {
                if (deadliftCount < 3)
                {
                    deadliftCount++;
                    player.SetSelectedObject(deadliftPlatform);
                    deadliftPlatform.InteractAndEnter(player);
                }
                else
                {
                    LiftingGameManager.isLifting = false;
                    player.SetIsLifting(false);
                    player.transform.position = new Vector3(0, 0, 0);

                    cpM.SetUpRankings(currentMeetType);
                    rankingManagerUI.SetUpRankings(currentMeetType);

                    LiftingGameManager.isCompeting = false;

                    OnCompetitionFinish?.Invoke(this, new OnCompetitionFinishEventArgs
                    {
                        newCamPosition = new Vector3(0, 30, -20)
                    });
                }
            }
        }
    }
    public thisCompetition GetCurrComp()
    {
        return currentCompetition;
    }

    public int GetSquatCount() 
    { 
        return squatCount;
    }
    public int GetBenchCount()
    {
       return benchCount;
    }
    public int GetDeadliftCount()
    {
        return deadliftCount;
    }
}

