using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    public float SkillCool = 0.0f;

    [Header("UI")]
    public Image img_Skill;

    [Header("Key")]
    public bool Q_Key;
    public bool W_Key;
    public bool E_Key;
    public bool R_Key;

    private bool canUseSkill = true;

    void Start()
    {
        img_Skill.fillAmount = 0.0f;
    }

    void Update()
    {
        if (Q_Key == true && canUseSkill == true) //Q키가 활성화 됐을 때
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canUseSkill = false;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (W_Key == true && canUseSkill == true) //W키가 활성화 됐을 때
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                canUseSkill = false;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (E_Key == true && canUseSkill == true) //E키가 활성화 됐을 때
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canUseSkill = false;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }

        else if (R_Key == true && canUseSkill == true) //R키가 활성화 됐을 때
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                canUseSkill = false;
                img_Skill.fillAmount = 1.0f;
                StartCoroutine(CoolTime(SkillCool));
            }
        }
    }

    IEnumerator CoolTime(float cool) //Skill Filter
    {
        while(img_Skill.fillAmount > 0.0f)
        {
            img_Skill.fillAmount -= 1 * Time.smoothDeltaTime/cool;
            yield return null;
        }

        canUseSkill = true;

        yield break;
    }
}
