using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to control the functionality of the Help page, which user can navigate to from Main Menu
public class HelpSceneContainer : MonoBehaviour
{
    [SerializeField] private Button returnToMenuButton;

    [SerializeField] private SoundManager sM;

    private void Awake()
    {

        returnToMenuButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
}
