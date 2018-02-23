using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// /!\  ATTENTION CE SCRIPT VA BEAUCOUP CHANGER, NOTTEMMENT LORSQU'ON VA METTRE PLUSIEURS ARME,JE VERRAI AVEC VOUS CAR IL FAUT ADAPTER LE SCRIPT EN FONCTION DES ARMES
public static class Sounds
{
    
    private static float timeSound = 0.001f; // temps du son de tir joué au fur et a mesur que l'on presse le bouton
    private static float cadence = 0f; // permet de cadencer les tirs

    #region AK47

    #region AK47shoot
    public static void AK47shoot (AudioSource AK47,AudioClip AK47shootSound,float nextFireTime, float volume = 0.3f)
    {
        if (nextFireTime <= Time.time) // permet un son de tir en bon timing, en fonction de la valeur nextFireTime entrée
        {
            if ((Input.GetButton("Fire1")) && (timeSound >= cadence) && (timeSound <= 0.560f) && nextFireTime <= Time.time) // permet de tirer en boucle et d'éviter que les sons de tirs dure plus de 860ms d'affilé
            {
                AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
                AK47.clip = AK47shootSound; // defini le son qu'emet l'AK47
                AK47.volume = volume; // defini le volume de l'AK47
                AK47.spatialBlend = 0.8f;
                AK47.Play(); // joue le son de l'AK47
                timeSound = timeSound + Time.deltaTime;
                cadence = cadence + 0.100f; // Permet de initialiser la cadence a 1 tir tout les 0,1s, soit la candence réelle d'un AK47 : https://fr.wikipedia.org/wiki/AK-47
            }
            else if (Input.GetButton("Fire1") && timeSound < 0.860f) // Permet l'empechement d'une boucle de son infinie
            {
                timeSound = timeSound + Time.deltaTime;
            }
            else if (!Input.GetButton("Fire1")) // permet de toujours initialiser ces variable a zero pour pouvoir retirer apres
            {
                timeSound = 0.001f;
                cadence = 0f;
            }
        }
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
}
