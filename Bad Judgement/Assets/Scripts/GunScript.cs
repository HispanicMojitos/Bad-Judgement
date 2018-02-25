using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    #region Variables
    private float damage = 10f; // First we declare our needed variables
    private float range = 100f;
    private float impactForce = 30f;
    private float fireRate = 15f;
    private Camera fpsCam; // camera reference
    //public ParticleSystem muzzleFlash; // this will search for the muzzle flash particle system we'll add
    private GameObject impactEffect; // So this one is also a particle effect but we want to reference it as an object so that we can place it inside our game world
    private float nextTimeToFire = 0f;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // If the user presses the fire buttton
        { // and if the time that has passed is greater than the rate of fire
            nextTimeToFire = Time.time + 1f / fireRate; // formula for fire rate

            Shoot();
        }

    }

    void Shoot() // to shoot, we will use raycasts. 
    {
        //muzzleFlash.Play();// Play the muzzle flash we create
                           // An invisible ray shot from the camera to the forward direction
                           // If the object is hit, we do some damage, if not, then nothing happens
                           // First we need to reference the camera
        RaycastHit hit; //This is a varaible that strores info of what the ray hits
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name); // So this is how to shoot a ray, Physics.Raycast asks for starting postion which is the camera, where to shoot it (forward from the camera) and what to gather (hit)
                                           // We then say that we want to log (like a Console.Write();) what our ray hit
                                           // We wrapped our code in an if statement because the return type of the Physics.Raycast is a boolean

            Target target = hit.transform.GetComponent<Target>(); // This uses the other script we created for the target
                                                                  // what it does is get the object with the component called Target and stores it in a variable
            if (target != null) // if the target recieves the variable we want (it will be null if we hit something without the target component)
                target.TakeDamage(damage); // then we give damage, notice that we can do this because we declared our TakeDamage method as public

            if (hit.rigidbody != null) // if the object that we hit has a rigidbody
                hit.rigidbody.AddForce(-hit.normal * impactForce); // we apply a force to it (the addforce is negative so that it goes away from us)

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(impactGO, 1f);
            // We use instantiate to create the object, we enter what we want to instantiate, where and in what direction, hit.normal is a flat surface that points directly in front, that way our effect will always be toward its source
            // We also destroy the object 1 second after the created of it, that way we won't have millions of objects on our scene

        }
    }
}
