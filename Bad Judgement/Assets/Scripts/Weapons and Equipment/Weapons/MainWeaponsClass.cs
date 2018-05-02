using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

public class MainWeaponsClass : MonoBehaviour
{
	#region Variables
	private GameObject impactEffect;
	private GameObject weapon;
	private GameObject gunEnd;
	private AudioClip reloadSound;
	private AudioClip shootSound;
	private AudioSource gunAudioSource;
	private Animator gunAnim;
	private float damage;
	private float impactForce;
	private float fireRate;
	private float nextTimeToFire = 0f;
	private int magQty;
	private Camera cam;
	private bool isAiming = false;
	#endregion

	#region Weapon Sway
	private float amount;
	private float smoothAmount;
	private float maxAmount;
	#endregion

	#region Reload
	private int bulletsPerMag;
	private int currentMag;
	private KeyCode reloadKey = KeyCode.R;
	#endregion

	/*
	public MainWeaponsClass(GameObject weapon, GameObject gunEnd, AudioClip reloadSound,
		AudioClip shootSound, Animator gunAnim, AudioSource gunAudioSource, GameObject impactEffect,
		float gunSwayAmount, float gunSwaySmooth, float gunSwayMax, int bulletsPerMag,
		float damage, float impactForce, float fireRate, int magQty)
	{

	}*/
	public MainWeaponsClass(string path)
	{

	}

	// read the folder with the guns and search the values of our variables
}
