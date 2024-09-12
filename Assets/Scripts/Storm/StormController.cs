using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(PolygonCollider2D))]

public class StormController : MonoBehaviour
{

    [Header("Particle System")]

    //Rate at which the storm expands horizontally
    [SerializeField] private float movementRate = 10f;

    //Initial Width of the storm
    private float currentWidth = 1f;

    private ParticleSystem pSystem;
    private ParticleSystem.ShapeModule shapeModule;

    private PolygonCollider2D stormCollider; 

    //Initial position of the particle system
    private Vector3 initialPosition;

    private float delay = 5;
    private bool isActive = false;

    private void Start()
    {
        stormCollider = GetComponent<PolygonCollider2D>();
        stormCollider.isTrigger = true;

        pSystem = GetComponent<ParticleSystem>();

        shapeModule = pSystem.shape;

        initialPosition = shapeModule.position;
        stormCollider.offset = new Vector2(initialPosition.x, stormCollider.offset.y);

        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(delay);

        isActive = true;
    }

    void Update()
    {

        if (isActive)
        {
            //Expand the width of the storm over time
            currentWidth += movementRate * Time.deltaTime;

            //Move the particle system along
            shapeModule.position = new Vector3(initialPosition.x + (currentWidth / 2), initialPosition.y, initialPosition.z);

            //Add an offset (-18) to the collider box to match with the rain drops
            stormCollider.offset = new Vector2((initialPosition.x + (currentWidth / 2)) - 18, stormCollider.offset.y);

        }

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
