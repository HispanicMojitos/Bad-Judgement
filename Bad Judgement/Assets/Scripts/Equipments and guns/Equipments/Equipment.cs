using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    protected new readonly string name;
    
    public Equipment(string name)
    {
        this.name = name;
    }
}
