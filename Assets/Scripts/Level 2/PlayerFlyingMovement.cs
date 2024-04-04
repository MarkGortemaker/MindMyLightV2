using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingMovement : MonoBehaviour
{
    public float speed = 8f;
    public float rotationMultiplier = 5f; 
    public float limitX = 60f;
    public float limitZ = 60f;

    public bool IsInvincible = false;
    public int hitPoints;

    public ParticleSystem burstParticle;
    public ParticleSystem hurtParticle;
    Material material;
    public Color hurtColor;
    public Color initialColor;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        initialColor = material.color;
        hitPoints = 3;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Balloon")
        {
            UpdateHUD.balloonCount++;
            col.gameObject.SetActive(false);
            burstParticle.gameObject.SetActive(true);
            burstParticle.transform.position = col.gameObject.transform.position;
            burstParticle.Play();

            if (UpdateHUD.balloonCount == 5)
            {
                GeneralControls.PauseGame();
                GameController.WinGame();
            }
        }

        if (col.tag == "Bird" || col.tag == "Thunder")
        {
            material.color = hurtColor;

            if (!IsInvincible) 
            {
                hitPoints--;
                UpdateHUD.lifeBar = UpdateHUD.lifeBar.Remove(UpdateHUD.lifeBar.Length - 2);

                if (hitPoints <= 0)
                {
                    GeneralControls.PauseGame();
                    GameController.LoseGame();
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
        yield return new WaitForSeconds(2f);

        material.color = initialColor;

        IsInvincible = false;
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
