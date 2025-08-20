using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Competitor = CompetitionPlacementManager.Competitor;

//Displays a UI Element of the results of the latest competition
public class CompetitionPlacementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textFields;
    [SerializeField] private Player p;
    [SerializeField] private SoundManager sM;

    private void Start()
    {
        Hide();
    }

    //When the Player clicks the Close Button, hides the window and triggers the close window sound
    public void OnButtonClick()
    {
        LiftingGameManager.isWindowOpen = false;
        p.SetWindowOpen(false);
        Hide();
        sM.OnButtonClick();
    }

    //Function displays Competitiors text and displays the UI
    public void SetText(ArrayList competitors)
    {
            LiftingGameManager.isWindowOpen = true;
            Show();

            p.SetWindowOpen(true);
            for (int i = 0; i < textFields.Length; i++)
            {
                int currPlace = i + 1;

                Competitor thisComp = (Competitor)competitors[i];

                textFields[i].text = currPlace + ".) " + thisComp.firstName + " " + thisComp.lastName + " " + thisComp.total;
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
}
