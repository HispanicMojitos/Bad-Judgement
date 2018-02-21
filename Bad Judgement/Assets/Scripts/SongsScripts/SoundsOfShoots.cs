using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// /!\ A OPTIMISER MARTIN /!\ ATTENTION CE SCRIPT VA BEAUCOUP CHANGER, NOTTEMMENT LORSQU'ON VA METTRE PLUSIEURS ARME, JY AI BEAUCOUP REFLECHI ET JE VERRAI AVEC VOUS CAR IL FAUT ADAPTER LE SCRIPTS EN FONCTION DES ARMES
public class SoundsOfShoots : MonoBehaviour
{
    public AudioClip shootSound; // Son de tirs du AK47
    public AudioSource machineGun; // Source des sons = 1K47
    public AudioClip endsong; // Son de fin apres les tirs
    public AudioClip reload; // son de rechargement

    
    private float timeSound; // temps du son de tir joué au fur et a mesur que l'on presse le bouton
    private bool sonJoué = false; // pour l'algorithme sache si le son est joué ou pas
    private int indice2 = 1; // Indice n°2 utilisé pour un fonctionnement de l'algorithme     !! EXPLIQUé PLUS BAS !! 
    private int indice1 = 1; // Indice n°1 utilisé pour un fonctionnement de l'algorithme

    [Range(0f,1f)] // Permet de regler le son
    public float volume = 0.2f; 
    [Range(1, 5)] // Permet de regler la 'vitesse' du son ANDREWS TEST sur 3 CA FAIS UN SON DE MINIGUN HAHA
    public int intensité = 1;

	// Use this for initialization
	void Awake  () {
        machineGun.clip = shootSound; // implémente le son de tir a la source (AK47)
        machineGun.volume = volume; // implémente l'intensité
        machineGun.pitch = intensité; // implément l'intensité
        machineGun.spatialBlend = 0.8f; // permet d'avoir un son 'réaliste'
        timeSound = shootSound.length; // regle le temps du son de tirs 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetButtonDown("Fire1")) // Si la touche pour tirer est cliqué, initialise des variables nécéssaire pour l'algorithme de fonctionnement 
        {
            indice1 = 0;
            timeSound = shootSound.length; // intialise le temps de son
            machineGun.clip = shootSound; // intitialise le son de tir des que l'on clique, logique
            sonJoué = true;
        }


		if(Input.GetButton("Fire1") && indice1 == 0)  // Si la touche pour tirer est préssé et que le son doit continuer de jouer (indice1) , on joue le son !! => ALGORITHME
        {
            indice2 = 1; // Empeche une boucle infinie du son de fin, croyez moi IL FAUT laisser ca la x) => ALGORITHME
            timeSound = timeSound - Time.deltaTime; // permet de decrementer le  temps du son actuel du tir en fonction du temps qui passe
            if (!machineGun.isPlaying) // si le son de tir n'est pas détécté, on va le jouer quoi !!!
            {
                if (timeSound > 0) // si le temp de son qui reste est supérieur à 0, on le jour
                    machineGun.Play();
                else if(timeSound <= 0) // si le temp de son qui reste est inferieur a Zero, on increment l'indice1 pour qque le son ne se fasse pas en boucle  => ALGORITHME
                    indice1 = 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.R) && !machineGun.isPlaying) // Si la touche R est préssée pour tirer, le son de recharge se fait !!
        {
            machineGun.clip = reload; // On implemente le son de rechargement
            machineGun.Play(); // On joue le son de rechargement
        }
        else
        {
            if (sonJoué == true) // permet d'acceder a la condition suivante et de decrementer le  temps du son actuel du tir
            {
                indice2 = 0; // Permet d'acceder a la condition suivante 
                timeSound = timeSound - Time.deltaTime; // Decrementation du temps de son passé
            }
            if (indice2 == 0 && timeSound > endsong.length) // Permet de jouer le son de fin 
            {
                machineGun.Stop(); // On arrete le son de tir
                machineGun.clip = endsong; // On implement le son de fin de tir (pour faire plus réaliste et pas faire un son coupé quoi
                machineGun.Play(); // On joue le son de fin de tir
                sonJoué = false; // Le son ne sera plus joué apres
                indice2 = 1;
            }
        }
	}
}
