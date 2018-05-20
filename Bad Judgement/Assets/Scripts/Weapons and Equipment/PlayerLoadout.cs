using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private Transform playerHand;

    public MainWeaponsClass[] weapons { get; private set; }
    public bool primaryWeaponIsActive { get { return weapons[0].LoadWeapon().activeInHierarchy; } }
    public bool secondaryWeaponIsActive { get { return weapons[1].LoadWeapon().activeInHierarchy; } }

    public List<Grenade> grenades { get; private set; }
    public List<ProtectionEquipment> protection { get; private set; }

    public int selectedItem { get; private set; }
    public static int maxSelectedItem { get; private set; }

    public int nbSmoke { get; private set; }
    public int nbFrag { get; private set; }
    public int nbFlash { get; private set; }

    public int nbGrd { get; private set; }

    public string[] grdTable { get; private set; }

    public int? indexSelectedGrd { get; private set; }
  
    private void Start()
    {
        selectedItem = 0;

        weapons = new MainWeaponsClass[] { null, null };
        protection = new List<ProtectionEquipment>();
        grenades = new List<Grenade>();

        grdTable = new string[] { null, null, null };

        CreateLoadout();
        maxSelectedItem = GetHowMuchGrdTypes() + 1;
    }

    private void Update()
    {
        if(!UIScript.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) OnLeftClick();
            EquipmentSelection();
        }
    }

    #region Equipment

    private void CreateLoadout()
    {
        int counterOfSlot = 0;

        if (EquipmentDB.HasHelmet()) protection.Add(new ProtectionEquipment("helmet", 25f, 100f));
        if (EquipmentDB.HasVest()) protection.Add(new ProtectionEquipment("vest", 75f, 100f));

        nbSmoke = EquipmentDB.GetGrdNumber("smoke");
        nbFlash = EquipmentDB.GetGrdNumber("flash");
        nbFrag = EquipmentDB.GetGrdNumber("frag");

        for (int i = 0; i < nbSmoke; i++)
        {
            grenades.Add(new SmokeGrenade("smoke", playerHand));
            if (i == 0) //Pour ne le faire qu'une seule fois s'il y a plusieurs grenades de ce type
            {
                grdTable[counterOfSlot] = "smoke";
                counterOfSlot++;
            }
        }

        for (int i = 0; i < nbFlash; i++)
        {
            grenades.Add(new FlashGrenade("flash", playerHand));
            if (i == 0) //Pour ne le faire qu'une seule fois s'il y a plusieurs grenades de ce type
            {
                grdTable[counterOfSlot] = "flash";
                counterOfSlot++;
            }
        }

        for (int i = 0; i < nbFrag; i++)
        {
            grenades.Add(new FragGrenade("frag", playerHand));
            if (i == 0) //Pour ne le faire qu'une seule fois s'il y a plusieurs grenades de ce type
            {
                grdTable[counterOfSlot] = "frag";
                counterOfSlot++;
            }
        }

        nbGrd = counterOfSlot + 1;
    }

    private int GetHowMuchGrdTypes()
    {
        int counter = 0;

        if (grenades != null && grenades.Count != 0)
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

        ActiveItemHandling();
    }

    private void ActiveItemHandling()
    {
        indexSelectedGrd = null;

        if(selectedItem < 2)
        {
            if (selectedItem == 0)
            {

            }//ActivatePrimary
            else
            {

            }//ActivateSecondary

            foreach (var grd in grenades.Where(g => g.throwable)) grd.DeactivateGrd();
        }
        else
        {
            indexSelectedGrd = GetGrdIndex();

            //DeactivatePrimary
            //DeactivateSecondary

            if (indexSelectedGrd >= 0)
            {
                grenades[(int)indexSelectedGrd].ActivateGrd();
                var toDeact = grenades.Where(g => g.GetType() != grenades[(int)indexSelectedGrd].GetType() && g.throwable);
                foreach (var grenade in toDeact) grenade.DeactivateGrd();
            }
        }
    }

    private void OnLeftClick()
    {
        switch (selectedItem)
        {
            case 0:
                Debug.Log("Tir principal");
                //Primary gun shot
                break;
            case 1:
                Debug.Log("Tir second");
                //Secondary gun shot
                break;
            default:
                if (indexSelectedGrd != null && grenades[(int)indexSelectedGrd].throwable)
                {
                    grenades[(int)indexSelectedGrd].ThrowGrenade();
                    grenades[(int)indexSelectedGrd].throwable = false;
                    nbGrd--;
                    Debug.Log("Throw grd");
                }
                break;
        }
    }

    public int ReturnTotalProtectionDuration()
    {
        int totalDuration = 0;

        if (protection != null && this.protection.Count != 0)
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

    /// <summary>
    /// Gets the grenade with the good type in the grenade list.
    /// </summary>
    /// <returns>Index of the searched grenade type. If it doesn't exist, it returns null</returns>
    private int? GetGrdIndex()
    {
        int equipmentNumber = selectedItem - 2; //The first equipment is the number 2 slot, but we want to use it as indexes (0, 1, 2) for an array of length 3

        var grd = grenades.FindIndex(g => g.name == grdTable[equipmentNumber] && g.throwable);

        return grd;
    }

    #endregion
}
