using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Skill_UI")]
    public Image img_Skill;
    public Text coolTimeCounter;

    private float SkillCool = 0.0f;
    private float currentCoolTime;
    private bool canUseSkill = true;

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
                    Debug.LogError("GameManager does Not Exist!");
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
        TypeIMG = GetComponent<Image>();

        img_Skill.fillAmount = 0.0f;

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

    IEnumerator CoolTime(float cool) //Skill Filter
    {
        while(img_Skill.fillAmount > 0.0f)
        {
            currentCoolTime -= 1 * Time.smoothDeltaTime;
            coolTimeCounter.text = "" + (int)currentCoolTime;

            if (currentCoolTime <= 0)
            {
                coolTimeCounter.text = "";
            }

            img_Skill.fillAmount -= 1 * Time.smoothDeltaTime/cool;
            Debug.Log("Test");

            yield return null;
        }
        canUseSkill = true;

        yield break;
    }
}
