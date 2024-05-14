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

    SOLUTIONS:
    1- Figure out what exactly is causing the shift in position. Is it a linerenderer issue?
   */

    public void UpdateMap(List<GameObject> list, int iconNumber)
    {
        if (iconNumber == 0) 
        { 
            List<GameObject> newList = new List<GameObject>();

            foreach (GameObject obj in list)
            {
                //FIND A WAY TO ADD EACH STARDUST PATCH
                //for i 
                //newList.Add(obj.GetComponentsInChildren<Transform>()[i].gameObject);
            }

            list = newList;
        }

        foreach (GameObject obj in list)
        {
            GameObject icon;

            if (!activeList.ContainsKey(obj))
            {   
                icon = Instantiate(icons[iconNumber], transform);
                activeList.Add(obj, icon);
            }

            else
            {
                if (!activeList.TryGetValue(obj, out icon)) { Debug.Log("Error: Could not find value of " + obj.name); }
            }

            RectTransform rect = icon.GetComponent<RectTransform>();
            rect.position = iconNumber == 0 ? obj.transform.position + new Vector3(12, 0, -20) : obj.transform.position;
        }

        foreach (KeyValuePair<GameObject, GameObject> pair in activeList) 
        { 
            if (pair.Key == null)
            {
                Destroy(pair.Value.gameObject);
            }
        }
    }

}
