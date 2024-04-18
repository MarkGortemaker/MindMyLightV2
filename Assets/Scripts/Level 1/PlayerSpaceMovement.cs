using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpaceMovement : MonoBehaviour
{
    public float speed = 8f;

    public bool IsInvincible;

    //public GameObject winScreen;
    //public GameObject loseScreen;

    Material material;
    public Color hurtColor;
    public Color initialColor;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        material = GetComponent<Renderer>().material;
        initialColor = material.color;

        IsInvincible = false;
    }
    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Obstacle")
        {
            material.color = hurtColor;

            if (!IsInvincible)
            {

                /* if (game over condition)
                {
                    //animation can go here
                    GeneralControls.PauseGame();
                    GameController.EndGame(loseScreen);
                } 

                else
                {
                    hurt ouch oof
                    IsInvincible = true;
                } */
            }

            StartCoroutine(EndInvincibility());
        }
    }

    IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(1.5f);

        material.color = initialColor;

        IsInvincible = false;
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddForce(direction * speed, ForceMode.Force);

        if (direction.magnitude != 0)
        {
            transform.LookAt(transform.position + direction * speed); //adjust this when player model is ready
        }

    }

    void FixedUpdate()
    {
        Move();
    }

}
