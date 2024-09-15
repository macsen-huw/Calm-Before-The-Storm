using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreManager : MonoBehaviour
{

    public GameObject ore;
    public EnergyManager energyManager;
    public float energyValue = 20f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ore.SetActive(false);
            energyManager.GainEnergy(energyValue);
            
        }

    }
}
