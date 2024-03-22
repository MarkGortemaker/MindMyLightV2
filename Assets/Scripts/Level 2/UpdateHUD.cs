using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public static int balloonCount = 0; 

    void Update()
    {
        scoreText.text = balloonCount.ToString();   
    }
}
