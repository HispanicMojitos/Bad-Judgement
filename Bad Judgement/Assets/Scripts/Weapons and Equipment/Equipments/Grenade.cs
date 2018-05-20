﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Grenade : Equipment
{
    public bool isDamageable { get { return this.GetType() == typeof(FragGrenade); } }
    public bool throwable { get; set; }

    protected GameObject grenadePrefab; //This is the actual grenade object that has to be thrown
    protected GameObject realGrenade;
    protected Transform player;
    protected Rigidbody grdRb;

    #region Ctor

    public Grenade(string name, Transform playerPos) : base(name)
    {
        this.player = playerPos;
        throwable = true;
    }

    public void InstanciateGrenades()
    {
        realGrenade = Instantiate(grenadePrefab, player.position, new Quaternion(0f, 0f, 0f, 0f), player);
        realGrenade.gameObject.SetActive(false);
    }

    #endregion

    #region Methods

    //Overriden in children classes (damage or not damage)
    public virtual void ThrowGrenade()
    {
        Vector3 direction = player.TransformDirection(Vector3.forward); //Setting direction
        
        var grenadeRb = realGrenade.GetComponent<Rigidbody>();
        grenadeRb.constraints = RigidbodyConstraints.None;
        grenadeRb.AddForce(direction * 0.7F, ForceMode.Impulse); //Applying impulse force to grenade
        grenadeRb.useGravity = true;
        realGrenade.transform.SetParent(null);
    }

    public void ActivateGrd() { realGrenade.SetActive(true); }
    public void DeactivateGrd() { realGrenade.SetActive(false); }

    #endregion
}
