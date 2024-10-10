using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public AudioSource audioSource;
    public AudioClip hoverSFX;
    public AudioClip pressSFX;

    private void Awake(){
        if (instance == null){
            instance =  this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void PlayHoverSFX(){
        if (hoverSFX != null){
            audioSource.PlayOneShot(hoverSFX);
        }
    }

    public void PlayPressSFX(){
        if (pressSFX != null){
            audioSource.PlayOneShot(pressSFX);
        }
    }
}
