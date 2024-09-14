using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] tyreRigidbodies;
    [SerializeField] private float speed = 1.0f;

    private float moveInput;
    private float moveThreshold = 0.1f;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMove (InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Rigidbody2D tyre in tyreRigidbodies) {
            tyre.AddTorque(-moveInput * speed);
        }


        //If rover is moving
        if (Math.Abs(moveInput) > moveThreshold)
            audioManager.RoverMoving();
        //Otherwise is isn't moving
        else 
            audioManager.RoverNotMoving();


    }
}
