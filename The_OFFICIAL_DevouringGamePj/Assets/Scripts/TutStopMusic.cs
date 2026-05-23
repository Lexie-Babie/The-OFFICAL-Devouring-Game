using UnityEngine;

public class TutStopMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    public void StopBackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Stops the music completely
        }
    }
}
