using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingMovement : MonoBehaviour
{
    public float speed = 6f;
    public int rotationMultiplier = 5; 
    public float limitX = 60f;
    public float limitZ = 60f;

    public ParticleSystem burstParticle;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Balloon")
        {
            UpdateHUD.balloonCount++;
            col.gameObject.SetActive(false);
            burstParticle.gameObject.SetActive(true);
            burstParticle.transform.position = col.gameObject.transform.position;
            burstParticle.Play();
        }

        if (col.tag == "Bird" || col.tag == "Thunder")
        {
            //HP loss and game over
        }
    }

    

    void FixedUpdate()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime); //constantly go forward with no input

        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * speed * rotationMultiplier * Time.deltaTime); //rotate right or left via input

        transform.Rotate(Vector3.right * -Input.GetAxis("Vertical") * speed * rotationMultiplier * Time.deltaTime); //rotate up or down via input

        Vector3 rotate = transform.localEulerAngles; //assign current rotation to this variable

        if (rotate.x > 180) { rotate.x -= 360; } //adjust the rotation to ensure it doesn't cut off
        rotate.x = Mathf.Clamp(rotate.x, -limitX, limitX); //limit the x axis...

        if (rotate.z > 180) { rotate.z -= 360; } 
        rotate.z = Mathf.Clamp(rotate.z, -limitZ, limitZ); //...and the z axis (the limits should be smoother, like snapping back after rotating too much)

        transform.localEulerAngles = rotate; //set the transform's rotation to limited values

        rotate.z = Mathf.Lerp(rotate.z, 0, 0.01f); //linearly interpolate the z axis rotation to reset to 0

        transform.localEulerAngles = rotate; 

        GameController.EnforceBorder(transform); //limit the player's movement to stay within stage borders
    }

}
