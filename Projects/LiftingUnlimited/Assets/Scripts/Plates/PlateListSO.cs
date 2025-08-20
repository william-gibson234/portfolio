using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object to store all of the Plate Scriptable Objects in order to display their visuals
[CreateAssetMenu()]
public class PlateListSO : ScriptableObject
{
    public PlateSO[] plateSOArray;
}
