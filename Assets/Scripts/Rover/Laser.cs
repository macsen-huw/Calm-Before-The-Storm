using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public GameObject laser;
    public float speed = 10f;
    public float lifetime = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    //Destroy as soon as it collides with anything
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((!collision.collider.CompareTag("Water")) & (!collision.collider.CompareTag("Player")))
        {
            Destroy(gameObject);
        }


    }
}
