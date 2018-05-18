using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoadout : MonoBehaviour
{
    public List<Grenade> grenades { get; private set; }
    public List<ProtectionEquipment> protection { get; private set; }

    public int selectedItem { get; private set; }
    public static int maxSelectedItem = 4;
  
    private void Start()
    {
        selectedItem = 0;

        protection = new List<ProtectionEquipment>();

        if (EquipmentDB.HasHelmet()) protection.Add(new ProtectionEquipment("helmet", 25f, 100f));
        if (EquipmentDB.HasVest()) protection.Add(new ProtectionEquipment("vest", 75f, 100f));
    }

    private void Update()
    {
        if(!UIScript.gameIsPaused)
        {
            EquipmentSelection();
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

    private void OnLeftClick()
    {
        switch (selectedItem)
        {
            case 0:
                //Primary gun shot
                break;
            case 1:
                //Secondary gun shot
                break;
            case 2:
                //1st equip "shot"
                break;
            case 3:
                //2nd equip "shot"
                break;
            case 4: //This is the last possible case
                //3rd equip "shot"
                break;
            default:
                throw new System.Exception("Impossible to use a non-existent equipment !");
                break;
        }
    }

    public int returnTotalProtectionDuration()
    {
        int totalDuration = 0;

        if (this.protection != null)
        {
            foreach (var protectionItem in this.protection) totalDuration += Mathf.RoundToInt(protectionItem.equipmentDuration);
            totalDuration /= this.protection.Count;
        }

        return totalDuration;
    }

    #endregion
}
