using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Class to control the functionality of the UI element that displays the amount of Coins the Player has
public class MoneyManagerUI : MonoBehaviour, IDataPersistence
{
    [SerializeField] TextMeshProUGUI coinText;

    private int playerMoney;


    public void LoadData(GameData data)
    {
        this.playerMoney = data.coinCount;
    }
    public void SaveData(ref GameData data)
    {
        data.coinCount = this.playerMoney;
    }
    private void Start()
    {
        coinText.text = playerMoney.ToString();
    }
    private void Update()
    {

        coinText.text = playerMoney.ToString();
    }
    //Function to update the money depending on how player places in a competition
    public void UpdateMoney(int playerRank, int meetType)
    {
        if (playerRank <= 5)
        {
            if (meetType == 1)
            {
                playerMoney += (6 - playerRank);
            }
            if (meetType == 2)
            {
                playerMoney += 3 * (6 - playerRank);
            }
            if (meetType == 3)
            {
                playerMoney += 10 * (6 - playerRank);
            }
            if (meetType == 4)
            {
                playerMoney += 100 * (6 - playerRank);
            }
            if (playerMoney > 9999)
            {
                playerMoney = 9999;
            }
        }
        coinText.text = playerMoney.ToString();
    }
    public void ItemBought(int cost)
    {
        playerMoney -= cost;

        coinText.text = playerMoney.ToString();
    }

    public int GetPlayerMoney()
    {
        return playerMoney;
    }
}
