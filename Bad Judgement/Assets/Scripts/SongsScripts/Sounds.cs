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

    public static void AK47shoot(AudioSource AK47, AudioClip AK47shootSound, float volume = 0.3f)
    { 
                AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
                AK47.clip = AK47shootSound; // defini le son qu'emet l'AK47
                AK47.volume = volume; // defini le volume de l'AK47
                AK47.spatialBlend = 0.8f;
                AK47.Play();
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
        if (!personnage.isPlaying) personnage.Play();
    }

    #endregion FootSteeps

    #region jump
    public static void JumpSound(AudioSource piedPersonnage)
    {
        if (!piedPersonnage.isPlaying) piedPersonnage.Play();
    }

    public static void DeclareSonDemarche(AudioSource personnage, AudioClip sonDePasSur,  AudioSource piedjumpPersonnage, AudioClip jumpOn, char tag, char lettre)
    {
        personnage.clip = sonDePasSur; // Ici le son va alors devenir celui d'un bruit de pas sur le bois, pour les autres ca va jouer les son que l'on aura alors importé aussi lorsque la surface change
        piedjumpPersonnage.clip = jumpOn; // Pareil pour les jumps 
        if (tag != lettre) tag = lettre;
    }
    #endregion jump


    #endregion Movement
}
