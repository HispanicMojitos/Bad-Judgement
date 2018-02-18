using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f; // We'll use this for different fire rates
    public float weaponRange = 50f; // Not all weapons have the same range
    public float hitForce = 100f; // This will help us add punch to diffrent weapons
    public Transform gunEnd; // This is the end of your gun's barrel, the shot will come out of this (this is an empty GameObject which will be postioned manually)
    private Camera fpsCam; // first person camera reference 
    private WaitForSeconds shotDuration = new WaitForSeconds(0.7f); // This will determine for how long will our ray be visible
    private AudioSource gunAudio; // This is for you to do your magic Martin!
    private LineRenderer laserLine; // Now this my friends, is something I don't know why I have to put it, but I'll understand later
    private float nextFire; // This is to determine how much the game needs to wait to shoot again (useful for pistols and shotguns)

    // Use this for initialization
    void Start()
    {
        laserLine = GetComponent<LineRenderer>(); // This is pretty straight forward, but just in case, the laser line gets the needed component
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>(); // Since the gun doesn't have a camera, we search the camera component in the parent directory (the player)
    }
    
    // Update is called once per frame
    void Update()
    {
        // Lets start shooting shit
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire) // if the shoot button is pressed and the game time exceeds the nextFire time then you shoot shit
        {
            nextFire = Time.time + fireRate; // So you can't spam shoot like an idiot
            StartCoroutine(ShotEffect()); // Don't ask me about this, still learning about this
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)); // this sets our ray origin to the center of the screen
            // that way our shoot will be accurate
            RaycastHit hit; // hit registration
            laserLine.SetPosition(0, gunEnd.position);
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point); //Don't ask me yet, 25 hours have passed since the last time I slept
            }
            else
            {
                laserLine.SetPosition(1, fpsCam.transform.forward * weaponRange);
            }

        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false; // All of this is to make the shooting sound, shooting the laser, waiting the laser duration time and then turning the laser off
    }


}