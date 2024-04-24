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
        if (col.tag == "Star")
        {
            if (Level1Controller.stardustMeter > 500)
            {
                col.GetComponentInChildren<ParticleSystem>().Play();

                float stardustGained = Level1Controller.stardustMeter - 500;
                Level1Controller.collectedStardust += stardustGained;
                Level1Controller.stardustMeter = 500f;
                Level1Controller.stardustRatio = Level1Controller.stardustMeter / Level1Controller.maxStardustMeter;

                StopAllCoroutines();

                StartCoroutine(Level1Controller.DecreaseLightRange(20 * Level1Controller.stardustRatio, Level1Controller.stardustRatio));
                StartCoroutine(Level1Controller.DecreaseSkyboxLightness(Level1Controller.stardustRatio, Level1Controller.stardustRatio / 20));

                Debug.Log("STARDUST RESET");
                Debug.Log("Total Stardust Collected: " + Level1Controller.collectedStardust); 
            }

            else
            {
                Debug.Log("You need to collect more stardust :(");
            }
        }

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
