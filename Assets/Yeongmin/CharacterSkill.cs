using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSkill : MonoBehaviour
{
    public ScriptableSkill SkillInfo;
    int CurrentSkillCoolTime = 0;
    protected CharacterPlayer Player;
    SkillCooltimeUI cooltimeUI;

    protected SkillCooldownManager CooldownManager;

    void Start()
    {
        Player = GetComponent<CharacterPlayer>();

        CurrentSkillCoolTime = SkillInfo.Cooltime;
        CooldownManager = new SkillCooldownManager(SkillInfo.Cooltime);
    }

    public abstract void StartSkill();
    public abstract void UpdateSkill();
    public abstract void EndSkill();
}
