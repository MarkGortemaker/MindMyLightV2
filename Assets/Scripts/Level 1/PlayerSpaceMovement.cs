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

    public Transform starTransform;

    Material material;
    public Color hurtColor;
    public Color initialColor;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        material = GetComponent<Renderer>().material;
        initialColor = material.color;

        starTransform = Level1Controller.starTransform;

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
            col.attachedRigidbody.AddForce((col.transform.position - transform.position) * speed / 3, ForceMode.Impulse);
            rb.AddForce((transform.position - col.transform.position) * speed / 2, ForceMode.Impulse);

            if (!IsInvincible)
            {
                if (Level1Controller.stardustMeter <= 0)
                { 
                    GeneralControls.PauseGame();
                    Debug.Log("GAME OVER");
                } 

                else
                {
                    float tempStardust = Level1Controller.stardustMeter; 

                    if (Level1Controller.stardustMeter > 500)
                    {
                        Level1Controller.stardustMeter = 500;
                        Level1Controller.stardustRatio = Level1Controller.stardustMeter / Level1Controller.maxStardustMeter;
                        StartCoroutine(Level1Controller.DecreaseLightRange(20 * Level1Controller.stardustRatio, Level1Controller.stardustRatio));
                        StartCoroutine(Level1Controller.DecreaseSkyboxLightness(Level1Controller.stardustRatio, Level1Controller.stardustRatio / 20));
                    }

                    else
                    {
                        Level1Controller.stardustMeter = 0;
                        Level1Controller.stardustRatio = Level1Controller.stardustMeter / Level1Controller.maxStardustMeter;
                        StartCoroutine(Level1Controller.DecreaseLightRange(0, 0.1f));
                        StartCoroutine(Level1Controller.DecreaseSkyboxLightness(0, 0.1f));
                    }

                    //drop stardust function here

                    IsInvincible = true;
                }
            }

            StartCoroutine(EndInvincibility());
        }
    }

    void DropStardust(float stardust)
    {
        //drop stardust patches according to amount of stardust lost (make large particles with bigger stardust values)
    }
    IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(1.5f);

        material.color = initialColor;

        IsInvincible = false;
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        rb.AddForce(direction * speed, ForceMode.Force);

        if (direction.magnitude != 0)
        {
            transform.LookAt(transform.position + direction * speed); //adjust this when player model is ready
        }

    }

    void EnforceBorder()
    {
        float borderDistance = Level1Controller.borderDistance;

        if ((transform.position - starTransform.position).magnitude > borderDistance)
        {
            Vector3 limitedPosition = (transform.position - starTransform.position).normalized * borderDistance;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, limitedPosition.x, 0.02f), transform.position.y, 
                Mathf.Lerp(transform.position.z, limitedPosition.z, 0.02f));
        }
    }

    void FixedUpdate()
    {
        Move();
        EnforceBorder();
    }

}
