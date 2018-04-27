using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Gilet : ProtectionEquipment
{
    public Gilet(string name, float protectionCoefficient, float equipmentDuration = 75F) : base(name, protectionCoefficient, equipmentDuration)
    {
    }
}
