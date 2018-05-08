using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SmokeGrenade : Grenade
{
    public SmokeGrenade(string name, Sprite uiSprite, int amount, GameObject prefab, Transform playerPos) : base(name, uiSprite, amount, prefab, playerPos)
    {
    }

    #region Methods

    protected override void ThrowGrenade(Transform startPos)
    {
        base.ThrowGrenade(startPos);
    }

    #endregion
}
