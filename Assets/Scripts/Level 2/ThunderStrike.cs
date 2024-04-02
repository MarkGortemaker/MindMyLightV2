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
            Debug.DrawRay(new Vector3(transform.position.x - 20, transform.position.y, transform.position.z - 20), Vector3.down * 300f, Color.red); //debug lines
            Debug.DrawRay(new Vector3(transform.position.x + 20, transform.position.y, transform.position.z + 20), Vector3.down * 300f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 20, transform.position.y, transform.position.z + 20), Vector3.down * 300f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x + 20, transform.position.y, transform.position.z - 20), Vector3.down * 300f, Color.red);

            if (Physics.BoxCast(transform.position, new Vector3(20f, 20f, 20f), Vector3.down * 500f, out hit) && hit.collider.tag == "Player")
            {
                Debug.Log("Warning...");
                hudWarning.gameObject.SetActive(true); //just slapping on a dark screen for now, improve this when it's time to do the HUD (maybe with a fade-in)
                IsWarned = true;
                StartCoroutine(ThunderHit());
            }
        }
    }

    IEnumerator ThunderHit()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Strike!");
        thunder.transform.position = transform.position; //maybe we can use multiple thunders per cloud?
        thunder.SetActive(true);
        yield return new WaitForSeconds(2f);
        IsWarned = false;
        hudWarning.gameObject.SetActive(false);
    }
}
