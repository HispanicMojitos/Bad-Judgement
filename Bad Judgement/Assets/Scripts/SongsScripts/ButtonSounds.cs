using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class ButtonSounds : MonoBehaviour
{

    [SerializeField] private AudioSource UI;
	
    public void ClickSound ()
    {
        UI.PlayOneShot(Resources.Load("Sounds/Boutton/Click") as AudioClip);
    }

    public void HoverSound()
    {
        UI.PlayOneShot(Resources.Load("Sounds/Boutton/Hover") as AudioClip);
    }
}
