using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ProtectionEquipment : Equipment
{
    protected float equipmentDuration { get; private set; }
    protected float protectionCoefficient { get; set; }
    protected float maxDuration { get; private set; }

    public ProtectionEquipment(string name, float protectionCoefficient, float equipmentDuration) : base(name)
    {
        if (protectionCoefficient < 0F) protectionCoefficient = 0F;
        if (protectionCoefficient > 100F) protectionCoefficient = 100F;
        this.protectionCoefficient = protectionCoefficient;

        this.equipmentDuration = equipmentDuration;
    }
}
