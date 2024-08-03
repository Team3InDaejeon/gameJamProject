using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerUIManager : MonoBehaviour
{
    private static PlayerUIManager playerUIManager;
    public static PlayerUIManager Inst
    {
        get
        {
            if (playerUIManager == null)
            {
                playerUIManager = FindObjectOfType<PlayerUIManager>();
                if (playerUIManager == null)
                {
                    Debug.LogError("playerUIManager does Not Exist!");
                }
            }
            return playerUIManager;
        }
    }
    [Header("Player_Type")]
    public bool RedType = false;
    public bool BlueType = false;

    [Header("HealthBar_Slider")]
    public Slider RedHealthBar;
    public Slider BlueHealthBar; 

    [Header("Condition_IMG")]
    public GameObject ConditionOBJ;
    public Sprite Red_IMG;
    public Sprite Blue_IMG;

    Image TypeIMG;

    PlayerUIManager(CharacterBase Character)
    {
        
    }

    float Gauge = 0.0f;

    void Start()
    {
        TypeIMG = ConditionOBJ.GetComponent<Image>();

        RedHealthBar.value = 0.0f;
        BlueHealthBar.value = 0.0f;
        RedType = true;
        BlueType = true;
    }

    void Update()
    {
        
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
                BlueHealthBar.value = Amount/100;
            }
        }
    }
}
