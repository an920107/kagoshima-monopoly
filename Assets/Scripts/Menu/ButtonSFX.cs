using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    public void PlayHoverSound() {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound() {
        audioSource.PlayOneShot(clickSound);
    }
}
