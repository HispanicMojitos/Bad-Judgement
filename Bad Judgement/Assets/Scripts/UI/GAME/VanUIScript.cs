using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VanUIScript : MonoBehaviour
{
    [SerializeField]private Text KToEquipText;
    [SerializeField]private GameObject panelMainPC;
    [SerializeField]private GameObject program;

    private bool isPrimaryPanel;
    private bool isSecondaryPanel;

    #region Program Objects

    [SerializeField] private GameObject equipmentChoicePanel;
    [SerializeField] private GameObject weaponsChoicePanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Text creditsText;

    #endregion

    #region Loadout_Objects

    ChooseLoadout loadout;

    #endregion

    #region Start & Update

    void Start()
    {
        loadout = new ChooseLoadout();
        loadout.Init();
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

            if(program.activeSelf)
            {
                creditsText.text = string.Format("Credits : {0} / {1}", loadout.credits, loadout.maxCredits);

                if (loadout.chosenPrimaryWeapon != null /*&& loadout.chosenSecondaryWeapon != null*/) playButton.interactable = true;
                else playButton.interactable = false;
            }
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
        loadout.SelectThatWeapon();
    }

    public void ShowNextWeapon()
    {
        loadout.NextWeapon();
    }
    
    public void ShowPreviousWeapon()
    {
        loadout.PreviousWeapon();
    }

    public void OnPlayButtonPressed()
    {
        loadout.Save();
        //Load scene
    }

    #endregion
}
