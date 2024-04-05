using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdateHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public static GameObject[] hearts = new GameObject[3];

    public static int balloonCount;
    public static int lives;

    private void Start()
    {
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        balloonCount = 0;
        lives = 3;
        SetActiveHearts();
    }
    void Update()
    {
        scoreText.text = balloonCount.ToString() + "/5";
    }
    public static void SetActiveHearts()
    {
        int counter = 0;
        foreach (GameObject heart in hearts)
        {
            if (counter < lives)
            {
                heart.SetActive(true);
                counter++;
            }
            else
            {
                heart.SetActive(false);
            }
        }
    }
}
