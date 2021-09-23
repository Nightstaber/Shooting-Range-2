using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{

    public TextMeshProUGUI ScoreTrackerText;
    int currentScore = 0;
    int nextScore;

    // Start is called before the first frame update
    void Start()
    {
        ChangeScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(int scoreChange)
    {
        nextScore = currentScore + scoreChange;
        currentScore = currentScore + scoreChange;
        ScoreTrackerText.text = "Score: " + nextScore;
    }
}
