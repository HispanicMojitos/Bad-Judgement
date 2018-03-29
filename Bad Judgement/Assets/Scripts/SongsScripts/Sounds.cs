using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// /!\  ATTENTION CE SCRIPT VA BEAUCOUP CHANGER, NOTTEMMENT LORSQU'ON VA METTRE PLUSIEURS ARME,JE VERRAI AVEC VOUS CAR IL FAUT ADAPTER LE SCRIPT EN FONCTION DES ARMES
public static class Sounds
{
    #region AK47

    public static void AK47shoot(AudioSource AK47, AudioClip AK47shootSound, float volume = 0.3f)
    { 
         AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
         AK47.clip = AK47shootSound; // defini le son qu'emet l'AK47
         AK47.volume = volume; // defini le volume de l'AK47
         AK47.spatialBlend = 0.8f;
         AK47.Play();
    }
    
    public static void AK47reload(AudioSource AK47, AudioClip AK47reloadSound, float volume = 1f)
    {
        AK47.Stop();
        AK47.clip = AK47reloadSound;
        AK47.volume = volume;
        AK47.Play();
    }

    #endregion AK47
    
    #region Movement
    public static void FootSteepsSound(AudioSource personnage)
    {
        if (!personnage.isPlaying) personnage.Play();
    }
    
    
    public static void Jump(AudioSource piedPersonnage, AudioClip jumpSound)
    {
        if (!piedPersonnage.isPlaying)
        {
            piedPersonnage.clip = jumpSound;
            piedPersonnage.Play();
        }
    }

    static private int numeroPied = 0;
    /// <summary> Permet de jouer les sons de pas lorsqu'on marche OU cours (tout depends du clipAudio donné) </summary>
    public static void Marche(AudioSource[] pieds,AudioClip sonDePas, bool canJump)
    {
        if (canJump == true)
        {
            if (!pieds[numeroPied].isPlaying)
            {
                pieds[0].clip = sonDePas;
                pieds[1].clip = sonDePas;
                if (numeroPied == 0) numeroPied = 1;
                else if (numeroPied == 1) numeroPied = 0;
                pieds[numeroPied].Play();
            }
        }
    }
    #endregion Movement

    public static void Death(AudioSource mouthHead)
    {
        if (!mouthHead.isPlaying) mouthHead.Play();
    }

    public static void PlayDoorSond(AudioSource door, AudioClip doorSound)
    {
        if(!door.isPlaying || door.clip != doorSound)
        {
            door.clip = doorSound;
            door.Play();
        }
    }

    /// <summary> Joue le son d'un battement de coeur en 2D </summary>
    public static void BeatsOfHeart(AudioSource coeur, AudioClip battements) 
    {
        if ( !coeur.isPlaying)
        {
            coeur.clip = battements;
            coeur.spatialBlend = 0f;
            coeur.Play(); 
        }
    }

    public static void  bulletSound(AudioSource corps, AudioClip[] bullet)
    {
        int random = Random.Range(0,1);
        if( random == 0)
        {
            if (!corps.isPlaying)
            {
                int randomBullet = Random.Range(0, bullet.Length);
                corps.clip = bullet[randomBullet];
                corps.spatialBlend = 1f;
                corps.Play();
            }
        }
    }
}
