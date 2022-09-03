using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("General Setup Settings")] 
    [Tooltip("How fast ship moves")][SerializeField] float controlSpeed = 10f;
   [Tooltip("How far player moves left and right")] [SerializeField] float xRange = 10f;
   [Tooltip("How far player moves up and down")] [SerializeField] float yRange = 9f;


    [Header("Lasers gun array")]
    [Tooltip("Added all player lasers")][SerializeField] GameObject[] lasers;
    [SerializeField] AudioClip laserSound;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -10f;
    [SerializeField] private float rotationFactor; // this line is where the rest of your serialized variables are

    AudioSource audioSource;

    float xThrow;
    float yThrow;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();

    }

   

     void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition +pitchDueControlThrow ;

        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        // transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
        // On next line, we get the target rotation but we don't assign it to transform.localRotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        // here, we use Quaternion.RotateTowards from the current rotation
        // to the target rotation. NOTE that the rotationFactor has to be small, such as 1, otherwise the rotation will be too fast and will be janky.
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationFactor);
    }

    private void ProcessTranslation()
    {
         xThrow = Input.GetAxis("Horizontal"); // moving using the old input manager 

         yThrow = Input.GetAxis("Vertical"); // moving using the old input manager 

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(laserSound);
            }
        }
        else
        {
            SetLasersActive(false);
            audioSource.Stop();

        }
    }
    

    private void SetLasersActive(bool isActive)
    {
        foreach(GameObject laser in lasers)
        {
            var emissionModule=laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled=isActive;
        }
    }

    
}
