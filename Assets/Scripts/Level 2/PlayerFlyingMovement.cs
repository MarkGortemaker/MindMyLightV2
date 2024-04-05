using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingMovement : MonoBehaviour
{
    public float speed = 8f;
    public float rotationMultiplier = 5f; 
    public float limitX = 60f;
    public float limitZ = 60f;

    public bool IsInvincible;

    public GameObject winScreen;
    public GameObject loseScreen;

    public GameObject lilguy;
    public GameObject lilguyDizzy;
    public GameObject lilguyText;

    public ParticleSystem burstParticle;
    public ParticleSystem hurtParticle;

    Material material;
    public Color hurtColor;
    public Color initialColor;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        initialColor = material.color;

        IsInvincible = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Balloon")
        {
            UpdateHUD.balloonCount++;
            col.gameObject.SetActive(false);

            StartCoroutine(DisplayBalloonGetMessage());

            burstParticle.gameObject.SetActive(true);
            burstParticle.transform.position = col.gameObject.transform.position;
            burstParticle.Play();

            if (UpdateHUD.balloonCount == 5)
            {
                IsInvincible = true;
                //for now the winscreen is instant, but we can insert a small animation or at least a waitforseconds coroutine here
                GeneralControls.PauseGame();
                GameController.EndGame(winScreen);
            }
        }

        if (col.tag == "Bird" || col.tag == "Thunder")
        {
            material.color = hurtColor;

            if (!IsInvincible) 
            {
                UpdateHUD.lives--;
                UpdateHUD.SetActiveHearts();
                StartCoroutine(DisplayDizzyLilguy());

                if (UpdateHUD.lives <= 0)
                {
                    //animation can go here
                    GeneralControls.PauseGame();
                    GameController.EndGame(loseScreen);
                }

                else
                {
                    hurtParticle.gameObject.SetActive(true);
                    hurtParticle.Play();
                    IsInvincible = true;
                }
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

    IEnumerator DisplayBalloonGetMessage()
    {
        lilguyText.SetActive(true);
        yield return new WaitForSeconds(1f);
        lilguyText.SetActive(false);
    }
    IEnumerator DisplayDizzyLilguy()
    {
        lilguy.SetActive(false);
        lilguyDizzy.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        lilguy.SetActive(true);
        lilguyDizzy.SetActive(false);
    }
    void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); //constantly go forward with no input

        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * speed * rotationMultiplier * Time.deltaTime); 

        transform.Rotate(Vector3.right * -Input.GetAxis("Vertical") * speed * rotationMultiplier * Time.deltaTime); 
    }

    void ClampRotation() //limits the rotation of the x and z axis by the limit variables on the script
    {
        Vector3 rotate = transform.localEulerAngles; 

        if (rotate.x > 180) { rotate.x -= 360; } //adjust the rotation to ensure it doesn't cut off
        rotate.x = Mathf.Clamp(rotate.x, -limitX, limitX); 

        if (rotate.z > 180) { rotate.z -= 360; }
        rotate.z = Mathf.Clamp(rotate.z, -limitZ, limitZ); //the limits should be smoother, like snapping back after rotating too much

        transform.localEulerAngles = rotate; 

        rotate.z = Mathf.Lerp(rotate.z, 0, 0.01f); //linearly interpolate the z axis rotation to reset to 0

        transform.localEulerAngles = rotate;
    }

    void FixedUpdate()
    {
        Move();

        ClampRotation();

        GameController.EnforceBorder(transform); //limit the player's movement to stay within stage borders
    }

}
