using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to store all of the data for the game, as well as the randomly generated lists of Record Holders
[System.Serializable]
public class GameData
{
    public bool injuryStatus;
    public int coinCount;

    public float bestSquat;
    public float bestBench;
    public float bestDeadlift;
    public float bestTotal;

    public string stateRecordText;
    public string nationalRecordText;
    public string internationalRecordText;

    public float benchProgression;
    public float squatProgression;
    public float deadliftProgression;

    public float bestCompTotal;

    public bool[] isHatOwnedList = new bool[9];

    private struct Competitor
    {
        public string firstName;
        public string lastName;

        public int total;
    }
    private List<Competitor> StateRecordHolders;
    private List<Competitor> NationalRecordHolders;
    private List<Competitor> InterNationalRecordHolders;

    //Declaring the class results in a new instance with default data
    public GameData()
    {
        injuryStatus = false; 
        coinCount = 0; 

        bestSquat = 0; 
        bestBench = 0;
        bestDeadlift = 0;
        bestTotal = 0;

        bestCompTotal = 0;

        benchProgression = 0f;
        squatProgression = 0f;
        deadliftProgression = 0f;

        for (int i = 0;i<isHatOwnedList.Length;i++)
        {
            isHatOwnedList[i] = false;
        }

        StateRecordHolders = new List<Competitor>();

        Competitor s1 = new Competitor();
        s1.firstName = "John";
        s1.lastName = "David";
        s1.total = 2545;
        StateRecordHolders.Add(s1);

        Competitor s2 = new Competitor();
        s2.firstName = "Chris";
        s2.lastName = "Wilson";
        s2.total = 2535;
        StateRecordHolders.Add(s2);

        Competitor s3 = new Competitor();
        s3.firstName = "Trevor";
        s3.lastName = "Martinez";
        s3.total = 2495;
        StateRecordHolders.Add(s3);

        Competitor s4 = new Competitor();
        s4.firstName = "Ian";
        s4.lastName = "Nguyen";
        s4.total = 2490;
        StateRecordHolders.Add(s4);

        Competitor s5 = new Competitor();
        s5.firstName = "Jimmy";
        s5.lastName = "Jackson";
        s5.total = 2480;
        StateRecordHolders.Add(s5);

        for (int i = 0; i < 5; i++)
        {
            int currPlace = i + 1;
            stateRecordText += currPlace + ". " + StateRecordHolders[i].firstName + " " + StateRecordHolders[i].lastName + " " + StateRecordHolders[i].total + "\n";
        }

        NationalRecordHolders = new List<Competitor>();

        Competitor n1 = new Competitor();
        n1.firstName = "James";
        n1.lastName = "Taylor";
        n1.total = 3105;
        NationalRecordHolders.Add(n1);

        Competitor n2 = new Competitor();
        n2.firstName = "Miles";
        n2.lastName = "Williams";
        n2.total = 3100;
        NationalRecordHolders.Add(n2);

        Competitor n3 = new Competitor();
        n3.firstName = "Scott";
        n3.lastName = "Davis";
        n3.total = 3085;
        NationalRecordHolders.Add(n3);

        Competitor n4 = new Competitor();
        n4.firstName = "Robert";
        n4.lastName = "Jones";
        n4.total = 3055;
        NationalRecordHolders.Add(n4);

        Competitor n5 = new Competitor();
        n5.firstName = "Elijah";
        n5.lastName = "Thompson";
        n5.total = 3025;
        NationalRecordHolders.Add(n5);

        for (int i = 0; i < 5; i++)
        {
            int currPlace = i + 1;
            nationalRecordText += currPlace + ". " + NationalRecordHolders[i].firstName + " " + NationalRecordHolders[i].lastName + " " + NationalRecordHolders[i].total + "\n";
        }
        InterNationalRecordHolders = new List<Competitor>();

        Competitor i1 = new Competitor();
        i1.firstName = "Will";
        i1.lastName = "Gibson";
        i1.total = 4095;
        InterNationalRecordHolders.Add(i1);

        Competitor i2 = new Competitor();
        i2.firstName = "Liam";
        i2.lastName = "Morris";
        i2.total = 3875;
        InterNationalRecordHolders.Add(i2);

        Competitor i3 = new Competitor();
        i3.firstName = "Paul";
        i3.lastName = "Turner";
        i3.total = 3685;
        InterNationalRecordHolders.Add(i3);

        Competitor i4 = new Competitor();
        i4.firstName = "Henry";
        i4.lastName = "Lee";
        i4.total = 3435;
        InterNationalRecordHolders.Add(i4);

        Competitor i5 = new Competitor();
        i5.firstName = "Daniel";
        i5.lastName = "Cook";
        i5.total = 3395;
        InterNationalRecordHolders.Add(i5);

        for (int i = 0; i < 5; i++)
        {
            int currPlace = i + 1;
            internationalRecordText += currPlace + ". " + InterNationalRecordHolders[i].firstName + " " + InterNationalRecordHolders[i].lastName + " " + InterNationalRecordHolders[i].total + "\n";
        }
    }
}
