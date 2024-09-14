using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public Image healthBar;
    public DeathScreen deathScreen;
    public float healthAmount = 100f;
    public float maxHealth = 100f;
    private bool inStorm = false;
    public float stormDamageRate = 25f;

    private void Awake()
    {
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 

        if(inStorm)
        {
            float healthToRemove = stormDamageRate * Time.deltaTime;
            TakeDamage(healthToRemove);
        }

        //Check if the player has died
        if(healthAmount <= 0)
        {
            deathScreen.HasDied();
        }
        
    }

    //Take damage from player
    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    //Give health to player
    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        healthBar.fillAmount = healthAmount / maxHealth;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Storm"))
        {
            print("Currently in Storm");
            inStorm = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Storm"))
        {
            print("Currently out of storm");
            inStorm = false;
        }
    }
}
