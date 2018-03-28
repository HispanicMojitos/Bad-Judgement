using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioClip heartBeats;
    private float vie;
    private bool attendRepos = false;
    private float delaiAvantRepos=10;
    private float delaiEntreRecup = 0;
	// Use this for initialization
	void Start ()
    {
        vie = this.gameObject.GetComponent<Target>().vie;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (vie != this.gameObject.GetComponent<Target>().vie)
        {
            vie = this.gameObject.GetComponent<Target>().vie;
            delaiAvantRepos = 10f;
            attendRepos = true;
        }

        if (attendRepos == true)
        {
            delaiAvantRepos = delaiAvantRepos - Time.deltaTime;
            if(delaiAvantRepos < delaiEntreRecup) Repos();
        }

        if (this.gameObject.GetComponent<Target>().vie <= (this.gameObject.GetComponent<Target>().vieMax * 0.2))
        {
            Sounds.BeatsOfHeart(player, heartBeats);
        }
        else if (player.isPlaying) player.Stop();
	}
    
    private void Repos()
    {
        delaiEntreRecup -= 0.2f;
        this.gameObject.GetComponent<Target>().GainHealth(1);
        vie = this.gameObject.GetComponent<Target>().vie;
        if (vie == 50)
        {
            attendRepos = false;
            delaiEntreRecup = 0;
        }
    }
}
