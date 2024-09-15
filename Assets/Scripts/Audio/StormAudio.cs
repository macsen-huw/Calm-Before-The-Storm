using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormAudio : MonoBehaviour
{

    public AudioSource stormAudio;
    public Transform player;

    //Max distance where sound is audible
    public float maxDistance = 75f;

    private ParticleSystem pSystem;
    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        shapeModule = pSystem.shape;
    }

    void Update()
    {
        Vector3 particleSystemWorld = transform.TransformPoint(shapeModule.position);

        float distance = Vector2.Distance(particleSystemWorld, player.position);

        //print("Storm Position: " + particleSystemWorld);
        if(distance > maxDistance)
         stormAudio.volume = 0;
        else
        {
            float volume = 1 - (distance / maxDistance);
            stormAudio.volume = Mathf.Clamp01(volume);
        }
      

    }
}
