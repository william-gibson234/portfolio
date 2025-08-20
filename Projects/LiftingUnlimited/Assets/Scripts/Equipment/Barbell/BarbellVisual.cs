using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the visuals of the Barbell Game Object
public class BarbellVisual : MonoBehaviour
{
    [SerializeField] private PlateListSO plateSOList;

    [SerializeField] private Barbell barbell;

    [SerializeField] Transform plateHoldPointLeft;
    [SerializeField] Transform plateHoldPointRight;

    private const float PLATE_OFFSET = .75f;

    private void Start()
    {
        UpdateVisual();
    }

    //Method called whenever the barbell's weight must be updated
    public void UpdateVisual()
    {
        //Clears the weight visual
        for(int i = 0;i< plateHoldPointLeft.childCount; i++)
        {
            Object.Destroy(plateHoldPointLeft.GetChild(i).gameObject);
            Object.Destroy(plateHoldPointRight.GetChild(i).gameObject);
        }

        List<float> platesWeight = FindPlatesNeeded();

        for (int i=0;i<platesWeight.Count; i++ )
        {
            float plateWeight = platesWeight[i];

            PlateSO thisPlateSO = null;
            //finds the correct visual to go along with the current weight in platesWeight[]
            foreach (PlateSO plateSO in plateSOList.plateSOArray)
            {
                if (plateWeight == plateSO.weight)
                {
                    thisPlateSO = plateSO;
                }
            }

            //Adds the weight onto each side
            Transform newLeftPlateTransform = Instantiate(thisPlateSO.platePrefab, plateHoldPointLeft);
            newLeftPlateTransform.position += new Vector3(PLATE_OFFSET*i*-1, 0, 0);

            Transform newRightPlateTransform = Instantiate(thisPlateSO.platePrefab, plateHoldPointRight);
            newRightPlateTransform.position += new Vector3(PLATE_OFFSET * i, 0, 0);
        }
    }

    //Function to determine which plates needed to be added, handing errors if weight is illegitimate
    //Returns List containing the weights needed on each side
    private List<float> FindPlatesNeeded()
    {
        float weightOnSide = barbell.GetWeight() - 45;

        if (weightOnSide < 0)
        {
            Debug.LogError("Not enough weight for 45 lb barbell");
            return null;
        }

        weightOnSide /= 2f;

        List<float> platesOnSide = new List<float>();
        
        while (weightOnSide > 0f)
        {
            
            if(weightOnSide >= 45f)
            {
                platesOnSide.Add(45f);
                weightOnSide -= 45f;
                continue;
            }

            if (weightOnSide >= 35f)
            {
                platesOnSide.Add(35f);
                weightOnSide -= 35f;
                continue;
            }
            if (weightOnSide >= 25f)
            {
                platesOnSide.Add(25f);
                weightOnSide -= 25f;
                continue;
            }
            if (weightOnSide >= 10f)
            {
                platesOnSide.Add(10f);
                weightOnSide -= 10f;
                continue;
            }
            if (weightOnSide >= 5f)
            {
                platesOnSide.Add(5f);
                weightOnSide -= 5f;
                continue;
            }
            if (weightOnSide >= 2.5f)
            {
                platesOnSide.Add(2.5f);
                weightOnSide -= 2.5f;
                continue;
            }

            if(weightOnSide > 0 && weightOnSide< 2.5f)
            {
                Debug.LogError("Weight is not divisible by 5");
                return null;
            }
        }
        return platesOnSide;
    }
}
