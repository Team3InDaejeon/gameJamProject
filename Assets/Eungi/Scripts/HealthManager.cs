using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private static HealthManager healthManager;
    public static HealthManager Inst
    {
        get
        {
            if (healthManager == null)
            {
                healthManager = FindObjectOfType<HealthManager>();
                if (healthManager == null)
                {
                    Debug.LogError("GameManager does Not Exist!");
                }
            }
            return healthManager;
        }
    }
    [Header("Type")]
    public bool RedType = false;
    public bool BlueType = false;

    [Header("Slider")]
    public Slider RedHealthBar;
    public Slider BlueHealthBar; 

    HealthManager(CharacterBase Character)
    {
        
    }

    float Gauge = 0.0f;

    void Start()
    {
        RedHealthBar.value = 0.0f;
        BlueHealthBar.value = 0.0f;
        RedType = true;
        BlueType = true;
    }

    public void UpdateGauge(int Amount)
    {
        if (Amount == 0.0f)
        {
            RedType = true;
            BlueType = true;
        }

        if (Amount > 0.0f)
        {
            RedType = true;
            BlueType = false;
            if (RedType == true)
            {
                RedHealthBar.value = Amount/100;
            }
        }

        if (Amount < 0.0f)
        {
            BlueType = true;
            RedType = false;
            if (BlueType == true)
            {
                RedHealthBar.value = Amount/100;
            }
        }
    }
}
