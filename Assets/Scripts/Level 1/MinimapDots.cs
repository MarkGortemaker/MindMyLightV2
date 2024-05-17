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

    public GameObject container;

    public GameObject[] icons;
    public GameObject[] images;

    public Dictionary<GameObject, GameObject> activeList = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        container = GameObject.Find("Other Icons");
        starIcon.GetComponent<RectTransform>().position = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>().position;
    }

    private void FixedUpdate()
    {
        playerIcon.GetComponent<RectTransform>().position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;

        UpdateMap(Level1Controller.stardustLines, 0);
        UpdateMap(Level1Controller.meteors, 1);
        UpdateMap(Level1Controller.comets, 2);
    }

    public void UpdateMap(List<GameObject> list, int iconNumber)
    {

        foreach (KeyValuePair<GameObject, GameObject> pair in activeList)
        {
            if (pair.Key == null)
            {
                Destroy(pair.Value.gameObject);
            }
        }

        foreach (GameObject obj in list)
        {
            GameObject icon;

            if (!activeList.ContainsKey(obj))
            {   
                icon = Instantiate(icons[iconNumber], container.transform);
                activeList.Add(obj, icon);
            }

            else
            {
                if (!activeList.TryGetValue(obj, out icon)) { Debug.Log("Error: Could not find value of " + obj.name); }
            }

            RectTransform rect = icon.GetComponent<RectTransform>();
            rect.position = iconNumber == 0 ? obj.GetComponentInChildren<Transform>().position : obj.transform.position;
        }
        
    }

}
