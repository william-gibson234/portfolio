using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Loader;

//Class to load different game scenes
public static class Loader
{
    //Enum with each different scene
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
        HelpScene,
    }

    private static Scene targetScene;

    //Two functions load the scenes properly, with the loading screen up for the in between sections
    public static void Load(Scene scene)
    {
        Loader.targetScene = scene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {

        SceneManager.LoadScene(Loader.targetScene.ToString());

    }
}

