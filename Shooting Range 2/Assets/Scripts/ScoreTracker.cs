using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ScoreTrackerText;
    int currentScore = 0;
    int nextScore;

    // Start is called before the first frame update
    void Start()
    {
        // Set score to 0 on start
        ChangeScore(0);
    }

    // Change score method, can be called from other scripts to set the score
    public void ChangeScore(int scoreChange)
    {
        nextScore = currentScore + scoreChange;
        currentScore = currentScore + scoreChange;
        ScoreTrackerText.text = "Score: " + nextScore;
    }
}
