using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFootSoundEffect : MonoBehaviour
{
    public AudioClip sonDePasSurRue; //Initialisation des sons
    public AudioClip sonDePasSurBois;
    public AudioClip sonDePasSurterre;
    public AudioSource personnage;
    [Range(0f, 1f)] // Permet de regler le volume via un bouton de reglage dans Unity
    public float volumeDesSonsDePas = 0.2F; 
    [Range(1, 3)] // Permet de regler l'intensité via un bouton de reglage dans Unity
    public int intensité = 1;

    private void OnCollisionEnter(Collision collision) // Permet d'evaluer le son a jouer en fonction du type de sol rencontré /!\ On a besoin d'un rigibody et d'une box Colider /!\
    {
        if (collision.transform.tag == "Wood")
            personnage.clip = sonDePasSurBois; // Ici le son va alors devenir celui d'un bruit de pas sur le bois, pour les autres ca va jouer les son que l'on aura alors importé aussi lorsque la surface change
        else if (collision.transform.tag == "Street")
            personnage.clip = sonDePasSurRue;
        else if (collision.transform.tag == "Grass")
            personnage.clip = sonDePasSurterre;
    }

    // Use this for initialization
    void Start () {
        personnage.volume = volumeDesSonsDePas;  // Regle le volume des pas
        personnage.spatialBlend = 1F;  // Permet d'avoir les effet de son en 3D (on entend les bruits de pas de gauche vers la gauche, de droite vers la droite, etc)
        personnage.pitch = intensité; // Regle l'intensité du son
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.D))
        {
            if (!personnage.isPlaying) //Permet de jouer les bruits de pas si les touche "S","Q","Z","D"
                personnage.Play();
        }
	}
}
