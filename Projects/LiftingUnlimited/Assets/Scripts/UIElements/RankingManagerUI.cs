using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Class controlling the functionality of the Record books, which store competitors who have State, National, and International Records
//Each time a competition finishes, get the players total and the type of competition, determine if a new record is set, adjust record books accordingly
//Implements IDataPersistence because it needs to store record book data
public class RankingManagerUI : MonoBehaviour, IDataPersistence{

    [SerializeField] LiftingManagerUI liftingManagerUI;

    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] TextMeshProUGUI nationalText;
    [SerializeField] TextMeshProUGUI internationalText;

    [SerializeField] Button openWindowButton;
    [SerializeField] Image buttonBackgroundImage;

    [SerializeField] CompetitionSymbol cS;
    [SerializeField] CompetitionManager cM;
    [SerializeField] private SoundManager sM;

    private float bestCompTotal = 0;

    [SerializeField] private NewPauseGameUI newPauseGameUI;
    private void Awake()
    {
        //At program start, set up the record books with preset names
        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;

        openWindowButton.gameObject.SetActive(true);

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
    }
    public void LoadData(GameData data)
    {
        this.stateText.text = data.stateRecordText;
        this.nationalText.text = data.nationalRecordText;
        this.internationalText.text = data.internationalRecordText;

        this.bestCompTotal = data.bestCompTotal;
    }
    public void SaveData(ref GameData data)
    {
        data.stateRecordText = this.stateText.text;
        data.nationalRecordText = this.nationalText.text;
        data.internationalRecordText = this.internationalText.text;

        data.bestCompTotal = this.bestCompTotal;
        Hide();
    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        openWindowButton.gameObject.SetActive(true);
        buttonBackgroundImage.gameObject.SetActive(true);
    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        openWindowButton.gameObject.SetActive(false);
        buttonBackgroundImage.gameObject.SetActive(false);
    }

    public struct Competitor
    {
        public string firstName;
        public string lastName;

        public int total;
    }

    private List<Competitor> StateRecordHolders;
    private List<Competitor> NationalRecordHolders;
    private List<Competitor> InterNationalRecordHolders;

    private bool isEntered;

    //Function that sets up the text of the rankings to be able to display
    public void SetUpRankings(int meetType)
    {
        int currentTotal = (int)liftingManagerUI.GetBestCompTotal();
        if (meetType >= 2)
        {
            if (currentTotal > StateRecordHolders[4].total && currentTotal > bestCompTotal)
            {
                stateText.text = "";
                isEntered = false;
                for(int i = 0; i < 5; i++)
                {
                    int currPlace = i + 1;
                    if (currentTotal > StateRecordHolders[i].total && !isEntered)
                    {
                        stateText.text += currPlace + ". Player 1 " + currentTotal + "\n";
                        isEntered = true;
                    }
                    else
                    {
                        stateText.text += currPlace + ". " + StateRecordHolders[i].firstName + " " + StateRecordHolders[i].lastName + " " + StateRecordHolders[i].total + "\n";
                    }
                }
            }
        }
        if(meetType >= 3)
        {
            if (currentTotal > NationalRecordHolders[4].total && currentTotal > bestCompTotal)
            {

                nationalText.text = "";
                isEntered = false;
                for (int i = 0; i < 5; i++)
                {
                    int currPlace = i + 1;
                    if (currentTotal > NationalRecordHolders[i].total && !isEntered)
                    {
                        nationalText.text += currPlace + ". Player 1 " + currentTotal + "\n";
                        isEntered = true;
                    }
                    else
                    {
                        nationalText.text += currPlace + ". " + NationalRecordHolders[i].firstName + " " + NationalRecordHolders[i].lastName + " " + NationalRecordHolders[i].total + "\n";
                    }
                }
            }
        }
        if(meetType == 4)
        {
            if (currentTotal > InterNationalRecordHolders[4].total && currentTotal > bestCompTotal)
            {
                internationalText.text = "";
                isEntered = false;
                for (int i = 0; i < 5; i++)
                {
                    int currPlace = i + 1;
                    if (currentTotal > InterNationalRecordHolders[i].total && !isEntered)
                    {
                        internationalText.text += currPlace + ". Player 1 " + currentTotal + "\n";
                        isEntered = true;
                    }
                    else
                    {
                        internationalText.text += currPlace + ". " + InterNationalRecordHolders[i].firstName + " " + InterNationalRecordHolders[i].lastName + " " + InterNationalRecordHolders[i].total + "\n";
                    }
                }
            }
        }
        if (currentTotal > bestCompTotal)
        {
            bestCompTotal = currentTotal;
        }
    }
    public void OnOpenRankingButtonPressed()
    {
        if (!LiftingGameManager.isCompeting && !LiftingGameManager.isLifting && !LiftingGameManager.isWindowOpen)
        {
            LiftingGameManager.isWindowOpen = true;
            Show();
            sM.OnButtonClick();
        }
    }

    public void OnCloseButtonPressed()
    {
        LiftingGameManager.isWindowOpen = false;
        Hide();
        sM.OnButtonClick();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
