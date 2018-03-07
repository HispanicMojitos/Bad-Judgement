using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

public class GunScript : MonoBehaviour
{
    #region Variables

    #region Sounds members
    [SerializeField] private AudioSource AK47; // AK47 qui est la source de son propre son
    [SerializeField] private AudioClip AK47shoot; // bruit de l'AK47 fait lors d'un seul tir
    [SerializeField] private AudioClip AK47reload; // bruit de rechargement d'un AK47
    #endregion Sounds members

    [SerializeField]
    private float damage = 10f; // First we declare our needed variables
    [SerializeField]
    private KeyCode reloadKey = KeyCode.R;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float impactForce = 30f;
    [SerializeField]
    private float fireRate = 15f;
    [SerializeField]
    private GameObject gunEnd; // camera reference
    //public ParticleSystem muzzleFlash; // this will search for the muzzle flash particle system we'll add
    [SerializeField]
    private GameObject impactEffect; // So this one is also a particle effect but we want to reference it as an object so that we can place it inside our game world
    private float nextTimeToFire = 0f;
    [SerializeField]
    private Animation aiming;
    [SerializeField]
    private float amount;
    [SerializeField]
    private float smoothAmount;
    [SerializeField]
    private float maxAmount;
    [SerializeField]
    private int bulletsPerMag = 30;
    [SerializeField]
    private static int maxAmmo = 300;
    private static int magQty = 6;// number of mags you can have
    private Vector3 initialPosition;
    [SerializeField] private static Animator anim;
    [SerializeField]
    private static int currentMag;
    private bool _isReloading = false;
    private int reloadTime = 3000;
    private Magazines mags;
    #endregion

