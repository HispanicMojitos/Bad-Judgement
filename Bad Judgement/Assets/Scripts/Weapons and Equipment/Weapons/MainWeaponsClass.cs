using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.IO;
//using Newtonsoft.Json;

public class MainWeaponsClass : MonoBehaviour
{
    #region Variables
    private AudioClip[] weaponResources;
    private GameObject impactEffect;
    private GameObject muzzleFlash;
	private GameObject weapon;
	private GameObject gunEnd;
	private AudioClip reloadSound;
	private AudioClip shootSound;
	private AudioSource gunAudioSource;
    private AudioSource AK47;
    private Animator anim;
    private Camera cam;
    private float damage;
	private float impactForce;
	private float fireRate;
	private float nextTimeToFire = 0f;
	private static int magQty;
	private bool isAiming = false;
    private string path = "/ParticleEffects"; // this is not correct read contructor to see why
    private Vector3 spawnPos;
    private string name;

    #region Reload
    private int bulletsPerMag;
    private static int currentMag;
    private KeyCode reloadKey = KeyCode.R;
    private static Magazines mag;
    private bool _isReloading;
    #endregion

    #endregion

    #region Properties
    public bool IsReloading
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
    public static Magazines Mag
    {
        get { return mag; }
        set { mag = value; }
    }
    public Vector3 SpawnPos
    {
        get { return spawnPos; }
        private set { spawnPos = value; }
    }
    public int FireRate
    {
        get { return (int)fireRate; }
        private set { fireRate = value; }
    }
    public string Name
    {
        get { return name; }
        private set { name = value; }
    }
    #endregion

    /// <summary>
    /// Instance of this class must have the wepon name found in resources folder.
    /// </summary>
    /// <param name="magQty"></param>
    /// <param name="bulletsPerMag"></param>
    /// <param name="damage"></param>
    /// <param name="impactForce"></param>
    /// <param name="fireRate"></param>
    public MainWeaponsClass(int magQty, int bulletsPerMag, float damage, float impactForce, float fireRate, Vector3 spawnPos, string name)
	{
        try
        {
            weaponResources = Resources.LoadAll(string.Format("Weapons/{0}", name), typeof(AudioClip)) as AudioClip[];
            reloadSound = (AudioClip)weaponResources.Where(x => x.name == string.Format("{0}ReloadSound", name)).SingleOrDefault();
            shootSound = (AudioClip)weaponResources.Where(x => x.name == string.Format("{0}ShootSound", name)).SingleOrDefault();
            weapon = Resources.Load(string.Format("Weapons/{0}/{1}", name, name), typeof(GameObject)) as GameObject;
            impactEffect = Resources.Load("ParticleEffects/ImpactEffect", typeof(GameObject)) as GameObject;
            muzzleFlash = Resources.Load("ParticleEffects/MuzzleFlash", typeof(GameObject)) as GameObject;

            gunEnd = weapon.transform.Find("GunEnd").gameObject;
            gunAudioSource = weapon.GetComponent<AudioSource>();
            anim = weapon.GetComponent<Animator>();
            cam = weapon.GetComponentInParent<Camera>();

            this.damage = damage;
            this.impactForce = impactForce;
            this.name = name;
            mag = new Magazines(magQty, bulletsPerMag);
            this.spawnPos = spawnPos;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }
    // read the folder with the guns and search the values of our variables
    // "/" is the assets folder
    // I really like this font, everyone should use the Monaco font

    #region Methods


    #region Shoot
    /// <summary>
    /// <para>This is used with a condition as follows: </para>
    /// <para>if(Input.GetButton("Fire1") && </para> 
    /// <para>Time.time >= nextTimeToFire && </para>
    /// <para>!anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))</para>
    /// </summary>
    public void Shoot()
    {
        //muzzleFlash.Play();// Play the muzzle flash we create
        // An invisible ray shot from the camera to the forward direction
        // If the object is hit, we do some damage, if not, then nothing happens
        // First we need to reference the camera
        RaycastHit hit;
        if (mag.currentMag > 0 && !_isReloading && Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit) && hit.transform.CompareTag("Ally") == false) // PErmet ainsi d'empecher le jouer de tirer sur son allié
        {
            mag.currentMag--;
            GameObject muzlFlash = Instantiate(muzzleFlash, gunEnd.transform);
            Destroy(muzlFlash, 1.3f);
            Sounds.Cz805shootPlayer(AK47);  //  Joue le son !! A metre l'AK47 comme AudioSource et AK47shoot comme AudioClip
            Debug.DrawLine(gunEnd.transform.position, gunEnd.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur
            ProduceRay(gunEnd, hit);
        }
    }
    public void ProduceRay(GameObject gunEnd, RaycastHit hit)// shoots the ray
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
    #endregion

    #region ADS
    /// <summary>
    /// <para>Used in the following condition: </para>
    /// <para>if(Input.GetButtonDown("Fire2"))</para>
    /// </summary>
    public void AimDownSight() //WIP
    { // this should edit the recoil values and play anim
        isAiming = !isAiming;
        anim.SetBool("IsAiming", isAiming);
    }
    #endregion

    #endregion

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
            if (_mags.Count > 1)
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
}
