using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine.UI;

public class WeaponHandler : MonoBehaviour {

    #region Variables
    private GameObject currentWeaponGO;
    private GameObject impactEffect;
    private GameObject muzzleFlash;

    private MainWeaponsClass currentWeapon;

    private KeyCode reloadKey = KeyCode.R;

    private bool isShooting = false;
    private bool isReloading = false;
    private bool isAiming = false;


    #endregion

    // Use this for initialization
    void Start ()
    {
        impactEffect = Resources.Load(@"ParticleEffects\HitSparks", typeof(GameObject)) as GameObject;
        muzzleFlash = Resources.Load(@"ParticleEffects\MuzzleFlash", typeof(GameObject)) as GameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
