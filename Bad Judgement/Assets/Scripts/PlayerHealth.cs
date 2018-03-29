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
    private float vie;
    private bool finiRepos = false;
    private bool attendRepos = false;
    private float delaiAvantRepos=10;
    private float delaiEntreRecup = 0;
    #endregion membres
    void Start ()
    {
        vie = this.gameObject.GetComponent<Target>().vie;
    }
	
	void Update ()
    {
        if (vie != this.gameObject.GetComponent<Target>().vie)
        {
            vie = this.gameObject.GetComponent<Target>().vie;
            delaiAvantRepos = 10f;
            Sounds.bulletSound(player,bulletHits);
            if (moutPlayer.clip == HealBreath) moutPlayer.Stop();
            Sounds.hurtHuman(moutPlayer, hurtSound);
            attendRepos = true;
        }

        if (attendRepos == true)
        {
            delaiAvantRepos = delaiAvantRepos - Time.deltaTime;
            if(delaiAvantRepos < delaiEntreRecup) Repos();
        }

        if (this.gameObject.GetComponent<Target>().vie <= (this.gameObject.GetComponent<Target>().vieMax * 0.2)) Sounds.BeatsOfHeart(player, heartBeats);
        else if (player.isPlaying && finiRepos == true)
        {
            player.Stop();
            finiRepos = false;
        }
	}
    
    private void Repos()
    {
        delaiEntreRecup -= 0.2f;
        this.gameObject.GetComponent<Target>().GainHealth(1);
        vie = this.gameObject.GetComponent<Target>().vie;

        if(!moutPlayer.isPlaying)
        {
            moutPlayer.clip = HealBreath;
            moutPlayer.Play();
        }

        if (vie == 50)
        {
            attendRepos = false;
            delaiEntreRecup = 0;
        }
        else if(vie == 20) finiRepos = true;
    }
}
