using UnityEngine;
using UnityEngine.UI;

public class HealthBarAudio : MonoBehaviour
{
    //public AudioSource audioSource;
    public AudioSource audioSource2;
   // public AudioClip healthChangeClip;
    public AudioClip damageChangeClip;
    private float lastValue;

    void Start()
    {
        // Store the initial value to prevent a sound on frame one
        lastValue = GetComponent<Slider>().value;
    }

    // This method will be called by the Slider's OnValueChanged event
    /*public void PlayHealthSound(float newValue)
    {
        // Only play if the value actually changed and it's not currently playing
        if (newValue != lastValue && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(healthChangeClip);
            lastValue = newValue;
        }
    }*/

    public void PlayDamageSound(float newValue)
    {
        // Only play if the value actually changed and it's not currently playing
        if (newValue != lastValue && !audioSource2.isPlaying)
        {
            audioSource2.PlayOneShot(damageChangeClip);
            lastValue = newValue;
        }
    }
}
