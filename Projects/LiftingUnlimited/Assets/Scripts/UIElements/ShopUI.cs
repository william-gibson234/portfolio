using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Class to control the functionality of the shop, which contains nine differernt hats, all represented at StoreItemUIs
//Implements the IDataPersistence because it needs to load the hats the Player has owned
public class ShopUI : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Player player;
    [SerializeField] private SoundManager sM;
    [SerializeField] private StoreItemUI[] itemList;
    [SerializeField] private Button closeButton;

    [SerializeField] private Color blackColor;

    //This list is set at the start of each new load based on saved data, if it exists
    //True if that item is owned, False if the player does not own that specific hat
    private bool[] setOwnList = new bool[9];

    private void Awake()
    {
        gameInput.OnShopOpenAction += GameInput_OnShopOpenAction;

    }
    public void LoadData(GameData data)
    {
        this.setOwnList = data.isHatOwnedList;

        //At the start of each run, updates each StoreItemUI with its "owned" status
        for (int i = 0; i < setOwnList.Length; i++)
        {
            itemList[i].thisItem.owned = setOwnList[i];
            if (itemList[i].thisItem.owned)
            {
                itemList[i].SetIsOwned();
            }
        }
        gameObject.SetActive(false);
    }
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < setOwnList.Length; i++)
        {
            this.setOwnList[i] = itemList[i].thisItem.owned;
        }
        data.isHatOwnedList = this.setOwnList;
    }

    private void GameInput_OnShopOpenAction(object sender, EventArgs e)
    {
        if (!LiftingGameManager.isCompeting && !LiftingGameManager.isLifting && !LiftingGameManager.isWindowOpen)
        {
            gameObject.SetActive(true);
            LiftingGameManager.isWindowOpen = true;
            closeButton.Select();
            sM.OnButtonClick();
        }
    }

    public void OnButtonClick()
    {
        sM.OnButtonClick();
        LiftingGameManager.isWindowOpen = false;
        gameObject.SetActive(false);
    }

    public void ClearStoreItemOutline()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            itemList[i].GetComponent<Outline>().effectColor = blackColor;
        }
    }

    public bool[] isOwned()
    {
        bool[] isOwnedList = new bool[9];
        for(int i = 0;i<itemList.Length;i++) {
            isOwnedList[i] = itemList[i].thisItem.owned;
        }
        return isOwnedList;
    }
}
