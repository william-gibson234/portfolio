using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Manages the generation of the crowd during a Weightlifting Competition
public class CompetitionCrowdManager : MonoBehaviour
{
    [SerializeField] private Material[] bodyColors;
    [SerializeField] private GameObject crowdVisual;

    [SerializeField] private CompetitionSymbol competitionSymbol;

    private int xStart, zStart, xSpace, zSpace, xCount, zCount;


    private void Awake()
    {
        competitionSymbol.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;
    }

    //Initializes:
    //"Start" for where the spectators start from with respect to X and Z
    //"Count" for how many spectators will be generated in the X and Z directions
    //"Space" for how much space to leave in between the individual spectators in the X and Z directions
    private void Start()
    {
        xStart = -145;
        zStart = 20;
        xCount = 9;
        zCount = 3;
        xSpace = 10;
        zSpace = 10;
    }

    //Event Listener to trigger whenever Player starts a Competition by interaction with the CompetitionSymbol
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        SetUpSpectators();
    }


    private void SetUpSpectators()
    {
        for(int z = 0;z< zCount; z++) {
            for (int x = 0; x < xCount; x++)
            {
                //Initializes a clone of the crowdVisual, visual of one spectator
                GameObject thisClone = Instantiate(crowdVisual);

                //Sets position of newest clone
                thisClone.transform.position = new Vector3(xStart + xSpace * (1 + x), 0, zStart - zSpace * (1 + z));

                //Randomly selects a color for each spectator's shirt to create a sense of a real crowd
                System.Random rand = new System.Random();
                Material randomMat = bodyColors[rand.Next(0, bodyColors.Length)];

                Transform[] go = thisClone.GetComponentsInChildren<Transform>();

                //Searches through the visual objects to find the "shirt", whose name is labeled as "sphere"
                for(int i=0;i<go.Length;i++)
                {
                   Transform t = go[i];
                    if(t.name == "Sphere")
                    {
                        Renderer mr = t.GetComponent<Renderer>();

                        //Set the current color of this spectator's shirt to be the randomly selected color
                        Material[] copyMats = mr.materials;
                        copyMats[0] = randomMat;
                        mr.materials = copyMats;
                    }
                }
            }
        }
    }
}
