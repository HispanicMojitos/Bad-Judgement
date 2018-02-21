using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// /!\ A REVENIR DESSUS POUR OPTIMISER MARTIN, AMELIORER LES SONS DE PAS SI POSSIBLE , AJOUTER LES SONS DATTERISAGE DE SAUTS ET TROUVER UN ASSETS DE SAUT POUR LA VOIX SI POSSIBLE
public class StepFootSoundEffect : MonoBehaviour
{
    public AudioClip sonDePasSurRue; //Initialisation des sons de pas 
    public AudioClip sonDePasSurBois;
    public AudioClip sonDePasSurterre;
    public AudioClip jumpOnGrass; // Initialisation de sons de Jump
    public AudioClip jumpOnStreet;
    public AudioClip jumpOnWood;
    public AudioSource personnage;
    public AudioSource piedjumpPersonnage;

    [Range(0f, 1f)] // Permet de regler le volume via un bouton de reglage dans Unity
    public float volumeDesSonsDePas = 0.2F; 
    [Range(1, 3)] // Permet de regler l'intensité via un bouton de reglage dans Unity
    public int intensité = 1;

    private void OnCollisionEnter(Collision collision) // Permet d'evaluer le son a jouer en fonction du type de sol rencontré /!\ On a besoin d'un rigibody et d'une box Colider /!\
    {
        if (collision.transform.tag == "Wood")
        {
            personnage.clip = sonDePasSurBois; // Ici le son va alors devenir celui d'un bruit de pas sur le bois, pour les autres ca va jouer les son que l'on aura alors importé aussi lorsque la surface change
            piedjumpPersonnage.clip = jumpOnWood; // Pareil pour les jumps 
        }
        else if (collision.transform.tag == "Street")
        {
            personnage.clip = sonDePasSurRue;
            piedjumpPersonnage.clip = jumpOnStreet;
        }
        else if (collision.transform.tag == "Grass")
        {
            personnage.clip = sonDePasSurterre;
            piedjumpPersonnage.clip = jumpOnGrass;
        }
    }

    // Use this for initialization
    void Start ()
    {
        personnage.volume = volumeDesSonsDePas;  // Regle le volume des pas
        piedjumpPersonnage.volume = 0.5f;
        personnage.spatialBlend = 1f;  // Permet d'avoir les effet de son en 3D (on entend les bruits de pas de gauche vers la gauche, de droite vers la droite, etc)
        piedjumpPersonnage.spatialBlend = 1f;
        personnage.pitch = intensité; // Regle l'intensité du son
        piedjumpPersonnage.pitch = intensité;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.D)) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space)) // permet d'evite 
        {
            if (!personnage.isPlaying && !piedjumpPersonnage.isPlaying && !Input.GetKey(KeyCode.Space)) //Permet de jouer les bruits de pas si les touche "S","Q","Z","D"
            {
                    personnage.Play(); // Joue le son attaché a la source qu'est le personnage
            }
            else if (Input.GetKeyDown(KeyCode.Space)) // Si la touche espace est appuyé pour sauter, les sons de saut von se faire
            {
                piedjumpPersonnage.Play(); // Joue le son attaché a la source que sont les pieds du personnages
            }
        }
        else if(Input.GetKeyDown(KeyCode.Space)) // Si la touche espace est appuyé pour sauter, les sons de saut von se faire
        {
            piedjumpPersonnage.Play(); // Joue le son attaché a la source que sont les pieds du personnages
        }
        else
            personnage.Stop(); // ne joue pas le son de pas si on ne bouge pas quoi, normal x)
	}
}
