using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText; //placeholder "lifebar"

    public static int balloonCount = 0;
    public static string lifeBar = "* * * "; //placeholder content for the "lifebar"

    void Update()
    {
        scoreText.text = balloonCount.ToString();
        lifeText.text = lifeBar;

        if (balloonCount == 5 )
        {
            scoreText.text = "Win! You are awesoome :)";
        }

        if (lifeBar == "")
        {
            lifeText.text = "DEAD";
        }
    }
}
