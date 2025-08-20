using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that controls the Lifting animations of the player to add a key visual, immersive aspect to the game
//Models physically the skeletal movements while Lifting
public class PlayerLiftingAnimator : MonoBehaviour
{
    [SerializeField] public Transform playerHead;
    [SerializeField] public Transform[] playerArms;
    [SerializeField] public Transform playerBody;
    [SerializeField] public Transform[] playerLegs;

    [SerializeField] private Player player;

    [SerializeField] private PlayerVisualManager playerVisualManager;

    public void DeadliftAnimator(float barbellY)
    {

        player.transform.eulerAngles = new Vector3(0, 0, 0);
        playerArms[0].position = new Vector3(playerArms[0].position.x, barbellY+1, playerArms[0].position.z);
        playerArms[0].eulerAngles = new Vector3(90, 0, 0);

        playerArms[1].position = new Vector3(playerArms[1].position.x, barbellY+1, playerArms[1].position.z);
        playerArms[1].eulerAngles = new Vector3(90, 0, 0);

        playerBody.position = new Vector3(playerBody.position.x, barbellY+.5f, playerBody.position.z);
        playerBody.eulerAngles = new Vector3(150- (60*(barbellY/7.5f)), 0, 0);

        playerLegs[0].position = new Vector3(playerLegs[0].position.x, playerLegs[0].position.y, playerBody.position.z-1.5f);
        playerLegs[1].position = new Vector3(playerLegs[1].position.x, playerLegs[1].position.y, playerBody.position.z - 1.5f);

        playerHead.position = new Vector3(playerBody.position.x, barbellY +5f, playerBody.position.z + 0.55f*(7.5f-barbellY));
    }
    public void SquatAnimator(float barbellY)
    {
        player.transform.eulerAngles = new Vector3(0, 0, 0);

        playerBody.position = new Vector3(playerBody.position.x, barbellY - 3, playerBody.position.z);

        playerHead.position = new Vector3(playerHead.position.x,playerBody.position.y+6, playerHead.position.z);

        playerArms[0].position = new Vector3(playerArms[0].position.x,playerBody.position.y+2f, playerArms[0].position.z);
        playerArms[1].position = new Vector3(playerArms[1].position.x, playerBody.position.y + 2f, playerArms[1].position.z);

    }
    public void BenchAnimator(float barbellY)
    {
        playerArms[0].eulerAngles = new Vector3(90, 0, 0);
        playerArms[1].eulerAngles = new Vector3(90, 0, 0);


        playerArms[0].transform.position = new Vector3(playerArms[0].position.x, barbellY-1f, playerArms[0].position.z);
        playerArms[1].transform.position = new Vector3(playerArms[1].position.x, barbellY - 1f, playerArms[1].position.z);
    }

    //Function to resets the position and rotation of the lifting body to the walking body's position  playerArms[1].transform.position = new Vector3(playerArms[1].position.x, barbellY - 1f, playerArms[1].position.z);
    public void Reset()
    {

        //left leg
        playerLegs[0].position = playerVisualManager.animatedVisual[0].transform.position;
        playerLegs[0].eulerAngles = playerVisualManager.animatedVisual[0].transform.eulerAngles;
        //right leg
        playerLegs[1].position = playerVisualManager.animatedVisual[1].transform.position;
        playerLegs[1].eulerAngles = playerVisualManager.animatedVisual[1].transform.eulerAngles;

        //left arm
        playerArms[1].position = playerVisualManager.animatedVisual[3].transform.position;
        playerArms[1].eulerAngles = playerVisualManager.animatedVisual[3].transform.eulerAngles;
        //right arm
        playerArms[0].position = playerVisualManager.animatedVisual[4].transform.position;
        playerArms[0].eulerAngles = playerVisualManager.animatedVisual[4].transform.eulerAngles;

        //body
        playerBody.position = playerVisualManager.animatedVisual[2].transform.position;
        playerBody.eulerAngles = playerVisualManager.animatedVisual[2].transform.eulerAngles;

        //head
        playerHead.position = playerVisualManager.animatedVisual[5].transform.position;
        playerHead.eulerAngles = playerVisualManager.animatedVisual[5].transform.eulerAngles;
    }
}
