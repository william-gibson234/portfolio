using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class to control the Main Menu which opens when the user opens the game and gives some options
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button helpButton;

    [SerializeField] private SoundManager sM;

    private void Awake()
    {
        Time.timeScale = 1f;

        playButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Loader.Load(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Application.Quit();
        });
        helpButton.onClick.AddListener(() =>
        {
            sM.OnButtonClick();
            Loader.Load(Loader.Scene.HelpScene);
        });
    }
}
