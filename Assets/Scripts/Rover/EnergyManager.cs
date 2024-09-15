using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{

    public Image energyBar;
    public DeathScreen deathScreen;
    public float energyAmount = 100f;
    public float maxEnergy = 100f;
    private bool inStorm = false;
    public float stormDamageRate = 25f;


    private void Start()
    {
        energyBar.fillAmount = energyAmount / maxEnergy;

    }
    // Update is called once per frame
    void Update()
    { 

        //If within the storm, then the player has died
        if(inStorm)
        {
            deathScreen.HasDied();
        }

        
    }

    public void LoseEnergy(float damage)
    {
        energyAmount -= damage;
        energyBar.fillAmount = energyAmount / maxEnergy;
    }

    public void GainEnergy(float healAmount)
    {
        energyAmount += healAmount;
        energyAmount = Mathf.Clamp(energyAmount, 0, maxEnergy);

        energyBar.fillAmount = energyAmount / maxEnergy;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Storm"))
        {
            inStorm = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Storm"))
        {
            inStorm = false;
        }
    }
}
