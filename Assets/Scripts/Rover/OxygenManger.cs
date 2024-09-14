using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenManger : MonoBehaviour
{
    public Image oxygenBar;
    public DeathScreen deathScreen;
    public float oxygenLevel = 100f;
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float oxygenRegenerationRate = 1f;
    private bool isUnderWater = false;

    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isUnderWater)
        {
            float depletedOxygen = oxygenDepletionRate * Time.deltaTime;
            DepleteOxygen(depletedOxygen);
        }
        
        else
        {
            //Making less calculations by only replenishing if not already replenished
            if(oxygenLevel != maxOxygen)
            {
                float regeneratedOxygen = oxygenRegenerationRate * Time.deltaTime;
                RegenerateOxygen(regeneratedOxygen);
            }
        }

        //Run out of oxygen -> Death
        if (oxygenLevel <= 0)
            deathScreen.HasDied();
    }

    //Take oxygen away from player
    public void DepleteOxygen(float oxygen)
    {
        oxygenLevel -= oxygen;
        oxygenBar.fillAmount = oxygenLevel / maxOxygen;
    }

    //Give oxygen to player
    public void RegenerateOxygen(float oxygen)
    {
        oxygenLevel += oxygen;
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);
        oxygenBar.fillAmount = oxygenLevel / maxOxygen;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("On Trigger Enter Collision");
        if (other.CompareTag("Water"))
        {
            print("Currently under water");
            isUnderWater = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        print("On Trigger Exit Collision");
        if (other.CompareTag("Water"))
        {
            print("Currently above water");
            isUnderWater = false;
        }
    }
}
