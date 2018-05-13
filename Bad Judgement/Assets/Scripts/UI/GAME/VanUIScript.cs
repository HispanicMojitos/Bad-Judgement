using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VanUIScript : MonoBehaviour
{
    [SerializeField]private Text KToEquipText;
    [SerializeField]private GameObject panelMainPC;
    [SerializeField]private GameObject program;

    #region Program Objects

    [SerializeField] private GameObject equipmentChoicePanel;
    [SerializeField] private GameObject weaponsChoicePanel;

    private static bool weaponChoiceIsPrimary = true;
    private static int weaponNumberShowed = 0;
    #endregion

    #region Loadout Objects

    ChooseLoadout loadout;

    #endregion

    #region Start & Update

    void Start()
    {
        loadout = new ChooseLoadout();
    }

    void Update()
    {
        if (VanCamControl.isSeating)
        {
            KToEquipText.gameObject.SetActive(true);
            panelMainPC.SetActive(false);
        }
        else
        {
            KToEquipText.gameObject.SetActive(false);
            panelMainPC.SetActive(true);
        }
    }

    #endregion

    #region EquipmentProgram

    public void StartProgram()
    {
        program.SetActive(true);
    }

    public void OnWeaponButtonClick()
    {
        equipmentChoicePanel.SetActive(false);
        weaponsChoicePanel.SetActive(true);
    }

    public void OnEquipmentButtonClick()
    {
        equipmentChoicePanel.SetActive(true);
        weaponsChoicePanel.SetActive(false);
    }

    public void OnEquipWeaponPress()
    {
        if (weaponChoiceIsPrimary)
        {
            
        }
        else
        {

        }
    }

    #endregion
}
