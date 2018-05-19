using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private Transform playerHand;

    public List<Grenade> grenades { get; private set; }
    public List<ProtectionEquipment> protection { get; private set; }

    public int selectedItem { get; private set; }
    public static int maxSelectedItem { get; private set; }

    public int nbSmoke { get; private set; }
    public int nbFrag { get; private set; }
    public int nbFlash { get; private set; }
  
    private void Start()
    {
        selectedItem = 0;
        maxSelectedItem = GetHowMuchGrdTypes();
        
        protection = new List<ProtectionEquipment>();
        grenades = new List<Grenade>();

        if (EquipmentDB.HasHelmet()) protection.Add(new ProtectionEquipment("helmet", 25f, 100f));
        if (EquipmentDB.HasVest()) protection.Add(new ProtectionEquipment("vest", 75f, 100f));

        nbSmoke = EquipmentDB.GetGrdNumber("smoke");
        nbFlash = EquipmentDB.GetGrdNumber("flash");
        nbFrag = EquipmentDB.GetGrdNumber("frag");

        for (int i = 0; i < nbSmoke; i++) grenades.Add(new SmokeGrenade("smoke", playerHand));
        for (int i = 0; i < nbFlash; i++) grenades.Add(new FlashGrenade("flash", playerHand));
        for (int i = 0; i < nbFrag; i++) grenades.Add(new FragGrenade("frag", playerHand));
    }

    private void Update()
    {
        if(!UIScript.gameIsPaused)
        {
            EquipmentSelection();
            Debug.Log(selectedItem);
            if (Input.GetKeyDown(KeyCode.Mouse0)) ; //OnLeftClick();
        }
    }

    #region Equipment

    private int GetHowMuchGrdTypes()
    {
        int counter = 1;

        if (grenades != null)
        {
            if (grenades.Exists(g => g.GetType() == typeof(FlashGrenade))) counter++;
            if (grenades.Exists(g => g.GetType() == typeof(SmokeGrenade))) counter++;
            if (grenades.Exists(g => g.GetType() == typeof(FragGrenade))) counter++; 
        }

        return counter;
    }

    private void EquipmentSelection()
    {
        var mouseScrollInput = Input.GetAxis("Mouse ScrollWheell");

        if (mouseScrollInput > 0)
        {
            if (selectedItem > 0) selectedItem--;
            else selectedItem = maxSelectedItem;
        }
        else if (mouseScrollInput < 0)
        {
            if (selectedItem < maxSelectedItem) selectedItem++;
            else selectedItem = 0;
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

    public int ReturnTotalProtectionDuration()
    {
        int totalDuration = 0;

        if (this.protection.Count != 0)
        {
            foreach (var protectionItem in this.protection) totalDuration += Mathf.RoundToInt(protectionItem.equipmentDuration);
            totalDuration /= this.protection.Count;
        }

        return totalDuration;
    }

    public int ReturnProtectionCoefficient()
    {
        float totalCoeff = 0;
        if( protection != null) foreach (var prot in protection) totalCoeff += prot.protectionCoefficient;

        return Mathf.RoundToInt(totalCoeff);
    }

    #endregion
}
