using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    public List<Grenade> grenades { get; private set; }
    public ProtectionEquipment protection { get; private set; }

    public int selectedItem { get; private set; }
    public int maxSelectedItem { get; private set; }
  
    private void Start()
    {
        selectedItem = 0;
    }

    private void Update()
    {
        if(!UIScript.gameIsPaused)
        {
            
        }
    }

    #region Equipment

    private void EquipmentSelection()
    {
        var mouseScrollInput = Input.GetAxis("Mouse ScrollWheell");

        if (mouseScrollInput > 0)
        {
            if(selectedItem > 0) selectedItem--;
        }
        else if (mouseScrollInput < 0)
        {
            if (selectedItem < maxSelectedItem) selectedItem++;
        }     
    }

    #endregion
}
