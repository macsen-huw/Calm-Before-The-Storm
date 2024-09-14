using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoverSound : MonoBehaviour
{
    public AudioSource engineAudioSource;
    public PlayerInput playerInput;
    public float movementThreshold = 0.1f;

    private Vector2 movementInput;

    private void Awake()
    {
        engineAudioSource = GetComponent<AudioSource>();
        engineAudioSource.loop = true;

        //Make sure it's not initially playing
        engineAudioSource.Stop();
    }

    public void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }

    // Update is called once per frame
    private void Update()
    {
        //If the rover moves
        if(movementInput.magnitude > movementThreshold)
        {
            if (!engineAudioSource.isPlaying)
                engineAudioSource.Play();
        }

        //If the rover doesn't move, make sure it turns off
        else
        {
            if(engineAudioSource.isPlaying)
                engineAudioSource.Pause();
        }
    }
}
