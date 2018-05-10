
using System;
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
    [SerializeField] private AudioSource AK47; // AK47 qui est la source de son propre son
    #endregion Sounds members

    #region Weapon variables
    [SerializeField] private float damage = 10f; // First we declare our needed variables
    [SerializeField] private float impactForce = 30f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private GameObject gunEnd; // camera reference
    [SerializeField] private Animator anim;
    //public ParticleSystem muzzleFlash; // this will search for the muzzle flash particle system we'll add
    [SerializeField] private GameObject impactEffect; // So this one is also a particle effect but we want to reference it as an object so that we can place it inside our game world 
    [SerializeField] private GameObject muzzleFlash;
    #endregion

    #region Gun Sway
    [SerializeField] private float amount;
    [SerializeField] private float smoothAmount;
    [SerializeField] private float maxAmount;
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
    private Vector3 initialPosition;
    private bool _isReloading = false;
    private float reloadTime;
    private static Magazines mag;
    private Camera cam;
    private bool isAiming = false;

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
    public static Magazines Mag
    {
        get { return mag; }
        set { mag = value; }
    }
    #endregion

    void Start()
    {
        initialPosition = transform.localPosition;
        mag = new Magazines(magQty, bulletsPerMag);
        cam = GetComponentInParent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        #region Refresh values
        currentMag = mag.currentMag;
        magQty = mag.mags.Count;
        //LookAtScreen();
        #endregion

        #region Reload Condition
        if (Input.GetKeyDown(reloadKey) && !isReloading && mag.currentMag<bulletsPerMag+1 && magQty!=0)
        {
            //StartCoroutine(Reload());
            Reload();
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

        #region Shooting Condition
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !anim.GetCurrentAnimatorStateInfo(0).IsName("Reload")) // If the user presses the fire buttton
        { // and if the time that has passed is greater than the rate of fire
            nextTimeToFire = (Time.time * Time.timeScale) + (1f / (fireRate / 60)); // formula for fire rate
            Shoot();
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
        RaycastHit hit;

        if (( mag.currentMag > 0  && Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit) && !isReloading && (hit.transform.CompareTag("Ally") == false)) ) // PErmet ainsi d'empecher le jouer de tirer sur son allié
        {
            mag.currentMag--;
            Sounds.Cz805shootPlayer(AK47);  //  Joue le son !! A metre l'AK47 comme AudioSource et AK47shoot comme AudioClip
                                            /// /!\ A enlever lors de la demonstration du jeux, ce bout de code n'est utile que pour aider a se retrouver avec le raycast
              
            GameObject muzlFlash = Instantiate(muzzleFlash, gunEnd.transform);
            Destroy(muzlFlash, 1.3f);
            Debug.DrawLine(gunEnd.transform.position, gunEnd.transform.forward * 500, Color.red); // Ici Debug.Drawlin permet de montrer le raycast, d'abord on entre l'origine du ray, apres on lui met sa fait (notemment ici a 500 unité), et on peut ensuite lui entrer une Couleur


            ProduceRay(gunEnd, hit);
        }
        else // Ici si on tire dans le vdie, on tirra quand même, et même si on tire sur l'allié, il ne recevra pas de degat !
        {
             mag.currentMag--;
             Sounds.Cz805shootPlayer(AK47);  //  Joue le son !! A metre l'AK47 comme AudioSource et AK47shoot comme AudioClip
             GameObject muzlFlash = Instantiate(muzzleFlash, gunEnd.transform);
             Destroy(muzlFlash, 1.3f);
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
                target.TakeDamage(damage); // then we give damage, notice that we can do this because we declared our TakeDamage method as public

            if (hit.rigidbody != null) // if the object that we hit has a rigidbody
                hit.rigidbody.AddForce(-hit.normal * impactForce); // we apply a force to it (the addforce is negative so that it goes away from us)

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(impactGO, 0.5f);
            // We use instantiate to create the object, we enter what we want to instantiate, where and in what direction, hit.normal is a flat surface that points directly in front, that way our effect will always be toward its source
            // We also destroy the object 1 second after the created of it, that way we won't have millions of objects on our scene
            
    }

    void LookAtScreen()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;
        var direction = new Vector3(screenX, screenY);
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(direction);

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);
        }
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
            isAiming = false;
            anim.SetBool("Aiming", isAiming);
            isReloading = true;

            anim.SetTrigger("Reload");
            Sounds.AK47reload(AK47);

            isReloading = false;
            mag.Reload();
        }
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