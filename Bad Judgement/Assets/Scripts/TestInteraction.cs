using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteraction : MonoBehaviour
{
    [SerializeField] private Image reloadImage;
    
	void Update ()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;
        Debug.DrawLine(transform.position, direction * 3,Color.cyan);
        if ( (Physics.Raycast(transform.position, direction, out hit,3f) && hit.transform.CompareTag("gun") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            reloadImage.enabled = true;
            if(Input.GetKeyDown(KeyCode.R) )
            {
                Destroy(hit.transform.gameObject);
                reloadImage.enabled = false;
                // Ici ajouter un montant de recharge de l'arme Actuelle
            }
        }
        else if (reloadImage.enabled == true) reloadImage.enabled = false;
    }
}
