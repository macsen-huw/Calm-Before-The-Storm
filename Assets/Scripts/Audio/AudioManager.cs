using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource waves;
    [SerializeField] AudioSource roverEngine;
    [SerializeField] AudioSource splash;

    [Header("Audio Clips")]
    public AudioClip engineClip;
    public AudioClip waveClip;
    public AudioClip splashClip;

    // Start is called before the first frame update
    void Start()
    {
        waves.clip = waveClip;
        roverEngine.clip = engineClip;
        splash.clip = splashClip;

        waves.Play();
    }

    
    //Play the rover sound Engine when necesary
    public void RoverMoving()
    {

        if (!roverEngine.isPlaying)
            roverEngine.Play();

    }

    public void RoverNotMoving()
    {
        if (roverEngine.isPlaying)
            roverEngine.Stop();
    }

    public void SplashNoise()
    {
        splash.Play();
    }
}
