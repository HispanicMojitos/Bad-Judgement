using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        
    }

    public void ActivateMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        optionsMenu.SetActive(false);
    }

    public void OnApplySettingButtonPressed()
    {
        //SAVE HERE
        //WAIT
        optionsMenu.SetActive(false);
    }
}
