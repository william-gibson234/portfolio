using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

//Functionality behind the CompetitionPlacementUI
public class CompetitionPlacementManager : MonoBehaviour
{
    [SerializeField] LiftingManagerUI liftingManagerUI;
    [SerializeField] CompetitionPlacementUI competitionPlacementUI;
    [SerializeField] MoneyManagerUI moneyManagerUI;

    public struct Competitor
    {
        public string firstName;
        public string lastName;

        public int total;
    }

    private string[] firstNames;
    private string[] lastNames;

    private int playerRank;

    private int[] competitorsTotal;

    private void Start()
    {
        //Initialize all of the generic Competitor's names
        firstNames = new string[30]{ "Liam", "Ian","John","Jake","Thomas","Max","Owen","Robert","Calvin","Eric","James","Scott","Chris","Trevor", "Jimmy", "Will", "Mike", "Leon", "Miles", "Alex", "Peter", "Noah", "Elijah", "Oliver", "Duncan", "Paul", "Henry", "Ben", "Teddy", "Daniel" };
        lastNames = new string[30] { "Brown", "Smith", "Wilson", "Williams", "Davis", "Garcia", "Jones", "Rodriguez", "Miller", "Taylor", "Martinez", "Gonzalez", "Jackson", "Thompson", "Nguyen", "Martin", "Moore", "Robinson", "Morris", "Collins", "Turner", "Reyes", "Edward", "Lee", "Cook", "Murphy", "Kim", "Patel", "Myers", "Sanders" }; 
    }
    
    //Function to generate competitors from generic names and initialize random scores
    public void SetUpRankings(int meetType)
    {
        ArrayList competitors = new ArrayList();

        Competitor playerCompetitor = new Competitor();
        playerCompetitor.firstName = "Player 1";
        playerCompetitor.lastName = "";
        playerCompetitor.total = (int)liftingManagerUI.GetBestCompTotal();

        //Generates random competitors
        for (int i  = 9; i >0; i--)
        {
            int lowerBound = 50 * i + (meetType*500+450);
            int upperBound = 50 * i + (meetType*500+500);
            float thisTotalDecimal = Random.value * (upperBound - lowerBound)+lowerBound;

            int thisTotalRounded = (int)(Math.Round(thisTotalDecimal/5f)*5f);

            Competitor thisCompetitor = new Competitor();
            thisCompetitor.firstName = firstNames[(int)(Random.value * 30)];
            thisCompetitor.lastName = lastNames[(int)(Random.value * 30)];
            thisCompetitor.total = thisTotalRounded;

            competitors.Add(thisCompetitor);
        }

        //Inserts player into list of competitors
        for(int i = 0; i < 9; i++)
        {
            if (((Competitor)competitors[i]).total < liftingManagerUI.GetBestCompTotal())
            {
                competitors.Insert(i, playerCompetitor);
                playerRank = i + 1;
                break;
            }
            if (i == 8)
            {
                playerRank = 10;
                competitors.Add(playerCompetitor);
            }
        }
        competitionPlacementUI.SetText(competitors);

        moneyManagerUI.UpdateMoney(playerRank, meetType);
    }
}
