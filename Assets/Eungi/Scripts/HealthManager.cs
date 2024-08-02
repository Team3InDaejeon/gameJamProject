using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Type")]
    public bool RedType = false;
    public bool BlueType = false;

    [Header("Slider")]
    public Slider RedHealthBar;
    public Slider BlueHealthBar; 

    [Header("IMG")]
    public GameObject ConditionOBJ;
    public Sprite Red_IMG;
    public Sprite Blue_IMG;

    Image TypeIMG;

    HealthManager(CharacterBase Character)
    {
        
    }

    float Gauge = 0.0f;

    void Start()
    {
        TypeIMG = GetComponent<Image>();

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
            TypeIMG.sprite = Red_IMG;
            if (RedType == true)
            {
                RedHealthBar.value = Amount/100;
            }
        }

        if (Amount < 0.0f)
        {
            BlueType = true;
            RedType = false;
            TypeIMG.sprite = Blue_IMG;
            if (BlueType == true)
            {
                RedHealthBar.value = Amount/100;
            }
        }
    }
}
