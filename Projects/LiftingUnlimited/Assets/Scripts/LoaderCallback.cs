using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to manage when to load
public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            //When the game starts, isFirstUpdate is true, so this code runs once when the code loads
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }
}
