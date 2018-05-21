 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VanUIScript : MonoBehaviour
{
    [SerializeField] private Text KToEquipText;
    [SerializeField]private GameObject panelMainPC;
    [SerializeField]private GameObject program;

    #region Program Objects

    [SerializeField] private GameObject equipmentChoicePanel;
    [SerializeField] private GameObject weaponsChoicePanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Text creditsText;

    [SerializeField] private Toggle vestSelected;
    [SerializeField] private Toggle helmetSelected;

    [SerializeField] private Text flashGrdText;
    [SerializeField] private Text smokeGrdText;
    [SerializeField] private Text fragGrdText;

    [SerializeField] private Image previewImage;
    [SerializeField] private Text previewWeaponName;
    [SerializeField] private Text isThisWeaponSelected;

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

            UpdateCredits();
            UpdateAmountGrenades();
            UpdateProtectionCheckboxes();
            UpdateWeaponImage();
        }
    }

    #endregion

    #region EquipmentProgram

    private void UpdateWeaponImage()
    {
        if (loadout.isChoosingPrimary)
        {
            previewImage.sprite = loadout.allPrimaryWeaponSprites[loadout.actualPrimaryWeaponDisplayed];
            previewWeaponName.text = loadout.allPrimaryWeapons[loadout.actualPrimaryWeaponDisplayed].Name;

            if (loadout.actualPrimaryWeaponDisplayed == loadout.actualWeaponSelected && loadout.weaponSelectedIsPrimary) isThisWeaponSelected.gameObject.SetActive(true);
            else isThisWeaponSelected.gameObject.SetActive(false);
        }
        else if (loadout.isChoosingSecondary)
        {
            previewImage.sprite = loadout.allSecondaryWeaponSprites[loadout.actualSecondaryWeaponDisplayed];
            previewWeaponName.text = loadout.allSecondaryWeapons[loadout.actualSecondaryWeaponDisplayed].Name;

            if (loadout.actualSecondaryWeaponDisplayed == loadout.actualWeaponSelected && !loadout.weaponSelectedIsPrimary) isThisWeaponSelected.gameObject.SetActive(true);
            else isThisWeaponSelected.gameObject.SetActive(false);
        }
    }

    private void UpdateCredits()
    {
        if (program.activeSelf)
        {
            creditsText.text = string.Format("Credits : {0} / {1}", loadout.credits, loadout.maxCredits);

            if (loadout.chosenWeapon != null) playButton.interactable = true;
            else playButton.interactable = false;
        }
    }

    private void UpdateAmountGrenades()
    {
        flashGrdText.text = loadout.amountFlashGrenade.ToString();
        smokeGrdText.text = loadout.amountSmokeGrenade.ToString();
        fragGrdText.text = loadout.amountFragGrenade.ToString();
    }

    private void UpdateProtectionCheckboxes()
    {
        if (loadout.credits == 0 && !vestSelected.isOn) vestSelected.interactable = false;
        else vestSelected.interactable = true;

        if (loadout.credits == 0 && !helmetSelected.isOn) helmetSelected.interactable = false;
        else helmetSelected.interactable = true;
    }

    public void StartProgram()
    {
        program.SetActive(true);
    }

    public void OnSecondaryWeaponClick()
    {
        WeaponPanelDisplay();
        loadout.isChoosingSecondary = true;
        loadout.isChoosingPrimary = false;
    }

    public void OnPrimaryWeaponClick()
    {
        WeaponPanelDisplay();
        loadout.isChoosingPrimary = true;
        loadout.isChoosingSecondary = false;
    }

    private void WeaponPanelDisplay()
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
        Teleport.WithName(MissionSave.GetActualMissionName());
    }
    
    public void AddSmoke() { loadout.AddGrenade("smoke"); }
    public void RetrieveSmoke() { loadout.RemoveGrenade("smoke"); }
    public void AddFlash() { loadout.AddGrenade("flash"); }
    public void RemoveFlash() { loadout.RemoveGrenade("flash"); }
    public void AddFrag() { loadout.AddGrenade("frag"); }
    public void RemoveFrag() { loadout.RemoveGrenade("frag"); }

    public void SelectOrUnselectVest(bool value) { loadout.SelectOrUnSelectProtection(value, "vest"); }
    public void SelectOrUnselectHelmet(bool value) { loadout.SelectOrUnSelectProtection(value, "helmet"); }

    #endregion
}
