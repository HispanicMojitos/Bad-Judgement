using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region membres
    [SerializeField] private AudioMixerSnapshot Repéré;
    [SerializeField] private AudioMixerSnapshot PeuDeVie;
    [SerializeField] private AudioMixerSnapshot enJeux;
    [SerializeField] private AudioClip[] bulletHits;
    [SerializeField] private AudioClip[] hurtSound;
    [SerializeField] private AudioClip HealBreath;
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioSource moutPlayer;
    [HideInInspector]public bool estRepere = false;
    [HideInInspector]public bool aPeuDeVie = false;
    /// <summary> Valeur récuperant la vie du joueur lors de la frame actuelle  </summary>
    private float vie;
    /// <summary> delai avant que le joueur récupere de la vie lorsqu'il est au repos </summary>
    private float delaiAvantRepos = 10f;
    /// <summary> delai entre chaque récupération de point de vie </summary>
    private float delaiEntreRecup = 0f;
    #endregion membres

    #region Start && Update

    void Start () {  vie = this.gameObject.GetComponent<Target>().vie;  }

	void Update ()
    {
        if(estRepere == true && aPeuDeVie == false) Sounds.transitionSound(Repéré, 0.01f);   // Les if ici permette de s'occuper du maintient des transitions des musiques/son en fonction de l'etat du joueur et du jeu
        else if(estRepere == false && aPeuDeVie == false) Sounds.transitionSound(enJeux, 1f);
        else if (aPeuDeVie == true) Sounds.transitionSound(PeuDeVie, 3f);

        if (vie != this.gameObject.GetComponent<Target>().vie) // Permet de jouer des bruits d'impact quand le joueur prenhd des degats
        {
            vie = this.gameObject.GetComponent<Target>().vie;
            if (moutPlayer.clip == HealBreath) moutPlayer.clip = null;
            Sounds.bulletSound(player, bulletHits);
            Sounds.hurtHuman(moutPlayer, hurtSound,vie);

            delaiAvantRepos = 10f;
        }

        if ( vie < this.gameObject.GetComponent<Target>().vieMax * 0.5f && vie != 0) // Si le joueur a moin de 50% de ses PV ET Qu'il ne recoit pas de degat, alors on va regenerer sa vie
        {
            delaiAvantRepos = delaiAvantRepos - Time.deltaTime;
            if(delaiAvantRepos < delaiEntreRecup) Repos();
            if (this.gameObject.GetComponent<Target>().vie <= (this.gameObject.GetComponent<Target>().vieMax * 0.2f))
            {
                aPeuDeVie = true;
                Sounds.BeatsOfHeart(player);
            }
        }
	}

    #endregion Start && Update


    #region methodes
    private void Repos()// Permet au joueur de se reposer et de récuperer de la vie
    {
        delaiEntreRecup -= 0.2f;
        this.gameObject.GetComponent<Target>().GainHealth(this.gameObject.GetComponent<Target>().vieMax * 0.01f);
        vie = this.gameObject.GetComponent<Target>().vie;

        if(!moutPlayer.isPlaying)
        {
            moutPlayer.clip = HealBreath;
            moutPlayer.Play();
        }

        if (vie == this.gameObject.GetComponent<Target>().vieMax * 0.5f)
        {
            moutPlayer.Stop();
            player.Stop();
            delaiEntreRecup = 0;
        }
        else if (vie >= this.gameObject.GetComponent<Target>().vieMax * 0.21f)
        {
            aPeuDeVie = false;
            player.Stop();
        }
    }
    #endregion methodes
}
