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
        print("Are we recording?");
        movementInput = input.Get<Vector2>();
    }

    // Update is called once per frame
    private void Update()
    {
        print("Whoo Ha!");
        //If the rover moves
        if(movementInput.magnitude > movementThreshold)
        {

            print("Moving!");
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
