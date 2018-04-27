using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Knife : Equipment
{
    Damage damage; //Damage interface implementation class

    public Knife(string name) : base(name)
    {
       this.damage = new Damage(100F, 2F);
    }
}
