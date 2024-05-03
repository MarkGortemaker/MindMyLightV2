using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class Level1UpdateHUD : MonoBehaviour
{
    TMPro.TMP_Text tmp;
    float stardustNumber;

    void Start()
    {
        tmp = GetComponentInChildren<TMPro.TMP_Text>();
        stardustNumber = 0;
    }


    void FixedUpdate()
    {
        stardustNumber = Mathf.Lerp(stardustNumber, Level1Controller.collectedStardust + 1, 0.1f);
        tmp.text = "GOAL:\nGive Zironko stardust!\n" + Mathf.FloorToInt(stardustNumber) + "/7500";
    }
}
