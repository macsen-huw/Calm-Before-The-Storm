using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StormController : MonoBehaviour
{

    [Header("Particle System")]

    //Rate at which the storm expands horizontally
    [SerializeField] private float expansionRate = 2f;

    //Max Width of the storm
    [SerializeField] private float maxWidth = 100f;

    //Initial Width of the storm
    [SerializeField] private float currentWidth = 1f;

    //Rate at which rain falls down
    [SerializeField] private float emissionRate = 100;

    private ParticleSystem pSystem;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;

    //Initial position of the particle system
    private Vector3 initialPosition;

    private void Start()
    {
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
}
