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
    public AudioSource AK47; // AK47 qui est la source de son propre son
    public AudioClip AK47shoot; // bruit de l'AK47 fait lors d'un seul tir
    public AudioClip AK47reload; // bruit de rechargement d'un AK47
    #endregion
    
    // Update is called once per frame
    void Update()
    {

        // Sounds.AK47shoot(AK47, AK47shoot); // ANDREWS !! si tu met la methode pour jouer le son ici, tu remarquera que le son joue a l'infini et qu'il est cadencé (a la cadence que j'ai mise)

        /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Pour montrer montrer un raycast dans la scene on a besoin de definir le ray ainsi que son origine qui est la camera, et le ray bougera en fonction que la souris bouge avec la camera
        Debug.DrawLine(ray.origin, Camera.main.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur
         /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
                                            
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // If the user presses the fire buttton
        { // and if the time that has passed is greater than the rate of fire
            nextTimeToFire = Time.time + 1f / fireRate; // formula for fire rate

             Sounds.AK47shoot(AK47, AK47shoot);  // ICI ANDREWS, le son n'est pas bien cadencé dans méthode, ca fait byzare, j'imagine que c'est a toi de pouvoir regler ta cadence de tir en fonction du son alors amuse toi bien ;)
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
