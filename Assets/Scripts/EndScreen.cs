using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player") || collider.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("End Scene");
        }
    }
}
