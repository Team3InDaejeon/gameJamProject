using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    public float SkillCool = 0.0f;

    [Header("UI")]
    public Image img_Skill;
    public Text coolTimeCounter;

    [Header("Key")]
    public bool Q_Key;
    public bool W_Key;
    public bool E_Key;
    public bool R_Key;

    private bool canUseSkill = true;
    public float currentCoolTime;

    void Start()
    {
        img_Skill.fillAmount = 0.0f;
    }

    void Update()
    {
        if (Q_Key == true && canUseSkill == true) //Q_Key
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canUseSkill = false;
                currentCoolTime = SkillCool;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (W_Key == true && canUseSkill == true) //W_Key
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                canUseSkill = false;
                currentCoolTime = SkillCool;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (E_Key == true && canUseSkill == true) //E_Key
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canUseSkill = false;
                currentCoolTime = SkillCool;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (R_Key == true && canUseSkill == true) //R_Key
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                canUseSkill = false;
                currentCoolTime = SkillCool;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
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
            yield return null;
        }

        canUseSkill = true;

        yield break;
    }
}
