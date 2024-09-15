using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private GameObject splashParticles;

    private EdgeCollider2D edgeColl;

    private InteractableWater water;

    AudioManager audioManager;

    private void Awake()
    {
        edgeColl = GetComponent<EdgeCollider2D>();
        water = GetComponent<InteractableWater>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if((waterMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();

            if(rb != null)
            {
                //Spawn particles
                Vector2 localPos = gameObject.transform.localPosition;
                Vector2 hitObjectPos = collision.transform.position;
                Bounds hitObjectBounds = collision.bounds;

                Vector3 spawnPos = Vector3.zero;

                if(collision.transform.position.y >= edgeColl.points[1].y + edgeColl.offset.y + localPos.y)
                {
                    //Hit from above
                    spawnPos = hitObjectPos - new Vector2(0f, hitObjectBounds.extents.y);
                }

                else
                {
                    //Hit from below
                    spawnPos = hitObjectPos + new Vector2(0f, hitObjectBounds.extents.y);
                }

                Instantiate(splashParticles, spawnPos, Quaternion.identity);

                //Clamp splash point to a max velocity
                int multiplier = 1;
                if (rb.velocity.y < 0)
                    multiplier = -1;
                else
                    multiplier = 1;

                float vel = rb.velocity.y * water.forceMultiplier;
                vel = Mathf.Clamp(Mathf.Abs(vel), 0f, water.maxForce);
                vel *= multiplier;

                water.Splash(collision, vel);

                //Play the splash noise when a splash happens
                audioManager.SplashNoise();
            }

        }

    }
}
