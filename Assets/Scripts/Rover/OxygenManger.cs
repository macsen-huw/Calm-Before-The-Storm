using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenManger : MonoBehaviour
{
    public Image oxygenBar;
    public DeathScreen deathScreen;
    public EdgeCollider2D waterCollider;
    public float oxygenLevel = 100f;
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float oxygenRegenerationRate = 1f;
    private bool isUnderWater = false;


    // Start is called before the first frame update
    void Start()
    {
        oxygenBar.fillAmount = oxygenLevel / maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 point = transform.position;
        Vector2 closestColliderPoint = waterCollider.ClosestPoint(point);

        float difference = (point - closestColliderPoint).y;

        if (difference <= 0)
            isUnderWater = true;
        else
            isUnderWater = false;

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

}
