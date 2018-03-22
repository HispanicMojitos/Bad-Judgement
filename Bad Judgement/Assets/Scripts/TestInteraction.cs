using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteraction : MonoBehaviour
{
    [SerializeField] private Image reloadImage;
    [SerializeField] private Image interactionImage;
    private bool porteOuverte = false;

	void Update ()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100; // Permet d'afficher le raycast
        RaycastHit hit;
        Debug.DrawLine(transform.position, direction * 3,Color.cyan);
        if ( (Physics.Raycast(transform.position, direction, out hit,3f) && hit.transform.CompareTag("gun") && Vector3.Distance(transform.position, hit.transform.position) < 3)) // Si la distance entre l'arme et le jouer est inférieur à 3, ainsi que le joueur regarde bien l'arme
        {
            reloadImage.enabled = true; // affiche l'image tant que l'on reste focalisé sur une arme
            if(Input.GetKeyDown(KeyCode.R)) // Si on appuye sur la touche R
            {
                if (GunScript.Mag.mags.Count < 6)
                {
                    int nbreDeBalles = Random.Range(4, 12);
                    Destroy(hit.transform.gameObject);
                    reloadImage.enabled = false; // Permet d'enelever l'image 
                    GunScript.Mag.mags.Enqueue(nbreDeBalles);
                }
                else
                {
                    Debug.Log("Max Mag count");
                }
            }
        }
        else if (reloadImage.enabled == true) reloadImage.enabled = false; // Permet d'empecher l'image de se réafficher par la suite sans qu'on l'ai demandé !!

        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("porte") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            interactionImage.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (porteOuverte == false)
                {
                    porteOuverte = true;
                    HingeJoint joint = hit.transform.GetComponent<HingeJoint>();
                    JointSpring jSpring = joint.spring;

                    jSpring.spring = 100;
                    jSpring.damper = 30;
                    jSpring.targetPosition = -90;
                    joint.spring = jSpring;
                    joint.useSpring = true;
                }
                else
                {
                    porteOuverte = false;
                    HingeJoint joint = hit.transform.GetComponent<HingeJoint>();
                    JointSpring jSpring = joint.spring;

                    jSpring.spring = 100;
                    jSpring.damper = 30;
                    jSpring.targetPosition = 0;
                    joint.spring = jSpring;
                    joint.useSpring = true;
                }
            }
        }
        else interactionImage.enabled = false;
    }
}
