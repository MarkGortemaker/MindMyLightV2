using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThunderStrike : MonoBehaviour
{
    RaycastHit hit;
    public static bool IsWarned = false;
    public GameObject thunder;
    public Image hudWarning;

    void Update()
    {
        if (!IsWarned)
        {
            if (Physics.BoxCast(transform.position, new Vector3(30f, 30f, 50f), Vector3.down * 500f, out hit) && hit.collider.tag == "Player")
            {
                Debug.Log("Warning...");
                hudWarning.gameObject.SetActive(true); //just slapping on a dark screen for now, improve this when it's time to do the HUD (maybe with a fade-in)
                IsWarned = true;
                StartCoroutine(ThunderHit());
            }
        }
    }
    private void OnDrawGizmos() //draw cube to visualize stage borders
    {
        if (!IsWarned)
        {
            Gizmos.color = new Color (Color.red.r, Color.red.g, Color.red.b, 0.5f);
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - 150, transform.position.z), new Vector3(30, 300, 50));
        }
    }
    IEnumerator ThunderHit()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Strike!");
        thunder.transform.position = new Vector3(transform.position.x, transform.position.y, 
            Random.Range(transform.position.z - 8, transform.position.z + 8)); 
        thunder.SetActive(true);
        hudWarning.gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
        IsWarned = false;
    }
}
