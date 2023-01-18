using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip dashSound;
    public AudioClip selectSound;
    public AudioClip decideSound;
    public AudioClip hitSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayDashSound()
    {
        audioSource.PlayOneShot(dashSound);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }

    public void PlaydecideSound()
    {
        audioSource.PlayOneShot(decideSound);
    }
}
