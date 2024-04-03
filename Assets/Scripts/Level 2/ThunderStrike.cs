using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThunderStrike : MonoBehaviour
{
    RaycastHit hit;
    public bool IsWarned = false;
    public GameObject thunder;
    public Image hudWarning;

    void Update()
    {
        if (!IsWarned)
        {
            Debug.DrawRay(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z - 5), Vector3.down * 200f, Color.red); //debug lines
            Debug.DrawRay(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z + 5), Vector3.down * 200f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z + 5), Vector3.down * 200f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z - 5), Vector3.down * 200f, Color.red);

            if (Physics.BoxCast(transform.position, new Vector3(5f, 5f, 5f), Vector3.down * 500f, out hit) && hit.collider.tag == "Player")
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
        thunder.transform.position = transform.position;
        thunder.SetActive(true);
        yield return new WaitForSeconds(2f);
        IsWarned = false;
        hudWarning.gameObject.SetActive(false);
    }
}
