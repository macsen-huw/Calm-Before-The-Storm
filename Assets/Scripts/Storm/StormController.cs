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
    [SerializeField] private float expansionRate = 10f;

    //Max Width of the storm
    [SerializeField] private float maxWidth = 1000f;

    //Initial Width of the storm
    [SerializeField] private float currentWidth = 1f;

    //Rate at which rain falls down
    [SerializeField] private float emissionRate = 50;

    private ParticleSystem pSystem;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;

    //private BoxCollider2D stormCollider; 

    //Initial position of the particle system
    private Vector3 initialPosition;

    private void Start()
    {
       /* stormCollider = GetComponent<BoxCollider2D>();
        stormCollider.isTrigger = true;*/

        pSystem = GetComponent<ParticleSystem>();

        shapeModule = pSystem.shape;
        emissionModule = pSystem.emission;

        initialPosition = shapeModule.position;
    }

    void Update()
    {
        
        if(currentWidth < maxWidth)
        {
            //Expand the width of the storm over time
            currentWidth += expansionRate * Time.deltaTime;

            //Make the particle system's box wider
            shapeModule.scale = new Vector3(currentWidth, shapeModule.scale.y, shapeModule.scale.z);

            shapeModule.position = new Vector3(initialPosition.x + (currentWidth / 2), initialPosition.y, initialPosition.z);

            //Adjust rate so that more raindrops are added, rather than the existing raindrops getting wider
            emissionModule.rateOverTime = emissionRate * currentWidth;

        }
    }

    /*
     TO BE IMPLMENTED WHEN THE PLAYER IS ADDED 
     * private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            //Do something
        }
    }*/
}
