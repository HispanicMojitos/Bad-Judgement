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

    #region AK47 Cz805

    /// <summary>Joue le son d'une unique balle d'AK47(Coupe le son precedent)</summary>
    public static void AK47shoot(AudioSource AK47, float volume = 1f)
    { 
        AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
        if(AK47.clip != Resources.Load("Sounds/AK47sounds/AK47shoot") as AudioClip) AK47.clip = Resources.Load("Sounds/AK47sounds/AK47shoot") as AudioClip;
        if(AK47.volume != volume) AK47.volume = volume; // defini le volume de l'AK47
        AK47.Play();
    }

    /// <summary>Joue le son d'une unique balle de CZ Pour le joueur(Coupe le son precedent)</summary>
    public static void Cz805shootPlayer(AudioSource AK47, float volume = 1f)
    {
        AK47.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
        if (AK47.clip != Resources.Load("Sounds/cZ805/Shoot (CZ 805)") as AudioClip) AK47.clip = Resources.Load("Sounds/cZ805/Shoot (CZ 805)") as AudioClip;
        if (AK47.volume != volume) AK47.volume = volume; // defini le volume de l'AK47
        AK47.Play();
    }

    /// <summary>Joue le son d'une unique balle d'un CZ 805(Coupe le son precedent)</summary>
    public static void Cz805shoot(AudioSource Cz805, float volume = 1f)
    {
        Cz805.Stop(); // permet de jouer le son de la prochaine balle qui arrivera
        if(Cz805.clip != Resources.Load("Sounds/cZ805/CZ805") as AudioClip) Cz805.clip = Resources.Load("Sounds/cZ805/CZ805") as AudioClip;
        if(Cz805.volume != volume) Cz805.volume = volume; // defini le volume de l'AK47
        Cz805.Play();
    }

    /// <summary> Permet de jouer le son de rechargement (Coupe le son precedent de l'AK47)</summary>
    public static void AK47reload(AudioSource AK47, float volume = 1f)
    {
        if (AK47.clip != Resources.Load("Sounds/cZ805/Reload2(CZ 805)"))
        {
            AK47.Stop();
             AK47.clip = Resources.Load("Sounds/cZ805/Reload2(CZ 805)") as AudioClip;
            if (AK47.volume != volume) AK47.volume = volume;
            AK47.Play();
        }
    }

    #endregion AK47 Cz805

    #region Movement

    /// <summary> permet de jouer le son de jump </summary>
    public static void Jump(AudioSource piedPersonnage, float volume = 1)
    {
        if (!piedPersonnage.isPlaying)
        {
           if(piedPersonnage.clip != Resources.Load("Sounds/SoldierSoundsPack/jump") as AudioClip) piedPersonnage.clip = Resources.Load("Sounds/SoldierSoundsPack/jump") as AudioClip;
           if(piedPersonnage.volume != volume)  piedPersonnage.volume = volume;
            piedPersonnage.Play();
        }
    }

    static private int numeroPied = 0; // Variabes utilisée pour la méthode statique Marche
    /// <summary> Permet de jouer les sons de pas lorsqu'on marche OU cours (tout depends du clipAudio donné) </summary>
    public static void Marche(AudioSource[] pieds, bool canJump,bool cours, float volume = 1f)
    {
        if (canJump == true)
        {
            if (!pieds[numeroPied].isPlaying && cours == false)
            {
                if(pieds[0].clip != Resources.Load("Sounds/SoldierSoundsPack/FootStep2") as AudioClip) pieds[0].clip = Resources.Load("Sounds/SoldierSoundsPack/FootStep2") as AudioClip;
                if(pieds[0].volume != volume)  pieds[0].volume = volume;
                if(pieds[1].clip = Resources.Load("Sounds/SoldierSoundsPack/FootStep2") as AudioClip) pieds[1].clip = Resources.Load("Sounds/SoldierSoundsPack/FootStep2") as AudioClip;
                if(pieds[1].volume != volume) pieds[1].volume = volume;
                if (numeroPied == 0) numeroPied = 1;
                else if (numeroPied == 1) numeroPied = 0;
                pieds[numeroPied].Play();
            }
            else if(!pieds[numeroPied].isPlaying && cours == true)
            {
                if(pieds[0].clip != Resources.Load("Sounds/SoldierSoundsPack/RUN") as AudioClip) pieds[0].clip = Resources.Load("Sounds/SoldierSoundsPack/RUN") as AudioClip;
                if (pieds[0].volume != volume) pieds[0].volume = volume;
                if(pieds[1].clip != Resources.Load("Sounds/SoldierSoundsPack/RUN") as AudioClip) pieds[1].clip = Resources.Load("Sounds/SoldierSoundsPack/RUN") as AudioClip;
                if (pieds[1].volume != volume) pieds[1].volume = volume;
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
    public static void BeatsOfHeart(AudioSource coeur, float volume = 1f) 
    {
        if (!coeur.isPlaying)
        {
            if (coeur.clip = Resources.Load("Sounds/Heartbeatsound") as AudioClip) coeur.clip = Resources.Load("Sounds/Heartbeatsound") as AudioClip;
            if(coeur.spatialBlend != 0f) coeur.spatialBlend = 0f;
            if(coeur.volume != volume) coeur.volume = volume;
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
    public static void  bulletSound(AudioSource corps, AudioClip[] bullet, float volume = 1)
    {
        int random = Random.Range(0,1); // Plus l'ecart est grand, moins le son de balle sera fréquent
        if( random == 0)
        {
            if (!corps.isPlaying)
            {
                int randomBullet = Random.Range(0, bullet.Length);
                corps.clip = bullet[randomBullet];
                if(corps.spatialBlend != 1f) corps.spatialBlend = 1f;
                if(corps.volume != volume) corps.volume = volume;
                corps.Play();
            }
        }
    }
    /// <summary> Choisi au hasard quand jouer le son de douleur du personnage, et choisi au hasard le son de personnage choisi grace au tableau "hurt" </summary>
    public static void hurtHuman(AudioSource mouth, AudioClip[] hurt, float vie, float volume = 1)
    {
        int random = Random.Range(0, 5); // Plus l'ecart est grand, moin le son de cris de douleur sera entendu
        if (random == 1)
        {
            if (!mouth.isPlaying)
            {
                int randomhirt = Random.Range(0, hurt.Length);
                mouth.clip = hurt[randomhirt];
                if(mouth.spatialBlend != 1f) mouth.spatialBlend = 1f;
                if(mouth.volume != volume) mouth.volume = volume;
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

    /// <summary>permet de mettre changer le son via une transition lorsqu'on met le jeux en pause(On a besoin d'un ellement AudioMixerSnashot en parametre et d'un temps de transition, pour passer au propriété audio de cet AudioMixer) </summary>
    static public void transitionSound(AudioMixerSnapshot pause, float tempsTransition) { pause.TransitionTo(tempsTransition); }

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
