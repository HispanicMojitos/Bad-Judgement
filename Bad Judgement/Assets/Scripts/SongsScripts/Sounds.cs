using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// /!\  ATTENTION CE SCRIPT VA BEAUCOUP CHANGER, NOTTEMMENT LORSQU'ON VA METTRE PLUSIEURS ARME,JE VERRAI AVEC VOUS CAR IL FAUT ADAPTER LE SCRIPT EN FONCTION DES ARMES
public static class Sounds
{
    #region propriétés && membres
    private static float VolumeSon = 1f;
    private static float VolumeMusique = 1f;
    #endregion propriété && membres

    #region AK47

    /// <summary>Joue le son d'une unique balle (Coupe le son precedent)</summary>
    public static void AK47shoot(AudioSource AK47, AudioClip AK47shootSound, float volume = 0.3f)
    { 
         AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
         AK47.clip = AK47shootSound; // defini le son qu'emet l'AK47
         AK47.volume = VolumeSon; // defini le volume de l'AK47
         AK47.Play();
    }

    /// <summary> Permet de jouer le son de rechargement (Coupe le son precedent de l'AK47)</summary>
    public static void AK47reload(AudioSource AK47, AudioClip AK47reloadSound, float volume = 1f)
    {
        AK47.Stop();
        AK47.clip = AK47reloadSound;
        AK47.volume = VolumeSon;
        AK47.Play();
    }

    #endregion AK47

    #region Movement

    /// <summary> permet de jouer le son de jump </summary>
    public static void Jump(AudioSource piedPersonnage, AudioClip jumpSound)
    {
        if (!piedPersonnage.isPlaying)
        {
            piedPersonnage.clip = jumpSound;
            piedPersonnage.volume = VolumeSon;
            piedPersonnage.Play();
        }
    }

    static private int numeroPied = 0; // Variabes utilisée pour la méthode statique Marche
    /// <summary> Permet de jouer les sons de pas lorsqu'on marche OU cours (tout depends du clipAudio donné) </summary>
    public static void Marche(AudioSource[] pieds,AudioClip sonDePas, bool canJump)
    {
        if (canJump == true)
        {
            if (!pieds[numeroPied].isPlaying)
            {
                pieds[0].clip = sonDePas;
                pieds[0].volume = VolumeSon;
                pieds[1].clip = sonDePas;
                pieds[1].volume = VolumeSon;
                if (numeroPied == 0) numeroPied = 1;
                else if (numeroPied == 1) numeroPied = 0;
                pieds[numeroPied].Play();
            }
        }
    }
    #endregion Movement

    #region interaction sound
    /// <summary> Permet de jouer un son de porte et de couper les son actuel, si existant</summary>
    public static void PlayDoorSond(AudioSource door, AudioClip doorSound)
    {
        if(!door.isPlaying || door.clip != doorSound)
        {
            door.clip = doorSound;
            door.volume = VolumeSon;
            door.Play();
        }
    }
    #endregion interaction sound

    #region human sound

    /// <summary> Joue le son d'un battement de coeur en 2D </summary>
    public static void BeatsOfHeart(AudioSource coeur, AudioClip battements) 
    {
        if ( !coeur.isPlaying)
        {
            coeur.clip = battements;
            coeur.spatialBlend = 0f;
            coeur.volume = VolumeSon;
            coeur.Play(); 
        }
    }

    /// <summary> Joue le son d'un cri d'un mort qu'une seul et unique fois grace au pramatre " playSoundOnce"</summary>
    public static void Death(AudioSource mouthHead,AudioClip deathSound,bool playSoundOnce)
    {
        if (!mouthHead.isPlaying && playSoundOnce == false)
        {
            mouthHead.clip = deathSound;
            mouthHead.volume = VolumeSon;
            mouthHead.Play();
            playSoundOnce = true;
        }
    }

    /// <summary> Joue le son des balles arrivant aux oreilles au hasard dans le tableau "bullet"</summary>
    public static void  bulletSound(AudioSource corps, AudioClip[] bullet)
    {
        int random = Random.Range(0,1); // Plus l'ecart est grand, moins le son de balle sera fréquent
        if( random == 0)
        {
            if (!corps.isPlaying)
            {
                int randomBullet = Random.Range(0, bullet.Length);
                corps.clip = bullet[randomBullet];
                corps.spatialBlend = 1f;
                corps.volume = VolumeSon;
                corps.Play();
            }
        }
    }
    /// <summary> Choisi au hasard quand jouer le son de douleur du personnage, et choisi au hasard le son de personnage choisi grace au tableau "hurt" </summary>
    public static void hurtHuman(AudioSource mouth, AudioClip[] hurt, float vie)
    {
        int random = Random.Range(0, 10); // Plus l'ecart est grand, moin le son de cris de douleur sera entendu
        if (random == 2)
        {
            if (!mouth.isPlaying)
            {
                int randomhirt = Random.Range(0, hurt.Length);
                mouth.clip = hurt[randomhirt];
                mouth.spatialBlend = 1f;
                mouth.volume = VolumeSon;
                mouth.Play();
            }
        }
    }
    #endregion human sound
    /// <summary>Permet de jouer une musique voulue grace à une source auditive initialisée </summary>
    public static void JouerMusique(AudioSource OrigineDuSon, AudioClip Musique)
    {
        if(!OrigineDuSon.isPlaying)
        {
            OrigineDuSon.clip = Musique;
            OrigineDuSon.volume = VolumeMusique;
            OrigineDuSon.loop = true;
            OrigineDuSon.Play();
        }

    }

    #region Mixer

    /// <summary>permet de mettre changer le son via une transition (On a besoin d'un ellement AudioMixerSnashot en parametre, pour passer au propriété audio de cet AudioMixer) </summary>
    static public void transitionSound(AudioMixerSnapshot pause) { pause.TransitionTo(0.1f); }
    /// <summary> Permet de modifier le volume des musiques en prenant en parametre le mixerMaster qui s'occupe de la gèrence de tout les mixeur </summary>
    static public void MusicVolumSet(AudioMixer backgroundMusicMixer, float volume)
    {
        backgroundMusicMixer.SetFloat("Volumemusic", volume);
    }
    /// <summary> Permet de modifier le volume des effets sonore en prenant en parametre le mixerMaster qui s'occupe de la gèrence de tout les mixeur </summary>
    static public void SoundEffectVolumeSet(AudioMixer soundEffectMixer, float volume)
    {
        soundEffectMixer.SetFloat("VolumesoundEffect", volume);
    }

    #endregion Mixer
}
