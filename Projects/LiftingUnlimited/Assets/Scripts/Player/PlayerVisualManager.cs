using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to control which animation style to set active, the Lifting animations or normla walking animations
public class PlayerVisualManager : MonoBehaviour
{
    [SerializeField] public GameObject liftingVisual;
    [SerializeField] public GameObject[] animatedVisual;

    public void UpdateVisual(bool isLifting)
    {
        if(LiftingGameManager.isLifting)
        {
            liftingVisual.SetActive(true);
            for (int i = 0; i < animatedVisual.Length; i++)
            {
                animatedVisual[i].SetActive(false);
            }
        }
        else
        {
            liftingVisual.SetActive(false);

            for (int i = 0; i < animatedVisual.Length; i++)
            {
                animatedVisual[i].SetActive(true);
            }
        }
    }
}
