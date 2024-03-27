using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    RaycastHit hit;
    bool IsWarned = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsWarned)
        {
            Debug.DrawRay(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z - 5), Vector3.down * 200f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z + 5), Vector3.down * 200f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z + 5), Vector3.down * 200f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z - 5), Vector3.down * 200f, Color.red);
            if (Physics.BoxCast(transform.position, new Vector3(5f, 5f, 5f), Vector3.down * 500f, out hit) && hit.collider.tag == "Player")
            {
                Debug.Log("Warning...");
                StartCoroutine(ThunderHit());
                IsWarned = true;
            }
        }
    }

    IEnumerator ThunderHit()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Strike!");
        IsWarned = false; 
    }
}
