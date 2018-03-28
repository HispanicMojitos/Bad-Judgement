using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteraction : MonoBehaviour
{
    [SerializeField] private Image reloadImage;
    [SerializeField] private Image interactionImage;
    [SerializeField] private AudioClip porteFerme;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;


    void Update()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;
        Debug.DrawLine(transform.position, direction * 3, Color.cyan); // Permet d'afficher le raycast
        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("gun") && Vector3.Distance(transform.position, hit.transform.position) < 3)) // Si la distance entre l'arme et le jouer est inférieur à 3, ainsi que le joueur regarde bien l'arme
        {
            reloadImage.enabled = true; // affiche l'image tant que l'on reste focalisé sur une arme
            if (Input.GetKeyDown(KeyCode.R)) // Si on appuye sur la touche R
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
                HingeJoint joint = hit.transform.GetComponent<HingeJoint>();
                JointSpring jSpring = joint.spring;
                if (jSpring.targetPosition == 0)
                {

                    hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                    jSpring.spring = 100;
                    jSpring.damper = 30;
                    jSpring.targetPosition = -90;
                    joint.spring = jSpring;
                    joint.useSpring = true;
                    Sounds.PlayDoorSond(hit.transform.GetComponent<AudioSource>(), openDoor);
                }
                else if (jSpring.targetPosition == -90)
                {
                    StartCoroutine(Attend(hit));
                    jSpring.spring = 100;
                    jSpring.damper = 30;
                    jSpring.targetPosition = 0;
                    joint.spring = jSpring;
                    joint.useSpring = true;
                    Sounds.PlayDoorSond(hit.transform.GetComponent<AudioSource>(), closeDoor);
                }
            }
        }
        else if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("porteFerme") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            interactionImage.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) Sounds.PlayDoorSond(hit.transform.GetComponent<AudioSource>(), porteFerme);
        }
        else interactionImage.enabled = false;
    }

    IEnumerator Attend(RaycastHit h)
    {
        yield return new WaitForSeconds(1.5f);
        h.transform.GetComponent<Rigidbody>().isKinematic = true;
    }
    
}