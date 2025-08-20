using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface related to saving and loading the Game Data, so Player can continue game after closing
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
