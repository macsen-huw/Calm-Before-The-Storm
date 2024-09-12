using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(ParticleSystem))]
//[RequireComponent(typeof(BoxCollider2D))]

public class StormController : MonoBehaviour
{

    [Header("Particle System")]

    //Rate at which the storm expands horizontally
    [SerializeField] private float movementRate = 10f;
    [SerializeField] private float colliderOffset = -2f;

    //Max Width of the storm
    //[SerializeField] private float maxWidth = 1000f;

    //Initial Width of the storm
    private float currentWidth = 1f;

    //Rate at which rain falls down
    //[SerializeField] private float emissionRate = 50;

    private ParticleSystem pSystem;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;

    private BoxCollider2D stormCollider; 

    //Initial position of the particle system
    private Vector3 initialPosition;
    private Vector2 initialColliderPosition;

    private void Start()
    {
        stormCollider = GetComponent<BoxCollider2D>();
        stormCollider.isTrigger = true;

        pSystem = GetComponent<ParticleSystem>();

        shapeModule = pSystem.shape;
        emissionModule = pSystem.emission;

        initialPosition = shapeModule.position;
    }

    void Update()
    {
        
        //Expand the width of the storm over time
        currentWidth += movementRate * Time.deltaTime;

        //Move the particle system along
        shapeModule.position = new Vector3(initialPosition.x + (currentWidth / 2), initialPosition.y, initialPosition.z);
        
        //Add an offset to the collider box to match with the rain drops
        stormCollider.offset = new Vector2((initialPosition.x + (currentWidth / 2)) + colliderOffset, stormCollider.offset.y);

    }

 
      private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("RoverBody"))
        {
            print("You've been hit!");

            //End Game
        }
    }
}
