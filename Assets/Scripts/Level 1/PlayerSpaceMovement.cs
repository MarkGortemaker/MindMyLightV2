using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpaceMovement : MonoBehaviour
{
    public float speed = 8f;

    public bool IsInvincible;

    public Transform starTransform;

    public GameObject lostStardustPatch;

    public GameObject winScreen;
    public GameObject loseScreen;

    Material material;
    public Color hurtColor;
    public Color initialColor;

    Rigidbody rb;

    ParticleSystem ps;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        ps = GetComponentInChildren<ParticleSystem>();

        material = GetComponent<Renderer>().material;
        initialColor = material.color;

        starTransform = Level1Controller.starTransform;

        IsInvincible = false;
    }

    private void Update()
    {
        if (Level1Controller.collectedStardust >= 7500)
        {
            Level1Controller.EndGame(winScreen);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Star"))
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

        if (col.CompareTag("Obstacle"))
        {
            material.color = hurtColor;
            col.attachedRigidbody.AddForce((col.transform.position - transform.position) * speed / 3, ForceMode.Impulse);
            rb.AddForce((transform.position - col.transform.position) * speed / 2, ForceMode.Impulse);

            if (!IsInvincible)
            {
                if (Level1Controller.stardustMeter <= 0)
                {
                    Level1Controller.EndGame(loseScreen);
                } 

                else
                {
                    float tempStardust = Level1Controller.stardustMeter;
                    ps.Play();

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

                    tempStardust -= Level1Controller.stardustMeter;

                    DropStardust(tempStardust);

                    IsInvincible = true;
                }
            }

            StartCoroutine(EndInvincibility());
        }
    }

    void DropStardust(float stardust)
    {
        int lostStardustCount = Mathf.FloorToInt(stardust / 10);

        while (lostStardustCount > 0)
        {
            Vector3 offset = new Vector3(Random.Range(3f, 5f), 0, Random.Range(3f, 5f));
            GameObject bigStardustPatch = Instantiate(lostStardustPatch, transform.position + offset, Quaternion.Euler(-90, 0, 0));
            var bigMain = bigStardustPatch.GetComponent<ParticleSystem>().main;
            bigMain.maxParticles = Mathf.Clamp(lostStardustCount, 0, 50);
            lostStardustCount -= bigMain.maxParticles;
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
