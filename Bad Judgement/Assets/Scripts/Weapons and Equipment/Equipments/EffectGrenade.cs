using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EffectGrenade : Grenade
{
    public EffectGrenade(string name, Sprite uiSprite, int amount) : base(name, uiSprite, amount)
    {
    }

    #region Methods

    protected override void ThrowGrenade(float force, Vector3 direction, Sprite effect)
    {
        base.ThrowGrenade(force, direction, effect);
    }

    #endregion
}