using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{

    [SerializeField] private Image energyBar;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float stormDamageRate = 25f;

    private float energyAmount = 100f;
    private bool inStorm = false;

    private void Start()
    {
        energyBar.fillAmount = energyAmount / maxEnergy;

    }
    // Update is called once per frame
    void FixedUpdate()
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
