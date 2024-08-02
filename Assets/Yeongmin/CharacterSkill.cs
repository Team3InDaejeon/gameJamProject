using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    [SerializeField]
    float SkillCoolTime = 0;

    [SerializeField]
    private string SkillName = "";

    [SerializeField]
    private string SkillSerialName = "";

    [SerializeField]
    private string SkillDesc = "";

    float CurrentSkillCoolTime = 0;

    void Start()
    {
        CurrentSkillCoolTime = SkillCoolTime;
    }

    public void StartSkill() 
    {
        Debug.Log("StartSkill");
    }

    public void UpdateSkill()
    {

    }

    public void EndSkill()
    {
        Debug.Log("EndSkill");
    }
}
