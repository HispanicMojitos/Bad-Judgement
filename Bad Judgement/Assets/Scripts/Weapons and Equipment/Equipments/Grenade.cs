using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Grenade : Equipment
{
    protected int amount { get; private set; } //This is the remaining amount of grenades

    #region Ctor

    public Grenade(string name, Sprite uiSprite, int amount) : base(name, uiSprite)
    {
    }

    #endregion

    #region Methods

    //Overriden in children classes (damage or not damage)
    protected virtual void ThrowGrenade(float force, Vector3 direction, Sprite effect)
    {
        //HERE
    }

    #endregion
}
