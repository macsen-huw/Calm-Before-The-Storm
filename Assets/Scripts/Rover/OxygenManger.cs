using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenManger : MonoBehaviour
{
    [SerializeField] private Transform waterCheck;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Image oxygenBar;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenDepletionRate = 1f;
    [SerializeField] private float oxygenRegenerationRate = 1f;
    
    private float oxygenLevel = 100f;
    private bool isUnderwater = false;


    // Start is called before the first frame update
    void Start()
    {
        oxygenBar.fillAmount = maxOxygen;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isUnderwater = Physics2D.OverlapCircle(waterCheck.position, 1.0f, waterLayer);

        if (isUnderwater) {
            float depletedOxygen = oxygenDepletionRate * Time.deltaTime;
            DepleteOxygen(depletedOxygen);
        }
        
        else {
            //Making less calculations by only replenishing if not already replenished
            if (oxygenLevel != maxOxygen) {
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

}
