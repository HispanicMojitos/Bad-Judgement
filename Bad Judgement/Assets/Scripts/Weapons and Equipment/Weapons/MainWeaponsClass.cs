using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.IO;
//using Newtonsoft.Json;

[System.Serializable] // you need this to be able to serialize into json stuff
public class MainWeaponsClass
{
    private AudioClip[] weaponResources;
    private GameObject impactEffect;
    private GameObject muzzleFlash;

    //Public Variables (Becuase Unity doesn't support getters and setters in serialized objects)
    public string Name;
    public float Damage;
    public int FireRate;
    public float ImpactForce;
    public Vector3 SpawnPos;
    public Magazines mag;
    public int BulletsPerMag;

    /// <summary>
    /// Instance of this class must have the wepon name found in resources folder.
    /// </summary>
    /// <param name="magQty"></param>
    /// <param name="bulletsPerMag"></param>
    /// <param name="damage"></param>
    /// <param name="impactForce"></param>
    /// <param name="fireRate"></param>
    public MainWeaponsClass(int magQty, int bulletsPerMag, float damage, float impactForce, int fireRate, Vector3 spawnPos, string newName)
    {
        this.Damage = damage;
        this.FireRate = fireRate;
        this.ImpactForce = impactForce;
        this.Name = newName;
        this.SpawnPos = spawnPos;
        this.BulletsPerMag = bulletsPerMag;
        mag = new Magazines(magQty, bulletsPerMag);
    }
    // read the folder with the guns and search the values of our variables
    // "/" is the assets folder
    // I really like this font, everyone should use the FiraCode Retina font

    public AudioClip[] LoadAudio()
    {
        AudioClip[] array = Resources.LoadAll<AudioClip>(string.Format("Weapons/{0}", Name));
        return array;
    }

    public GameObject LoadWeapon()
    {
        return Resources.Load<GameObject>(string.Format(@"Weapons\{0}\{1}", Name, Name));
    }

    public GameObject[] LoadParticles()
    {
        GameObject[] particles =
        {
            Resources.Load(@"ParticleEffects\HitSparks", typeof(GameObject)) as GameObject,
            Resources.Load(@"ParticleEffects\MuzzleFlash", typeof(GameObject)) as GameObject
        };

        return particles;
    }

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
            if(_mags != null)crtMag = _mags.Dequeue();
        }

        public void Reload()
        {
            if (_mags.Count > 1 && _mags != null)
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
}