using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSkill : MonoBehaviour
{
    [SerializeField]
    ScriptableSkill SkillInfo;

    float CurrentSkillCoolTime = 0;

    void Start()
    {
        CurrentSkillCoolTime = SkillInfo.Cooltime;
    }

    public abstract void StartSkill();
    public abstract void UpdateSkill();
    public abstract void EndSkill();
}
