using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Class to control when the Player will get injured
//Injuries ensure Player cannot progress too fast
public class InjuryManager : MonoBehaviour
{
    [SerializeField] LiftingManagerUI liftingManagerUI;

    private float injuryRisk;

    //Injury guaranteed if greater than 25 lbs
    private float weightJump;

    private void Awake()
    {
        injuryRisk = 0;
        liftingManagerUI.NewLiftSet += LiftingManagerUI_NewLiftSet;
    }

    //Event Listener to trigger whenever a new Lift is started
    private void LiftingManagerUI_NewLiftSet(object sender, LiftingManagerUI.OnNewLiftSetEventArgs e)
    {
        if (e.lC.isCompetition)
        {
            injuryRisk = 0;
        }
        else
        {
            if (e.lC.bestLift == 0 && e.lC.currLift == e.lC.minLift)
            {
                weightJump = 0f;
            }
            else if (e.lC.bestLift == 0)
            {
                weightJump = e.lC.currLift - e.lC.minLift;
            }
            else
            {
                weightJump = e.lC.currLift - e.lC.bestLift;
            }
            if (weightJump >= 25f)
            {
                injuryRisk = 100;
            }
            else
            {
                //Random number between 1 and 100, if 95 or greater, player is hurt
                injuryRisk = Random.value * (99f) + 1f;
            }
        }
    }

    //

    public float GetInjuryRisk()
    {
        return injuryRisk;
    }
}
