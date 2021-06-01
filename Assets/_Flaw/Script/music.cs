using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class music : MonoBehaviour
{
    
   // private bool pause;
    //public AudioClip song1;
    public AudioSource audioSource;


    public void PauseMusic()
    {
         if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }

        else

        {
            audioSource.Play();
        }
    }

   
}
