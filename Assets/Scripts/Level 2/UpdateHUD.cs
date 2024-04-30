using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdateHUD : MonoBehaviour
{
    public static GameObject[] hearts = new GameObject[3];
    public static GameObject[] balloons = new GameObject[5];

    public static int balloonCount;
    public static int lives;

    private void Start()
    {
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        balloons = GameObject.FindGameObjectsWithTag("UI Balloon");
        balloonCount = 0;
        lives = 3;
        SetActiveUI(lives, hearts);
        SetActiveUI(balloonCount, balloons);
    }

    /// <summary>
    /// Sets active/inactive the images in the given array displayed on UI, taking the given integer into account.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="objects"></param>
    public static void SetActiveUI(int amount, GameObject[] objects)
    {
        int counter = 0;
        foreach (GameObject obj in objects)
        {
            if (counter < amount)
            {
                obj.SetActive(true);
                counter++;
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    public static IEnumerator Tint(UnityEngine.UI.Image tint, float alpha, float speed)
    {
        Color color = tint.color;

        for (int i = 0; i < 60; i++)
        {
            color.a = Mathf.Lerp(color.a, alpha, Time.deltaTime * speed);
            tint.color = color;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
