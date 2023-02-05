using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private Transform plr;

    [SerializeField] private TMP_Text depthText;
    [SerializeField] private TMP_Text killText;
    [SerializeField] private float rate;
    private float depth = 0;
    private int kills = 0;
    private string depthString = "M DEPTH";
    private string killString = " POINTS";

    private float maxDepth = 0;

    // Start is called before the first frame update
    void Start()
    {
        depthText.text = depth.ToString("F2") + depthString;
        killText.text = kills.ToString("F0") + killString;
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
    }
    public void enemyPointIncrement()
    {
        kills++;
        print("In function" + kills);
    }
}
