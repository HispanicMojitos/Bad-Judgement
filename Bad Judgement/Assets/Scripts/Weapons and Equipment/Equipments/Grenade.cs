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

    #region Ctor

    public Grenade(string name, Sprite uiSprite, int amount, GameObject prefab, Transform playerPos) : base(name, uiSprite)
    {
        this.amount = amount;

        grenade = Instantiate(prefab, playerPos);
        grenade.transform.parent = null;
    }

    #endregion

    #region Methods

    //Overriden in children classes (damage or not damage)
    public virtual void ThrowGrenade(Transform startPos) //+TRANSFORM (POS DEPART)
    {
        Vector3 direction = startPos.TransformDirection(Vector3.forward) * 10F; //Setting direction

        var grenadeRb = grenade.GetComponent<Rigidbody>(); //Getting rigidbody to apply force later
        grenadeRb.AddForce(direction, ForceMode.Impulse); //Applying impulse force to grenade

        //else addingParticleEffect
    }

    public void TurnVisible(bool isVisible)
    {
        this.grenade.SetActive(isVisible);
    }


    #endregion
}