    #region Properties
    public bool isReloading
    {
        get { return _isReloading; }
        set { _isReloading = value; }
    }
    public static int CurrentMag
    {
        get { return currentMag; }
        set { currentMag = value; }
    }
    public static int MagQty
    {
        get { return magQty; }
        set { magQty = value; }
    }
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        aiming = GetComponent<Animation>();
        currentMag = bulletsPerMag;
        initialPosition = transform.localPosition;
        mags = new Magazines(magQty, bulletsPerMag);
        //magQty--;
        //currentMag = mags[0].bullets;
    }
    // Update is called once per frame
    void Update()
    {
        currentMag = mags[mags.index];
        #region Reload
        //magQty = mags.mags.Count-1;
        //magQty = maxAmmo;
        if (Input.GetKeyDown(reloadKey) && !isReloading && mags[mags.index]<bulletsPerMag+1 && magQty>0)
        {
            StartCoroutine(Reload());
            isReloading = false;
        }
        #endregion

        #region Weapon Sway
        // Sounds.AK47shoot(AK47, AK47shoot); // ANDREWS !! si tu met la methode pour jouer le son ici, tu remarquera que le son joue a l'infini et qu'il est cadencé (a la cadence que j'ai mise)
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;
        // -maxAmount is the amount of movement to the left side
        // maxAmount is the amount of movement to the right side
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        // original value, min value, max value
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);
        // this limits the amount of rotation
        Vector3 finalPositon = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPositon + initialPosition, Time.deltaTime * smoothAmount);
        // this interpolates the initial position with the final position
        #endregion

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // If the user presses the fire buttton
        { // and if the time that has passed is greater than the rate of fire
            nextTimeToFire = (Time.time * Time.timeScale) + (1f / (fireRate / 60)); // formula for fire rate
            Shoot();
        }
        if (Input.GetButton("Fire2")) // WIP
        {
            anim.SetBool("IsAiming",true);
        }
        else anim.SetBool("IsAiming", false);

    }

    #region Methods

    void Shoot() // to shoot, we will use raycasts. 
    {
        //muzzleFlash.Play();// Play the muzzle flash we create
        // An invisible ray shot from the camera to the forward direction
        // If the object is hit, we do some damage, if not, then nothing happens
        // First we need to reference the camera
        if (mags[mags.index] > 0 && !isReloading)
        {
            mags[mags.index]--;
            RaycastHit hit; //This is a varaible that strores info of what the ray hits
            Sounds.AK47shoot(AK47, AK47shoot);  //  Joue le son !! A metre l'AK47 comme AudioSource et AK47shoot comme AudioClip
                                                /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
            Debug.DrawLine(gunEnd.transform.position, gunEnd.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur
                                                                                                  /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit))
            {
                //Debug.Log(hit.transform.name); // So this is how to shoot a ray, Physics.Raycast asks for starting postion which is the camera, where to shoot it (forward from the camera) and what to gather (hit)
                                               // We then say that we want to log (like a Console.Write();) what our ray hit
                                               // We wrapped our code in an if statement because the return type of the Physics.Raycast is a boolean

                Target target = hit.transform.GetComponent<Target>(); // This uses the other script we created for the target
                                                                      // what it does is get the object with the component called Target and stores it in a variable
                if (target != null) // if the target recieves the variable we want (it will be null if we hit something without the target component)
                    target.TakeDamage(damage); // then we give damage, notice that we can do this because we declared our TakeDamage method as public

                if (hit.rigidbody != null) // if the object that we hit has a rigidbody
                    hit.rigidbody.AddForce(-hit.normal * impactForce); // we apply a force to it (the addforce is negative so that it goes away from us)

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

                Destroy(impactGO, 0.5f);
                // We use instantiate to create the object, we enter what we want to instantiate, where and in what direction, hit.normal is a flat surface that points directly in front, that way our effect will always be toward its source
                // We also destroy the object 1 second after the created of it, that way we won't have millions of objects on our scene

            }
        }
    }

    void AimDownSight() //WIP
    {

    }

    IEnumerator Reload()
    {
        /*if (maxAmmo > 1)
        {
            isReloading = true;
            if (currentMag != 0)
            {
                int tmp = bulletsPerMag - currentMag;
                if (maxAmmo > tmp)
                {
                    maxAmmo -= tmp;
                    maxAmmo--;
                    currentMag += tmp;
                    currentMag++;
                }
                else
                {
                    currentMag += maxAmmo;
                    maxAmmo = 1;
                }
            }
            else
            {
                int tmp = bulletsPerMag - currentMag;
                if (maxAmmo > tmp)
                {
                    maxAmmo -= tmp;
                    currentMag += tmp;
                }
                else
                {
                    currentMag += maxAmmo;
                    maxAmmo = 1;
                }
            }
            Sounds.AK47reload(AK47, AK47reload, 0.3f);
        }*/
        mags.Reload();
        yield return new WaitForSeconds(reloadTime);
    }

    #endregion

}
#region Classes

public class Magazines
{
    #region Variables
    private int[] _mags;
    private int _index;
    #endregion

    #region Properties
    public int[] mags
    {
        get { return _mags; }
        set { _mags = value; }
    }
    public int index
    {
        get { return _index; }
        set { _index = value; }
    }
    public int this[int i]
    {
        get { return _mags[i]; }
        set { _mags[i] = value; }
    }
    #endregion

    public Magazines(int magNum, int bulletsPerMag)
    {
        FillMags(magNum, bulletsPerMag);
        _index = 0;
    }

    private void FillMags(int magNum, int bulletsPerMag)
    {
        _mags = new int[magNum];
        for (int i = 0; i < _mags.Length; i++)
        {
            _mags[i] = bulletsPerMag;
        }
    }

    public void Reload()
    {
        {
            if (_mags[_index] == 0)
            {
                SearchMag();
                GunScript.MagQty--;
            }
            else if (_mags[_index] == 1)
            {
                _mags[_index] = 0;
                GunScript.MagQty--;
                SearchMag();
                _mags[_index]++;
            }
            else
            {
                _mags[_index]--;
                SearchMag();
                _mags[_index]++;
            }
        }
    }

    private void SearchMag()
    {
        for (int i = 0; i < _mags.Length; i++)
        {
            if (_mags[i] != 0)
            {
                if (i != _index)
                {
                    _index = i;
                    return;
                }
            }
        }
    }
}
#endregion