using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRSkill : CharacterSkill
{
    [SerializeField]
    int DamageAmount = 50;

    int OriginalATK = 0;

    public override void StartSkill() 
    {
        if (Player != null && CooldownManager.CheckCooldown())
        {
           if (Player.CurrentType != CharacterType.Blue) 
           {
               return;
           }
            CooldownManager.StartCooldown();
            OriginalATK = Player.GetStatComponent().GetATK();
            Player.GetStatComponent().SetATK(DamageAmount);
            Player.MeleeAttack();

            EndSkill();
        }
    }

    public override void UpdateSkill() 
    {
        if (Player == null)
        {
            return;
        }

        base.UpdateSkill();
    }

    public override void EndSkill() 
    {
        if (Player != null) 
        {
            Player.GetStatComponent().SetATK(OriginalATK);
        }
    }
}
