using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

//Class to control functionality of the UI element that the Player uses to Bench Press
public class CircleProgressBarUI : MonoBehaviour
{
    public event EventHandler<OnPlayerDoneBenchEventArgs> OnPlayerDoneBench;
    public class OnPlayerDoneBenchEventArgs
    {
        public float averageScore;
    }

    private float circleSpeed;

    private bool isBeforeSwitch;

    [SerializeField] Image movingCircle;
    [SerializeField] Image innerCircle;

    [SerializeField] Image targetCircleBefore;
    [SerializeField] Image innerTargetCircleBefore;

    [SerializeField] Image targetCircleAfter;
    [SerializeField] Image innerTargetCircleAfter;

    [SerializeField] Image backgroundCircle;

    [SerializeField] private TextMeshProUGUI circleBarNameText;

    [SerializeField] BenchPress benchPress;
    Barbell barbell;
    float weight;
    float benchProgression;

    Vector2 movingCircleStart;
    Vector2 innerMovingCircleStart;

    int count = 1;

    private List<Image> imageList;

    private float[] closePercentageList;

    private bool isCircleClosing = false;

    private float closePercentage;

    private void Awake()
    {
        benchPress.OnBenchProgressStopped += BenchPress_OnBenchProgressStopped;
        benchPress.OnBenchProgressStarted += BenchPress_OnBenchProgressStarted;
    }

    private void Start()
    {
        barbell = benchPress.barbell;

        movingCircleStart = new Vector2(movingCircle.rectTransform.rect.width, movingCircle.rectTransform.rect.height);
        innerMovingCircleStart = new Vector2(innerCircle.rectTransform.rect.width, innerCircle.rectTransform.rect.height);

        closePercentageList = new float[3];

        imageList = new List<Image>();
        imageList.Add(movingCircle);
        imageList.Add(innerCircle);
        imageList.Add(targetCircleBefore);
        imageList.Add(backgroundCircle);
        imageList.Add(innerTargetCircleBefore);
        imageList.Add(targetCircleAfter);
        imageList.Add(innerTargetCircleAfter);

        circleBarNameText.text = "Circle Bar #"+count;

        isBeforeSwitch = true;

        Hide();
    }

    private void Update()
    { 

        circleBarNameText.text = "Circle Bar #" + count;

        if (movingCircle.rectTransform.rect.width <= targetCircleBefore.rectTransform.rect.width)
        {
            isBeforeSwitch = false;
        }
        else
        {
            isBeforeSwitch = true;
        }

        weight = barbell.GetWeight();
        if (benchPress.GetPlayer() != null)
        {
            benchProgression = benchPress.GetPlayer().GetBenchProgression();
        }

        if (count >3)
        {
            isCircleClosing = false;
        }
        if (isCircleClosing)
        {
            //While the circles are closing, update the sizes
            Show();
            if (movingCircle.rectTransform.rect.width > 0)
            {
                movingCircle.rectTransform.sizeDelta -= new Vector2(circleSpeed, circleSpeed);
            }

            if (innerCircle.rectTransform.rect.width > 0)
            {
                innerCircle.rectTransform.sizeDelta -= new Vector2(circleSpeed, circleSpeed);
            }
            //Update the percentage of how close the moving circle is to the target circle
            closePercentage = Math.Abs((1f - Math.Abs(movingCircle.rectTransform.rect.width - targetCircleBefore.rectTransform.rect.width) / backgroundCircle.rectTransform.rect.width));
        }
        else
        {
            Hide();
        }
    }

    private void BenchPress_OnBenchProgressStopped(object sender, EventArgs e)
    {
        //Update behavior based on how many times the user triggered this event
        if (count <= 3)
        {
            closePercentageList[count - 1] = closePercentage;
            count++;

            circleSpeed += 0.02f;

            movingCircle.rectTransform.sizeDelta = movingCircleStart;
            innerCircle.rectTransform.sizeDelta = innerMovingCircleStart;
        }
        if (count > 3)
        {
            //Player is done with the bench press, must compute the score
            float average = 0;
            for (int i = 0; i < closePercentageList.Length; i++)
            {
                average += closePercentageList[i];
            }
            average /= 3;


            OnPlayerDoneBench?.Invoke(this, new OnPlayerDoneBenchEventArgs
            {
                averageScore = average
            });
        }
        
    }
    private void BenchPress_OnBenchProgressStarted(object sender, EventArgs e)
    {
        if (!isCircleClosing)
        {
            isCircleClosing = true;
            count = 1;

            targetCircleBefore.gameObject.SetActive(true);
            innerTargetCircleBefore.gameObject.SetActive(true);

            targetCircleAfter.gameObject.SetActive(false);
            innerTargetCircleAfter.gameObject.SetActive(false);

            //Mathematical formulas to model bench press progression to determine how challenging the lift should be
            if (0.000441989f * barbell.GetWeight() - 0.0025f * benchProgression + 0.058011f > 0.000265193f * barbell.GetWeight() - 0.0009 * benchProgression + 0.0748066)
            {
                circleSpeed = 0.000441989f * barbell.GetWeight() - 0.0025f * benchProgression + 0.058011f;
            }
            else
            {
                circleSpeed = 0.000265193f * barbell.GetWeight() - 0.0009f * benchProgression + 0.0748066f;
            }
        }
    }

    private void Show()
    {
        foreach (Image i in imageList)
        {
            i.gameObject.SetActive(true);
        }

        if (isBeforeSwitch)
        {

            targetCircleBefore.gameObject.SetActive(true);
            innerTargetCircleBefore.gameObject.SetActive(true);

            targetCircleAfter.gameObject.SetActive(false);
            innerTargetCircleAfter.gameObject.SetActive(false);
        }
        else
        {

            targetCircleBefore.gameObject.SetActive(false);
            innerTargetCircleBefore.gameObject.SetActive(false);

            targetCircleAfter.gameObject.SetActive(true);
            innerTargetCircleAfter.gameObject.SetActive(true);
        }
        circleBarNameText.gameObject.SetActive(true);
    }
    private void Hide()
    {
        foreach(Image i in imageList)
        {
            i.gameObject.SetActive(false);
        }
        circleBarNameText.gameObject.SetActive(false);
    }

    public int GetCount()
    {
        return count;
    }
    public float GetCircleSpeed()
    {
        return circleSpeed;
    }

    public void SetIsCircleClosing(bool IsCircleClosing)
    {
        isCircleClosing = IsCircleClosing;
        movingCircle.rectTransform.sizeDelta = movingCircleStart;
        innerCircle.rectTransform.sizeDelta = innerMovingCircleStart;

        OnPlayerDoneBench?.Invoke(this, new OnPlayerDoneBenchEventArgs
        {
            averageScore = 0
        });
    }
}
