using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1UpdateHUD : MonoBehaviour
{
    public TMPro.TMP_Text tmp;
    float stardustNumber;

    void Start()
    {
        tmp = GetComponentInChildren<TMPro.TMP_Text>();
        stardustNumber = 0;
    }


    void FixedUpdate()
    {
        if (tmp != null)
        {
            stardustNumber = Mathf.Lerp(stardustNumber, Level1Controller.collectedStardust + 1, 0.1f);
            tmp.text = Mathf.FloorToInt(stardustNumber) + "/" + Level1Controller.stardustWinGoal;
        }
    }
}
