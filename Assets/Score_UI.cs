using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score_UI : MonoBehaviour
{

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text hiScoreText;
    int score;
    int hiScore;  
    Scene_Manager manager;
    levelId scoreLevel;
    void Start()
    {
        manager = FindObjectOfType<Scene_Manager>();
        GetHiScore();
    }

    
    void Update()
    {
        score = manager.GetScore();
        scoreText.text = "Score: " + score.ToString();
        hiScoreText.text = "Hi Score: " + hiScore.ToString();
    }

    void GetHiScore()
    {
        hiScore = manager.GetHiScore(scoreLevel);
    
    }
}
