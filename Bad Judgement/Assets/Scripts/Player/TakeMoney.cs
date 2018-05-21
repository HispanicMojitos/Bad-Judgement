using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeMoney : MonoBehaviour {

    // Use this for initialization

    public Text moneyText;
    public int moneyCount;

    void Start()
    {

        moneyText.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;

        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("Money") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            moneyText.enabled = true;
            moneyText.text = "Press F to take the money";

            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(hit.transform.gameObject);
                moneyText.enabled = false;
                moneyCount++;
                int NumeroMoneyCount = Random.Range(1,14);
                AudioSource Joueur = GetComponent<AudioSource>();
                Joueur.Stop(); // Permet darreter le precedent son 
                if (Joueur.volume != 1) Joueur.volume = 1; // On initialize a cou sur le son de l'audiosource a 1
                Joueur.clip = Resources.Load(string.Format("Sounds/Cash/TakeMoney{0}",NumeroMoneyCount)) as AudioClip; // Ici on charge les sons UNIQUEMENT POSSIBLE DEPUIS LE DOSSIER RESSOURCES
                Joueur.Play(); // Ici on joue le son
            }
        }
        else if (moneyText.enabled == true) moneyText.enabled = false;
    }
}
