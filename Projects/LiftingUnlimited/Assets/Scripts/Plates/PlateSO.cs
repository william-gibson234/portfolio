using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object for a weight plate to control the visual
[CreateAssetMenu()]
public class PlateSO : ScriptableObject
{
    public float weight;
    public Transform platePrefab;
}
