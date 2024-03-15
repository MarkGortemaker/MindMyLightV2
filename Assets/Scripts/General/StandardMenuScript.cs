using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardMenuScript : MonoBehaviour
{
    public Button closeButton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //using the escape key to activate the close button on the menu this code is attached to
        {
            closeButton.onClick.Invoke();
        }
    }
}
