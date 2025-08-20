using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object to store data about each Store Item
[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public Sprite buttonImage;
    public GameObject hatPrefab;
    public int cost;
    public bool owned;
}
