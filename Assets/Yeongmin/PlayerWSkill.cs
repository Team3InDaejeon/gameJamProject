using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWSkill : CharacterSkill
{
    public override void StartSkill() 
    {
        if (Player != null && CooldownManager.CheckCooldown()) 
        {
            Player.GetStatComponent().SetHealth(0);
            CooldownManager.StartCooldown();
        }
    }
    public override void UpdateSkill() { }
    public override void EndSkill() { }
}
