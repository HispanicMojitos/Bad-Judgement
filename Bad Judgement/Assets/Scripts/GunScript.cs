using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;

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
    private int bulletsPerMag = 10;
    [SerializeField]
    private static int magQty = 4;// number of mags you can have
    private Vector3 initialPosition;
    [SerializeField]
    private static int currentMag;
    private Magazines mags;
    #endregion

    #region Properties
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
        aiming = GetComponent<Animation>();
        initialPosition = transform.localPosition;
        mags = new Magazines(magQty, bulletsPerMag);
        //magQty--;
        currentMag = mags[0].bullets;
    }
    // Update is called once per frame
    void Update()
    {

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

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // If the user presses the fire buttton
        { // and if the time that has passed is greater than the rate of fire
            nextTimeToFire = (Time.time * Time.timeScale) + (1f / (fireRate / 60)); // formula for fire rate
            Shoot();
        }
        if (Input.GetButton("Fire2")) // WIP
        {

        }
        if (Input.GetKeyDown(reloadKey))
        {
            Reload();
        }

    }

    #region Methods

    void Shoot() // to shoot, we will use raycasts. 
    {
        //muzzleFlash.Play();// Play the muzzle flash we create
        // An invisible ray shot from the camera to the forward direction
        // If the object is hit, we do some damage, if not, then nothing happens
        // First we need to reference the camera
        if (currentMag > 0)
        {
            currentMag--;
            RaycastHit hit; //This is a varaible that strores info of what the ray hits
            Sounds.AK47shoot(AK47, AK47shoot);  //  Joue le son !! A metre l'AK47 comme AudioSource et AK47shoot comme AudioClip
                                                /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
            Debug.DrawLine(gunEnd.transform.position, gunEnd.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur
                                                                                                  /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit))
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

                Destroy(impactGO, 0.5f);
                // We use instantiate to create the object, we enter what we want to instantiate, where and in what direction, hit.normal is a flat surface that points directly in front, that way our effect will always be toward its source
                // We also destroy the object 1 second after the created of it, that way we won't have millions of objects on our scene

            }
        }
    }

    void AimDownSight() //WIP
    {

    }

    void Reload()
    {
        Action<Magazines> reload = (mags) => {
            Magazines m = mags;
            int useMag = 0;
            for (int i = 0; i < m.maxIndex; i++) if (!m[i].isEmpty) useMag++;
            if (useMag != 0)
            {
                if (m[m.magIndex].bullets == bulletsPerMag+1) return;
                if (currentMag == 0)
                {
                    m[m.magIndex].isEmpty = true;
                    m.magIndex++;
                    Reload();
                }
                if (m.magIndex == m.maxIndex)
                {
                    m[m.magIndex].bullets = currentMag;
                    m.magIndex = 0;
                    if (m[m.maxIndex].bullets == 0)
                    {
                        m[m.maxIndex].isEmpty = true;
                        currentMag = m[m.magIndex].bullets;
                    }
                    else if (m[m.maxIndex].bullets != ++bulletsPerMag)
                    {
                        currentMag = ++m[m.magIndex].bullets;
                    }
                    Sounds.AK47reload(AK47, AK47reload, 0.3f);
                }
                else
                {
                    m[m.magIndex].bullets = currentMag;
                    m.magIndex++;
                    if (m[m.magIndex-1].bullets == 0)
                    {
                        m[m.magIndex-1].isEmpty = true;
                        currentMag = m[m.magIndex].bullets;
                    }
                    else if (--m[m.magIndex].bullets != ++bulletsPerMag)
                    {
                        currentMag = ++m[m.magIndex].bullets;
                    }
                    Sounds.AK47reload(AK47, AK47reload, 0.3f);
                }
            }
        }; reload(mags);
    }

    #endregion

}
#region Classes

public class Magazine
{
    #region Variables
    private int _bullets;
    private bool _isEmpty = true;
    #endregion

    #region Properties
    public int bullets
    {
        get { return _bullets; }
        set { _bullets = value; }
    }
    public bool isEmpty
    {
        get { return _isEmpty; }
        set { _isEmpty = value; }
    }
    #endregion

    public Magazine(int bullets)
    {
        _bullets = bullets;
    }

}

public class Magazines : IEnumerable
{
    #region Variables
    private Magazine[] _mags;
    private int _magNum;
    private int bullets;
    private Magazine mag;
    private int _magIndex;
    #endregion

    #region Properties
    public int magIndex
    {
        get { return _magIndex; }
        set { _magIndex = value; }
    }
    public int magNum
    {
        get { return _magNum; }
        private set { _magNum = value; }
    }

    public Magazine[] mags
    {
        get { return _mags; }
        set { _mags = value; }
    }

    public Magazine this[int i]
    {
        get { return _mags[i]; }
        set { _mags[i] = value; }
    }

    public int maxIndex
    {
        get { return mags.Length - 1; }// In this memorable moment I discovered that this piece of artwork bugged because I didn't add that -1
        private set { maxIndex = value; }
    }
    #endregion

    #region Interfaces
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion

    public Magazines(int magNum, int bullets)
    {
        _magNum = magNum;
        this.bullets = bullets;
        mags = fillMags(mags);
        _magIndex = 0;

    }

    public Magazine[] fillMags(Magazine[] mags)
    {
        mags = new Magazine[_magNum];
        for (int i = 0; i < mags.Length; i++)
        {
            mag = new Magazine(bullets);
            mags[i] = mag;
            mags[i].isEmpty = false;
            //_magIndex = i;

        }
        return mags;
    }
}
#endregion