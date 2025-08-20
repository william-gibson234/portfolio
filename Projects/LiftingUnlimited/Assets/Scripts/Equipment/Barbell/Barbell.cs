using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Container Class that controls functionality of the Barbell Game Object
public class Barbell : MonoBehaviour
{
    [SerializeField] private float weight;
    [SerializeField] private Transform thisBarbell;
    [SerializeField] private BarbellVisual barbellVisual;

    public void SetWeight(float newWeight)
    {
        weight = newWeight;

        barbellVisual.UpdateVisual();
    }
    public float GetWeight() 
    { 
        return weight; 
    } 
}
