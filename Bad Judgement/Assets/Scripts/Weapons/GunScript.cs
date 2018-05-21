
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    #region Variables

    #region Serialized Fields

    #region Sounds members
    private AudioSource AK47; // AK47 qui est la source de son propre son
    #endregion Sounds members

    #region Weapon variables
    [SerializeField] private float damage = 10f; // First we declare our needed variables
    [SerializeField] private float impactForce = 30f;
    [SerializeField] private float fireRate = 15f;
    private GameObject gunEnd; // camera reference
    private Animator anim;
    //public ParticleSystem muzzleFlash; // this will search for the muzzle flash particle system we'll add
    private GameObject impactEffect; // So this one is also a particle effect but we want to reference it as an object so that we can place it inside our game world 
    private GameObject muzzleFlash;
    #endregion

    #region Reload
    [SerializeField] private int bulletsPerMag = 30;
    [SerializeField] private static int currentMag;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    int reloadHash;
    #endregion

    #endregion

    private float nextTimeToFire = 0f;
    private static int magQty = 6;// number of mags you can have
    private bool _isReloading = false;
    private static bool _isShooting = false;
    private float reloadTime;
    private static Magazines mag;
    private Camera cam;
    private static bool isAiming = false;
    private Vector3 velocity = Vector3.zero;
    private Inaccuracy inacc;
    private Transform[] weapons;
    private Transform weapon;


    #region Recoil
    private float xKick = 0f;
    private float yKick = 0f;
    private float kickForce = 0.8f;
    private Vector3 initialRotation;
    #endregion

    #endregion

    #region Properties
    public bool isReloading
    {
        get { return _isReloading; }
        set { _isReloading = value; }
    }
    public static bool isShooting
    {
        get { return _isShooting; }
        set { _isShooting = value; }
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
    public static Magazines Mag
    {
        get { return mag; }
        set { mag = value; }
    }
    public static bool IsAiming
    {
        get { return isAiming; }
        set { IsAiming = value; }
    }
    #endregion

    void Start()
    {
        initialRotation = transform.localEulerAngles;
        mag = new Magazines(magQty, bulletsPerMag);
        cam = GetComponentInParent<Camera>();
        gunEnd = transform.Find("GunEnd").gameObject;
        anim = transform.GetComponent<Animator>();
        AK47 = transform.GetComponentInChildren<AudioSource>();
        anim.SetTrigger("TakeIn");
        impactEffect = Resources.Load(@"ParticleEffects\HitSparks", typeof(GameObject)) as GameObject;
        muzzleFlash = Resources.Load(@"ParticleEffects\MuzzleFlash", typeof(GameObject)) as GameObject;
        inacc = transform.GetComponentInParent<Inaccuracy>();
        DestroyImmediate(inacc);
        Inaccuracy sc = gameObject.AddComponent<Inaccuracy>() as Inaccuracy;
    }
    // Update is called once per frame
    void Update()
    {
        if (!UIScript.gameIsPaused)
        {
            #region Refresh values
            currentMag = mag.currentMag;
            magQty = mag.mags.Count;
            //LookAtScreen();
            #endregion

            #region Reload Condition
            if (Input.GetKeyDown(reloadKey) && !isReloading && mag.currentMag < bulletsPerMag + 1 && magQty != 0 && !Input.GetButton("Fire1"))
            {
                //StartCoroutine(Reload());
                Reload();
            }
            #endregion

            #region Shooting Condition
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !anim.GetCurrentAnimatorStateInfo(0).IsName("Reload")) // If the user presses the fire buttton
            { // and if the time that has passed is greater than the rate of fire
                nextTimeToFire = (Time.time * Time.timeScale) + (1f / (fireRate / 60)); // formula for fire rate
                Shoot();
            }
            else
            {
                _isReloading = false;
                //cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(0,0), kickForce * Time.deltaTime);
            }
            #endregion

            #region Aiming condition
            if (Input.GetButtonDown("Fire2")) // WIP
            {
                isAiming = !isAiming;
                anim.SetBool("Aiming", isAiming);
            }
            #endregion 
        }
    }

    #region Methods

    #region Shoot Script
    /// <summary>
    /// Add Shoot() summary here
    /// </summary>
    void Shoot() // to shoot, we will use raycasts. 
    {
        //muzzleFlash.Play();// Play the muzzle flash we create
        // An invisible ray shot from the camera to the forward direction
        // If the object is hit, we do some damage, if not, then nothing happens
        // First we need to reference the camera
        if (mag.currentMag > 0 && !isReloading)
        {
            RaycastHit hit;
            _isShooting = true;
			mag.currentMag--;
            Sounds.GunShoot(AK47, this.name);
			//Sounds.Cz805shootPlayer(AK47);
			GameObject muzlFlash = Instantiate(muzzleFlash, gunEnd.transform);
			Destroy(muzlFlash, 1.3f);

            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit)) // PErmet ainsi d'empecher le jouer de tirer sur son allié
            {
                /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
                Debug.DrawLine(gunEnd.transform.position, gunEnd.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur
                ProduceRay(gunEnd, hit);
            }
            anim.CrossFadeInFixedTime("Fire", 0.1f);
        }
    }
    /// <summary>
    /// Produces ray and deals damage if the Target Component is found.
    /// </summary>
    /// <param name="gunEnd"></param>
    /// <param name="hit"></param>
    void ProduceRay(GameObject gunEnd, RaycastHit hit)// shoots the ray
    {
        //Debug.Log(hit.transform.name); // So this is how to shoot a ray, Physics.Raycast asks for starting postion which is the camera, where to shoot it (forward from the camera) and what to gather (hit)
        // We then say that we want to log (like a Console.Write();) what our ray hit
        // We wrapped our code in an if statement because the return type of the Physics.Raycast is a boolean

        Target target = hit.transform.GetComponent<Target>(); // This uses the other script we created for the target
                                                              // what it does is get the object with the component called Target and stores it in a variable
        if (target != null) // if the target recieves the variable we want (it will be null if we hit something without the target component)
        {
            var dist = hit.collider.bounds.max.y - hit.point.y;

            if (target.tag != "player" && dist < (.16f * hit.collider.bounds.max.y)) target.TakeDamage(target.vieMax * 0.3333333f);
            else target.TakeDamage(damage); // then we give damage, notice that we can do this because we declared our TakeDamage method as public
        }

        if (hit.rigidbody != null) // if the object that we hit has a rigidbody
            hit.rigidbody.AddForce(-hit.normal * impactForce); // we apply a force to it (the addforce is negative so that it goes away from us)

        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(impactGO, 0.5f);
        // We use instantiate to create the object, we enter what we want to instantiate, where and in what direction, hit.normal is a flat surface that points directly in front, that way our effect will always be toward its source
        // We also destroy the object 1 second after the created of it, that way we won't have millions of objects on our scene
    }


    #endregion

    #region Aiming Script
    void AimDownSight() //WIP
    {

    }
    #endregion

    #region Reload Script
    void Reload()
    {
        if (magQty != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            //isAiming = false;
            //anim.SetBool("Aiming", isAiming);

                isReloading = true;
            Sounds.GunReload(AK47, this.name);
                //Sounds.AK47reload(AK47);

                isReloading = false;
                mag.Reload();
            anim.CrossFade("Reload", 0.1f);
        }
    }
    #endregion

    #region Recoil Script
    public void Recoil()
    {

    }
    #endregion

    #endregion

}
#region Classes

#region Mags
public class Magazines
{
    private Queue<int> _mags = new Queue<int>();
    private int crtMag;

    public Queue<int> mags
    {
        get { return _mags; }
        set { _mags = value; }
    }

    public int currentMag
    {
        get { return crtMag; }
        set { crtMag = value; }
    }

    public Magazines(int magNum, int bulletsPMag)
    {
        for (int i = 0; i < magNum; i++) _mags.Enqueue(bulletsPMag);
        crtMag = _mags.Dequeue();
    }

    public void Reload()
    {
        if (_mags.Count>1) 
        {
            switch (crtMag)
            {
                case 0:
                    crtMag = _mags.Dequeue();
                    //GunScript.MagQty--;
                    break;
                case 1:
                    crtMag = _mags.Dequeue() + 1;
                    //GunScript.MagQty--;
                    break;
                default:
                    crtMag--;
                    _mags.Enqueue(crtMag);
                    crtMag = _mags.Dequeue() + 1;
                    break;
            }
        }
    }
}
#endregion

#endregion