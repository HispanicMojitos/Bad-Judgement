using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// /!\  ATTENTION CE SCRIPT VA BEAUCOUP CHANGER, NOTTEMMENT LORSQU'ON VA METTRE PLUSIEURS ARME,JE VERRAI AVEC VOUS CAR IL FAUT ADAPTER LE SCRIPT EN FONCTION DES ARMES
public static class Sounds
{
    #region AK47

    #region membres
    private static float timeSound = 0.001f; // temps du son de tir joué au fur et a mesur que l'on presse le bouton
    private static float cadence = 0f; // permet de cadencer les tirs de l'AK K7
    #endregion membres

    #region AK47shoot

    public static float Cadence
    {
        get { return cadence; }
        set { Cadence = value; }
    }
    public static void AK47shoot(AudioSource AK47, AudioClip AK47shootSound, /*int nmbreDeMunitions = 1,*/ float volume = 0.3f) // int nmbreDeMunitions = 1 et float volume = 0.3f son des membre només, pour les appeler il faut faire Sounds.AK47shoot(AK47, AK47shoot, munitions, volume);
    {
        //if (nmbreDeMunitions != 0) // Si il n'y a plus de munitions, alors le bruit de tir ne se fait plus car on ne sait plus tirer
        //{
            //if ((Input.GetButton("Fire1")) && (timeSound >= cadence)) // permet de s'assurer que le boutton est résté appuyé, ainsi que de cadencer les tirs
            //{
                AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
                AK47.clip = AK47shootSound; // defini le son qu'emet l'AK47
                AK47.volume = volume; // defini le volume de l'AK47
                AK47.spatialBlend = 0.8f;
                AK47.Play(); // joue le son de l'AK47
                //timeSound = timeSound + Time.deltaTime;  // permet de savoir le temps de son joué
                //cadence = cadence + 0.100f; // Permet de initialiser la cadence a 1 tir tout les 0,1s, soit la candence réelle d'un AK47 : https://fr.wikipedia.org/wiki/AK-47
            //}
            /*else if (Input.GetButton("Fire1")) // Permet l'empechement d'une boucle de son infinie
            {
                timeSound = timeSound + Time.deltaTime;
            }
            else if (!Input.GetButton("Fire1")) // permet de toujours initialiser ces variable a zero pour pouvoir retirer apres
            {
                timeSound = 0.001f;
                cadence = 0f;
            }*/
        //}
    }
    #endregion AK47shoot

    #region AK47reload
    public static void AK47reload(AudioSource AK47, AudioClip AK47reloadSound, float volume = 1f)
    {
        AK47.Stop();
        AK47.clip = AK47reloadSound;
        AK47.volume = volume;
        AK47.Play();
    }

    #endregion AK47reload

    #endregion AK47
    
    #region Movement

    #region FootSteeps
    public static void FootSteepsSound(AudioSource personnage)
    {
        if (!personnage.isPlaying) personnage.PlayOneShot(personnage.clip);

    }
    #endregion FootSteeps

    #region jump
    public static void JumpSound(AudioSource piedPersonnage)
    {
        if (!piedPersonnage.isPlaying) piedPersonnage.Play();
    }
    #endregion jump


    #endregion Movement
}
