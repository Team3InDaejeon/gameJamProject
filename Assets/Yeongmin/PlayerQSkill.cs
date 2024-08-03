using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQSkill : CharacterSkill
{
    [SerializeField]
    float Force = 2.0f ;

    [SerializeField]
    float SkillDuration = 0.4f;
    float SkillStartTime = 0.0f;

    public override void StartSkill() 
    {
        if (Player != null && CooldownManager.CheckCooldown())
        {
            Player.SetInvincibility(true);  
            SkillStartTime = Time.time;
            CooldownManager.StartCooldown();
        }
    }
    
    public override void UpdateSkill() 
    {
        if (Player == null)
        {
            return;
        }

        if (Player.bIsInvincible) 
        {
            if (Time.time - SkillStartTime >= SkillDuration)
            {
                EndSkill();
            }
            else
            {
                Player.MoveWithMultiplier(Force);
            }
        }
    }
    
    public override void EndSkill() 
    {
        if (Player == null)
        {
            return;
        }

        Player.SetInvincibility(false);
        Player.CharacterRigidbody.velocity = Vector2.zero; 
    }
}
