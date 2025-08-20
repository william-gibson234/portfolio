using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Class to handle the logic of when to load and when to save the game
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;

    [SerializeField] private string fileName;

    [SerializeField] private NewPauseGameUI nPGUI;

    [SerializeField] private bool useEncryption;

    private FileDataHandler dataHandler;

    private GameData gameData;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager");
        }
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        nPGUI.OnGameButtonClick += NewPauseGameUI_OnGameButtonClick;
    }
    private void NewPauseGameUI_OnGameButtonClick(object sender, NewPauseGameUI.OnGameButtonClickedEventArgs e)
    {
        if (e.newGame)
        {
            NewGame();
        }
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.dataHandler.Save(this.gameData);
    }

    //Function to push all loaded data to scripts that need it
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            NewGame();
        }
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
            if (dataPersistenceObj is RankingManagerUI)
            {
                (dataPersistenceObj as RankingManagerUI).Hide();
            }
        }
    }

    //Function to pass data to other scripts so they can update it
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            if(dataPersistenceObj is RankingManagerUI)
            {
                (dataPersistenceObj as RankingManagerUI).Show();
            }
            dataPersistenceObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    //Function only runs when Player quits the game unexpected, not when they save the game through the Pause screen
    private void OnApplicationQuit()
    {
        if (!LiftingGameManager.isInNewPauseGameUI)
        {
            SaveGame();
        }
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public GameData GetData()
    {
        return gameData;
    }
}
