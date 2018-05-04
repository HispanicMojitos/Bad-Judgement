using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteraction : MonoBehaviour
{
    [SerializeField] private Image Crosshair;
    [SerializeField] private Image CrosshairAlly;
    [SerializeField] private Image CrosshairEnemy;
    [SerializeField] private Image CanShoot;
    [SerializeField] private Image CannootShoot;
    [SerializeField] private Image alliesInterraction;
    [SerializeField] private Image reloadImage;
    [SerializeField] private Image interactionImage;
    [SerializeField] private AudioClip porteFerme;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;
    [SerializeField] private GameObject visualisationCibleDeplacement;
    public GameObject visualisationCiblePosition;
    private bool vaDeplacerAllié = false;
    private bool deplacementAllié = false;
    [HideInInspector] public Transform emplacementCible;
    private Transform alliéChosi;
    [SerializeField] private List<Transform> alliés;

    private void Start()
    {
        emplacementCible = visualisationCiblePosition.transform;
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;
        Debug.DrawLine(transform.position, direction * 3, Color.cyan); // Permet d'afficher le raycast

        if(Input.GetKeyDown(KeyCode.O))
        {
            foreach (Transform al in alliés)
            {
                if (al.GetComponent<AIally>().autoriséATirer == true)
                {
                    al.GetComponent<AIally>().autoriséATirer = false;
                    CanShoot.enabled = false;
                    CannootShoot.enabled = true;
                }
                else if (al.GetComponent<AIally>().autoriséATirer == false)
                {
                    al.GetComponent<AIally>().autoriséATirer = true;
                    CanShoot.enabled = true;
                    CannootShoot.enabled = false;
                }
            }
        }

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
        else if (reloadImage.enabled == true ) reloadImage.enabled = false; // Permet d'empecher l'image de se réafficher par la suite sans qu'on l'ai demandé !!

        if (vaDeplacerAllié == false && Physics.Raycast(transform.position, direction, out hit, 50f) && hit.transform.CompareTag("Ally"))
        {
            alliesInterraction.enabled = true;

            if (Input.GetKeyDown(KeyCode.T)) // Ici on choisit l'allié a bouger
            {
                vaDeplacerAllié = true;
                alliéChosi = hit.transform;
            }

        }
        else if (alliesInterraction.enabled == true) alliesInterraction.enabled = false;

        if (vaDeplacerAllié == true)
        {
            if (Physics.Raycast(transform.position, direction, out hit) && hit.transform.CompareTag("Ground")) // Permet d'afficher zau sol l'emplacement souhaité ou envoyer l'allié
            {
                alliesInterraction.enabled = true;
                if (visualisationCibleDeplacement.activeSelf == false) visualisationCibleDeplacement.SetActive(true);
                visualisationCibleDeplacement.transform.position = hit.point + new Vector3(0, 4f, 0);

                if(Input.GetKeyDown(KeyCode.T)) // Permet De dire a l'allié de se placer a cet en placement
                {
                    vaDeplacerAllié = false;
                    deplacementAllié = true;
                    alliesInterraction.enabled = false;
                    visualisationCibleDeplacement.SetActive(false);
                    emplacementCible.position =  hit.point;
                    alliéChosi.GetComponent<AIally>().ordreDeplacement = true;
                }
            }
            else
            {
                alliesInterraction.enabled = false;
                visualisationCibleDeplacement.SetActive(false);
            }
        }
        else if(deplacementAllié == true)
        {
            visualisationCiblePosition.transform.position = emplacementCible.transform.position + new Vector3(0, 4f, 0);
            visualisationCiblePosition.SetActive(true);
            deplacementAllié = false;
        }
        else if(visualisationCibleDeplacement.activeSelf == true || visualisationCiblePosition.activeSelf == true) // Permet d'enlever le visuel des emplacement lorsque on en a pas besoin
        {
            visualisationCibleDeplacement.SetActive(false);
        }

        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("porte") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            interactionImage.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) // Si on regarde une porte ET que l'on appuye sur E
            {
                HingeJoint joint = hit.transform.GetComponent<HingeJoint>(); // Permet de récupérer le Hinge Joint
                JointSpring jSpring = joint.spring;
                if (jSpring.targetPosition <= 3 && jSpring.targetPosition >= 0) // Si la porte est fermée, on l'ouvre
                {
                    hit.rigidbody.isKinematic = false;
                    StartCoroutine(Attend(hit));
                    jSpring.spring = 150;
                    jSpring.damper = 30;
                    jSpring.targetPosition = -90; // Grace au HingeJoint et son fonctionnement, une force sera appliquée a cause du composant spring récupéré du hinge Joint, pour que la porte tourne autour de ce joint, jusqu'a atteindre une position voulue
                    joint.spring = jSpring;
                    joint.useSpring = true;
                    Sounds.PlayDoorSond(hit.transform.GetComponent<AudioSource>(), openDoor);
                }
                else if (jSpring.targetPosition == -90) // Si la porte est ouverte, on la ferme
                {
                    hit.rigidbody.isKinematic = false;
                    StartCoroutine(Attend(hit,false));
                    jSpring.spring = 150;
                    jSpring.damper = 30;
                    jSpring.targetPosition = 3; // Grace au HingeJoint et son fonctionnement, une force sera appliquée a cause du composant spring récupéré du hinge Joint, pour que la porte tourne autour de ce joint, jusqu'a atteindre une position voulue
                    joint.spring = jSpring;
                    joint.useSpring = true;
                }
            }
        }
        else if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("porteFerme") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            interactionImage.enabled = true; // Ici On joue le son d'une porte fermée
            if (Input.GetKeyDown(KeyCode.E)) Sounds.PlayDoorSond(hit.transform.GetComponent<AudioSource>(), porteFerme);
        }
        else interactionImage.enabled = false; // On eleve l'image d'interacation GUI E si non besoin d'elle

        if (Physics.Raycast(transform.position, direction, out hit, 50f) && hit.transform.CompareTag("Ally")) ChooseCrosshair(defaultCross: false, enemyCross: false, allyCross: true);
        else if (Physics.Raycast(transform.position, direction, out hit, 50f) && hit.transform.CompareTag("Enemy")) ChooseCrosshair( defaultCross:false, enemyCross:true, allyCross:false);
        else if (Crosshair.enabled == false) ChooseCrosshair();

    }

    private void ChooseCrosshair(bool defaultCross = true, bool enemyCross = false, bool allyCross = false )
    {
        Crosshair.enabled = defaultCross;
        CrosshairAlly.enabled = allyCross;
        CrosshairEnemy.enabled = enemyCross;
    }

    IEnumerator Attend(RaycastHit h, bool souvre = true) // Permet de faire en sorte que l'on puisse plus pousser la porte apres un certain temps
    {
        if (souvre == true)
        {
            for (float vel = 0.2f; h.rigidbody.isKinematic == false;) // Tant que la porte n'est pas correctement fermée, on continue
            {
                yield return new WaitForSeconds(0.1f); // On attend dans cette interface IEnumerator en parallele de tout ce qui se passe dans le jeux
                if (h.rigidbody.velocity.magnitude < vel) h.rigidbody.isKinematic = true; // Permet de fair que l'on puisse pas bouger une porte lorsqu'elle est fermée
            }
        }
        else
        {
            for (float vel = 0.2f; h.rigidbody.isKinematic == false;) // Tant que la porte n'est pas correctement fermée, on continue
            {
                yield return new WaitForSeconds(0.1f); // On attend dans cette interface IEnumerator en parallele de tout ce qui se passe dans le jeux
                if (h.rigidbody.velocity.magnitude < vel)
                {
                    h.transform.GetComponent<AudioSource>().time = 0.5f;
                    Sounds.PlayDoorSond(h.transform.GetComponent<AudioSource>(), closeDoor);
                    h.rigidbody.isKinematic = true; // Permet de fair que l'on puisse pas bouger une porte lorsqu'elle est fermée
                    h.transform.GetComponent<AudioSource>().time = 0f;
                }
                else h.transform.GetComponent<AudioSource>().Stop();
            }
        }

    }
}