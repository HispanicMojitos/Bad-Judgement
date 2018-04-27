using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Casque : ProtectionEquipment
{
    public Casque(string name, float protectionCoefficient, float equipmentDuration = 25F) : base(name, protectionCoefficient, equipmentDuration)
    {
    }
}
