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
    [SerializeField] private AudioClip heartBeats;
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


    void Start () {  vie = this.gameObject.GetComponent<Target>().vie;  }

	void Update ()
    {
        if(estRepere == true && aPeuDeVie == false) Sounds.transitionSound(Repéré, 0.01f);
        else if(estRepere == false && aPeuDeVie == false) Sounds.transitionSound(enJeux, 0.5f);
        else if (aPeuDeVie == true) Sounds.transitionSound(PeuDeVie, 0.3f);

        if (vie != this.gameObject.GetComponent<Target>().vie)
        {
            vie = this.gameObject.GetComponent<Target>().vie;
            if (moutPlayer.clip == HealBreath) moutPlayer.Stop();
            Sounds.bulletSound(player, bulletHits);
            Sounds.hurtHuman(moutPlayer, hurtSound,vie);

            delaiAvantRepos = 10f;
        }

        if ( vie < this.gameObject.GetComponent<Target>().vieMax * 0.5f)
        {
            delaiAvantRepos = delaiAvantRepos - Time.deltaTime;
            if(delaiAvantRepos < delaiEntreRecup) Repos();
            if (this.gameObject.GetComponent<Target>().vie <= (this.gameObject.GetComponent<Target>().vieMax * 0.2f))
            {
                aPeuDeVie = true;
                Sounds.BeatsOfHeart(player, heartBeats);
            }
        }

	}
    
    private void Repos()
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
}
