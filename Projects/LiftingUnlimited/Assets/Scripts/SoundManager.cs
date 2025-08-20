using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to manage sound effects
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip liftSound;
    [SerializeField] AudioClip moneySound;
    [SerializeField] AudioClip drinkSound;
    [SerializeField] AudioClip clickSound;

    [SerializeField] OptionsScreenUI oS;


    public void OnLiftComplete()
    {
        if (oS.GetSoundEffects())
        {
            AudioSource.PlayClipAtPoint(liftSound, new Vector3(0, 0, 0), 10f*oS.GetVolume());
        }
    }
    public void OnPlayerDrink()
    {
        if (oS.GetSoundEffects())
        {
            AudioSource.PlayClipAtPoint(drinkSound, new Vector3(0, 0, 0), 10f * oS.GetVolume());
        }
    }
    public void OnItemBought()
    {
        if (oS.GetSoundEffects())
        {
            AudioSource.PlayClipAtPoint(moneySound, new Vector3(0, 0, 0), 10f * oS.GetVolume());
        }
    }
    public void OnButtonClick()
    {
        if (oS != null)
        {
            if (oS.GetSoundEffects())
            {
                AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, 0), 10f * oS.GetVolume());
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(clickSound, new Vector3(0, 0, 0), 10f);
        }
    }
}
