using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQSkill : CharacterSkill
{
    [SerializeField]
    float SpeedMultiplier = 2.0f ;

    [SerializeField]
    float SkillDuration = 0.5f;

    float SkillStartTime = 0.0f;
    float OriginalSpeed;

    PlayerQSkill()
    {
        
    }

    public override void StartSkill() 
    {
        if (Player != null)
        {
            Player.SetInvincibility(true);  
            SkillStartTime = Time.time;
            CooldownManager.StartCooldown();
            OriginalSpeed = Player.GetStatComponent().GetMoveSpeed();
            Player.GetStatComponent().SetMoveSpeed(OriginalSpeed * SpeedMultiplier);

            Debug.Log("Dash 시작"); ;
        }
    }
    
    public override void UpdateSkill() 
    {
        if (Player != null && Player.bIsInvincible && Time.time - SkillStartTime >= SkillDuration)
        {
            EndSkill();
        }
    }
    
    public override void EndSkill() 
    {
        if (Player != null)
        {
            Player.SetInvincibility(false);  // 무적 상태 해제
            Player.GetStatComponent().SetMoveSpeed(OriginalSpeed);

            Debug.Log("Dash 종료");
        }
    }
}
