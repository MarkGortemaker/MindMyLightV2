using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MinimapDots : MonoBehaviour
{
    public GameObject starIcon;
    public GameObject playerIcon;

    public GameObject[] icons;
    public GameObject[] images;

    public Dictionary<GameObject, GameObject> activeList = new Dictionary<GameObject, GameObject>();

    /* -> Dictionary class implementation here <- */

    void Start()
    {
        starIcon.GetComponent<RectTransform>().position = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>().position;
    }

    private void FixedUpdate()
    {
        playerIcon.GetComponent<RectTransform>().position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;

        UpdateMap(Level1Controller.stardustLines, 0);
        UpdateMap(Level1Controller.meteors, 1);
        UpdateMap(Level1Controller.comets, 2);
    }

    /*
     PROBLEMS:
    1- Stardust locations are inaccurate
    2- Despawning objects do not have their icons removed

    SOLUTIONS:
    1- Figure out what exactly is causing the shift in position. Is it a linerenderer issue?
    2- Write a custom Dictionary class that works both ways, that can check whether its pair is null or not. Then have it destroy the icon if the obj is missing
   */

    public void UpdateMap(List<GameObject> list, int iconNumber)
    {
        foreach (GameObject obj in list)
        {
            GameObject icon;

            if (!activeList.ContainsKey(obj))
            {
                icon = Instantiate(icons[iconNumber], transform);
                activeList.Add(obj, icon);
                activeList.Add(icon, obj);
            }

            else
            {
                if (!activeList.TryGetValue(obj, out icon)) { Debug.Log("Error: Could not find value of " + obj.name); }
            }

            RectTransform rect = icon.GetComponent<RectTransform>();
            rect.position = iconNumber == 0 ? obj.transform.position + new Vector3(12, 0, -20) : obj.transform.position;
        }
    }

}
