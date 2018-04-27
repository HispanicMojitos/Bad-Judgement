using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DamagingGrenade : Grenade
{
    Damage damage;

    public DamagingGrenade(string name) : base(name)
    {
        this.damage = new Damage(100F, 0F);
    }
}
