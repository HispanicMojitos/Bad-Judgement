using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Grenade : Equipment
{
    public int amount { get; private set; } //This is the remaining amount of grenades
    public bool isDamageable { get { return this.GetType() == typeof(FragGrenade); } }

    protected GameObject grenade; //This is the actual grenade object that has to be thrown
    protected Transform player;

    #region Ctor

    public Grenade(string name, int amount, Transform playerPos) : base(name)
    {
        this.amount = amount;
        this.player = playerPos;
    }

    #endregion

    #region Methods

    //Overriden in children classes (damage or not damage)
    public virtual void ThrowGrenade()
    {
        Vector3 direction = player.TransformDirection(Vector3.forward) * 10F; //Setting direction

        var grenadeRb = grenade.GetComponent<Rigidbody>(); //Getting rigidbody to apply force later
        grenadeRb.AddForce(direction, ForceMode.Impulse); //Applying impulse force to grenade
    }

    public void SwitchingToGrenade()
    {
        Instantiate(grenade, null);
    }

    #endregion
}
