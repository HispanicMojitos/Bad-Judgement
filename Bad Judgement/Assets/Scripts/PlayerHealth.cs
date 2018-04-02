using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region membres
    [SerializeField] private AudioClip[] bulletHits;
    [SerializeField] private AudioClip[] hurtSound;
    [SerializeField] private AudioClip heartBeats;
    [SerializeField] private AudioClip HealBreath;
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioSource moutPlayer;
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
            if (this.gameObject.GetComponent<Target>().vie <= (this.gameObject.GetComponent<Target>().vieMax * 0.2f)) Sounds.BeatsOfHeart(player, heartBeats);
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
        else if (vie >=this.gameObject.GetComponent<Target>().vieMax * 0.21f) player.Stop();
    }
}
