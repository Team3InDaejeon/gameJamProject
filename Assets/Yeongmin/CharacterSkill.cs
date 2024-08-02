using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSkill : MonoBehaviour
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

    public abstract void StartSkill();
    public abstract void UpdateSkill();
    public abstract void EndSkill();
}
