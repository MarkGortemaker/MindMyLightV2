using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDProgressCount : MonoBehaviour
{
    public Image[] progressCounters;
    public Sprite filledImage;
    void Awake()
    {
        progressCounters = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        int progress = Level1Controller.difficulty - 1;

        int counter = 0;
        foreach (Image obj in progressCounters)
        {
            if (counter < progress)
            {
                obj.sprite = filledImage;
                counter++;
            }
        }
    }
}
