using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Rover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] tyreRigidbodies;
    [SerializeField] private float speed = 1.0f;

    private float moveInput;
    private float moveThreshold = 0.1f;

    AudioManager audioManager;

    public EnergyManager energyManager;

    [Header("Laser")]
    public Laser laser;
    public Transform firePoint;
    public float energyCost = 20f;
    private Laser currentLaser;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (energyManager.GetEnergyLevel() >= energyCost)
            {
                FireLaser();
            }

        }
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

    void FireLaser()
    {

        energyManager.LoseEnergy(energyCost);

        audioManager.LaserZap();

        Instantiate(laser, firePoint.position, firePoint.rotation);
        Vector2 forwardDirection = transform.right;

        StartCoroutine(MoveLaser(laser, forwardDirection));

        
    }

    IEnumerator MoveLaser(Laser laser, Vector2 direction)
    {
        while (laser != null)
        {
            laser.transform.Translate(direction * speed * Time.deltaTime);

            yield return null;
        }
                
    }

}
