using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rover : MonoBehaviour
{
    [SerializeField] private Transform waterCheck;
    [SerializeField] private Rigidbody2D bodyRigidbody;
    [SerializeField] private Rigidbody2D[] tyreRigidbodies;

    [SerializeField] private LayerMask waterLayer;
    
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float submarineSpeed = 1.0f;
    [SerializeField] private float thrustSpeed = 1.0f;

    private float moveInput;
    private float thrustInput;
    private float moveThreshold = 0.1f;

    private bool inSubmarineMode = false;
    private bool isUnderwater = false;
    private WheelJoint2D[] wheelJoints;

    private Animator animator;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        wheelJoints = GetComponents<WheelJoint2D>();
    }

    public void OnMove (InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
    }

    public void OnThrust (InputAction.CallbackContext context) {
        thrustInput = context.ReadValue<float>();
    }

    public void OnToggleMode (InputAction.CallbackContext context) {
        if (context.performed) {
            inSubmarineMode = !inSubmarineMode;
            animator.SetBool("submarineMode", inSubmarineMode);
            
            foreach(WheelJoint2D wheelJoint in wheelJoints) {
                wheelJoint.enabled = !inSubmarineMode;
            }

            foreach(Rigidbody2D tyreRigidbody in tyreRigidbodies) {
                tyreRigidbody.isKinematic = inSubmarineMode;
            }

            if (inSubmarineMode) {
                bodyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                bodyRigidbody.rotation = 0.0f;
            }
            else 
                bodyRigidbody.constraints = RigidbodyConstraints2D.None;
        }         
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isUnderwater = Physics2D.OverlapCircle(waterCheck.position, 1.0f, waterLayer);

        if (!inSubmarineMode) {
            foreach (Rigidbody2D tyre in tyreRigidbodies) {
                tyre.AddTorque(-moveInput * speed);
            }
        }

        if (inSubmarineMode && isUnderwater) {
            bodyRigidbody.AddForce(new Vector2(moveInput * submarineSpeed, thrustInput * thrustSpeed)); 
        }

        //If rover is moving
        if (Math.Abs(moveInput) > moveThreshold || Math.Abs(thrustInput) > moveThreshold)
            audioManager.RoverMoving();
        //Otherwise is isn't moving
        else 
            audioManager.RoverNotMoving();


    }
}
