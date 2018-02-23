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
    public AudioSource AK47; // This is for you to do your magic Martin! // I did it ! and it is the audiosource of AK47 sound, so choose the AK47 weapon like componant of the AudioSource in the Inspecto unity editor
    public AudioClip AK47shootSound; // Sound of AK47
    public AudioClip AK47reload; // Sounf or reloading AK47, to use for reloading (when you'll implement the reloading codes Andrews, have pleasure to use it ;) )
    private LineRenderer laserLine; // Now this my friends, is something I don't know why I have to put it, but I'll understand later
    private float nextFire = 0; // This is to determine how much the game needs to wait to shoot again (useful for pistols and shotguns)

    // Use this for initialization
    void Start()
    {
        laserLine = GetComponent<LineRenderer>(); // This is pretty straight forward, but just in case, the laser line gets the needed component
        fpsCam = GetComponentInParent<Camera>(); // Since the gun doesn't have a camera, we search the camera component in the parent directory (the player)
    }
    
    // Update is called once per frame
    void FixedUpdate() // MARTIN => I've change Update to FixeUpdate for allowing a good rate of sound shoot in the AK47shoot method
    {
        Sounds.AK47shoot(AK47, AK47shootSound, nextFire); // Methode pour engager le son de l'AK 47 : le volume est en parametre nomé, si tu souhaite le changer utilise comme dernier argument (un argument nommé) tel quel : 'volume:0.5f' /!\ le volume se calibre de 0.000f => 1.000F*f, si le volume n'est pas pris entre ces valeur il y aura une erreur
        // Lets start shooting shit
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) // if the shoot button is pressed and the game time exceeds the nextFire time then you shoot shit
        {
            nextFire = Time.deltaTime + fireRate; // So you can't spam shoot like an idiot
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
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false; // All of this is to make the shooting sound, shooting the laser, waiting the laser duration time and then turning the laser off
    }


}