using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class controlling the UI for a Progress Bar which is used by various Equipment
public class ProgressionBarUI : MonoBehaviour
{
    private enum BarType{
        Bench,
        Squat,
        Deadlift,
    }

    [SerializeField] private BarType thisBarType;
    [SerializeField] private Image thisBarImage;
    [SerializeField] private Player player;

    private float progress;

    private void Update()
    {
        if(thisBarType == BarType.Deadlift)
        {
            progress = (player.GetDeadliftProgression()/100f);
        }
        else if(thisBarType== BarType.Squat)
        {
            progress = (player.GetSquatProgression() / 100f);

        }
        else if(thisBarType== BarType.Bench) {

            progress = (player.GetBenchProgression() / 100f);
        }

        thisBarImage.fillAmount = progress;
    }
}
