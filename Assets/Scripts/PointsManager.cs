using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private Transform plr;

    [SerializeField] public TMP_Text depthText;
    [SerializeField] public TMP_Text killText;

    [SerializeField] public TMP_Text highScore;
    [SerializeField] public TMP_Text scoreText;

    [SerializeField] private float rate;
    private float depth = 0;
    private int kills = 0;
    private int score = 0;
    private string depthString = "M DEPTH";
    private string killString = " POINTS";

    private float maxDepth = 0;

    // Start is called before the first frame update
    void Start()
    {
        depthText.text = depth.ToString("F2") + depthString;
        killText.text = kills.ToString("F0") + killString;
        // score.text = score.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        // Depth Counter
        float tempDepth = -plr.position.y;
        
        if (tempDepth > maxDepth) {
            maxDepth = tempDepth;
            depthText.text = tempDepth.ToString("F2") + depthString;
        }

        // Enemy Kill Counter
        killText.text = kills.ToString("F0") + killString;
        scoreText.text = score.ToString("F0") + killString;
    }

    public void enemyPointIncrement()
    {
        kills++;
        score++;
        print("In function" + kills);
    }

    public void UpdateSavedScore() {
        if (score > PlayerPrefs.GetInt("highScore", 0)){
            PlayerPrefs.SetInt("highScore", score);
            highScore.text = score.ToString("F0");
        }
        highScore.text = PlayerPrefs.GetInt("highScore", score).ToString();
    }
}
