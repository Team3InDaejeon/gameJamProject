using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterSkill : MonoBehaviour
{
    public ScriptableSkill SkillInfo;
    protected CharacterPlayer Player;

    [SerializeField]
    private SkillCooltimeUI CooltimeUI;

    protected SkillCooldownManager CooldownManager;

    void Awake()
    {
        Player = GetComponent<CharacterPlayer>();
        CooldownManager = new SkillCooldownManager();
    }

    void Start()
    {
        if (SkillInfo != null)
        {
            CooldownManager.Initialize(SkillInfo.Cooltime);
        }

        if (CooltimeUI != null)
        {
            CooltimeUI.Initizlize(CooldownManager);
        }
    }

    public abstract void StartSkill();
    
    void Update() 
    {
        CooldownManager.Update();
    }

    public abstract void UpdateSkill();
    public abstract void EndSkill();
}
