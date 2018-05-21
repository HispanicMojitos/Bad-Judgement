﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine.UI;

public class WeaponHandler : MonoBehaviour {

    #region Variables
    private GameObject currentWeaponGO;
    private GameObject primary;
    private GameObject secondary;

    private GameObject primaryGunEnd;
    private GameObject secondaryGunEnd;
    private GameObject currentWeaponGunEnd;

    private GameObject impactEffect;
    private GameObject muzzleFlash;
    private GameObject[] particles;

    private MainWeaponsClass currentWeapon;

    private KeyCode reloadKey = KeyCode.R;

    private bool isShootButton = false;
    private bool isReloadButton = false;
    private bool isAimButton = false;

    private bool isShooting = false;
    private bool isReloading = false;
    private bool isAiming = false;

    private bool canShoot = false;
    private bool canReload = false;

    private bool isLoaded = false;

    private int currentMag;
    private int magQty;

    private float nextTimeToFire = 0f;

    private PlayerLoadout loadout;

    private Animator primaryAnim;
    private Animator secondaryAnim;
    private Animator currentAnim;

    private AudioSource primaryAS;
    private AudioSource secondaryAS;
    private AudioSource currentAS;

    #endregion

    #region Properties
    public bool IsAiming
    {
        get { return isAiming; }
        set { IsAiming = value; }
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        loadout = transform.GetComponentInParent<PlayerLoadout>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        loadout = transform.GetComponentInParent<PlayerLoadout>();
        Debug.Log("Weapons Instantiated: " + loadout.isInstantiated);
        if (loadout.isInstantiated)
        {
            Debug.Log("Weapons Instantiated: " + loadout.isInstantiated);
            if (!isLoaded)
            {
                LoadCurrentWeaponAssets();

                loadout = transform.GetComponentInParent<PlayerLoadout>();

                primary = this.transform.Find(loadout.weapons[0].Name + "(Clone)").gameObject;
                Debug.Log(primary);
                secondary = loadout.secondary;

                primaryAnim = primary.GetComponent<Animator>();
                secondaryAnim = loadout.secondary.GetComponent<Animator>();

                primaryAS = primary.GetComponentInChildren<AudioSource>();
                secondaryAS = secondary.GetComponentInChildren<AudioSource>();

                primaryGunEnd = primary.transform.Find("GunEnd").gameObject;
                secondaryGunEnd = loadout.secondary.transform.Find("GunEnd").gameObject;

                isLoaded = true;
            }

            Debug.Log(primary);

            currentWeaponGO = primary.activeInHierarchy ? primary
                            : secondary.activeInHierarchy ? secondary
                            : null;

            Debug.Log("CurrentWeaponGO: " + currentWeaponGO);

            if (primary != null) Debug.Log("PrimaryGO: " + primary);
            else Debug.Log("PrimaryGO null");

            if (secondary != null) Debug.Log("SecondaryGO: " + secondary);
            else Debug.Log("SecondaryGO null");

            currentWeapon = primary.activeInHierarchy ? loadout.weapons[0]
                          : secondary.activeInHierarchy ? loadout.weapons[1]
                          : null;

            currentAnim = primary.activeInHierarchy ? loadout.primary.GetComponent<Animator>()
                        : secondary.activeInHierarchy ? loadout.secondary.GetComponent<Animator>()
                        : null;

            currentAS = primary.activeInHierarchy ? primary.GetComponentInChildren<AudioSource>()
                      : secondary.activeInHierarchy ? secondary.GetComponentInChildren<AudioSource>()
                      : null;

            currentWeaponGunEnd = primary.activeInHierarchy ? loadout.primary.transform.Find("GunEnd").gameObject
                                : secondary.activeInHierarchy ? loadout.secondary.transform.Find("GunEnd").gameObject
                                : null;


            if (currentWeaponGO != null)
            {
                Debug.Log("CurrentWeaponGO is not Null!");
                currentMag = currentWeapon.mag.currentMag;
                magQty = currentWeapon.mag.mags.Count;
                canReload = currentMag < currentWeapon.BulletsPerMag + 1 && magQty != 0 && !isShooting && !currentAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload");
                canShoot = !currentAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload");

                if (!UIScript.gameIsPaused)
                {
                    isShootButton = loadout.primaryWeaponIsActive ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
                    isReloadButton = Input.GetKeyDown(reloadKey);
                    isAimButton = Input.GetButtonDown("Fire2");

                    if (isShootButton && Time.time >= nextTimeToFire && canShoot)
                    { // and if the time that has passed is greater than the rate of fire
                        nextTimeToFire = (Time.time * Time.timeScale) + (1f / (currentWeapon.FireRate / 60)); // formula for fire rate
                        Shoot();
                    }

                    if (isReloadButton && canReload)
                    {
                        Reload();
                    }

                    if (isAimButton)
                    {
                        isAiming = !isAiming;
                        currentAnim.SetBool("Aiming", isAiming);
                    }
                }
            }
        }
	}

    #region Shoot Method
    void Shoot()
    {
        if (currentMag > 0 && !isReloading)
        {
            RaycastHit hit;
            isShooting = true;
            currentMag--;

            Sounds.GunShoot(currentAS, currentWeapon.Name);
            GameObject muzlFlash = Instantiate(muzzleFlash, currentWeaponGunEnd.transform);
            Destroy(muzlFlash, 1.3f);

            if (Physics.Raycast(currentWeaponGunEnd.transform.position, currentWeaponGunEnd.transform.forward, out hit))
            {
                Debug.DrawLine(currentWeaponGunEnd.transform.position, currentWeaponGunEnd.transform.forward * 500, Color.red);
                ProduceRay(currentWeaponGunEnd, hit);
            }

            currentAnim.CrossFadeInFixedTime("Fire", 0.1f);
        }
    }
    void ProduceRay(GameObject gunEnd, RaycastHit hit)// shoots the ray
    {

        Target target = hit.transform.GetComponent<Target>();

        {
            var dist = hit.collider.bounds.max.y - hit.point.y;

            if (target.tag != "player" && dist < (.16f * hit.collider.bounds.max.y)) target.TakeDamage(target.vieMax);
            else target.TakeDamage(currentWeapon.Damage);
        }

        if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * currentWeapon.ImpactForce);

        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(impactGO, 0.5f);
    }
    #endregion

    #region Reload Method
    void Reload()
    {
        if (magQty != 0 && !currentAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            isReloading = true;

            Sounds.GunReload(currentAS, currentWeapon.Name);

            isReloading = false;

            currentWeapon.mag.Reload();

            currentAnim.CrossFade("Reload", 0.1f);
        }
    }
    #endregion

    public void LoadCurrentWeaponAssets()
    {
        if (currentWeapon != null)
        {
            particles = currentWeapon.LoadParticles();
            muzzleFlash = Resources.Load(@"ParticleEffects\MuzzleFlash", typeof(GameObject)) as GameObject;
            impactEffect = Resources.Load(@"ParticleEffects\HitSparks", typeof(GameObject)) as GameObject;
        }
    }
}
