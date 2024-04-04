using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText; //placeholder "lifebar"

    public static int balloonCount;
    public static string lifeBar; //placeholder content for the "lifebar"

    private void Start()
    {
        balloonCount = 0;
        lifeBar = "* * * "; 
    }
    void Update()
    {
        scoreText.text = balloonCount.ToString();
        lifeText.text = lifeBar;
    }
}
