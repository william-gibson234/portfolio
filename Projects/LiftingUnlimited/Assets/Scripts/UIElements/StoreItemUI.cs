using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Class to store the individual store item UI element
public class StoreItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image coinImage;
    [SerializeField] private MoneyManagerUI moneyManagerUI;
    [SerializeField] private Button button;
    [SerializeField] public ItemSO thisItem;

    [SerializeField] private Player player;

    [SerializeField] private SoundManager sM;

    [SerializeField] private Color effectColor;

    [SerializeField] private ShopUI shopUI;

    private Outline outline;

    private void Start()
    {
        costText.text = thisItem.cost.ToString();
        button.GetComponent<Image>().sprite = thisItem.buttonImage;

        outline = GetComponent<Outline>();
    }
    public void SetIsOwned()
    {
        costText.gameObject.SetActive(false);
        coinImage.gameObject.SetActive(false);
        shopUI.ClearStoreItemOutline();
    }

    //Function for when the Player clicks on this UI element
    //Determines whether the Player owns the hat, if not it buys the hat and updates the environment, if they do, it sets the Player's current hat to this one
    public void OnButtonClick()
    {
        if (!thisItem.owned)
        {
            if (thisItem.cost <= moneyManagerUI.GetPlayerMoney())
            {
                thisItem.owned = true;

                sM.OnItemBought();

                moneyManagerUI.ItemBought(thisItem.cost);

                costText.gameObject.SetActive(false);
                coinImage.gameObject.SetActive(false);

                player.SetHat(thisItem.hatPrefab);

                shopUI.ClearStoreItemOutline();

                outline.effectColor = effectColor;
            }
        }
        else
        {
            sM.OnButtonClick();
            player.SetHat(thisItem.hatPrefab);

            shopUI.ClearStoreItemOutline();

            outline.effectColor = effectColor;
        }
    }
}
