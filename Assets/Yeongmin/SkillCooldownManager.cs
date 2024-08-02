using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillCooldownManager : MonoBehaviour
{
    private float lastSkillTime;
    private float cooldownDuration;

    public SkillCooldownManager(float cooldownDuration)
    {
        this.cooldownDuration = cooldownDuration;
        lastSkillTime = -cooldownDuration; 
    }

    public bool CheckCooldown()
    {
        return Time.time >= lastSkillTime + cooldownDuration;
    }

    public void StartCooldown()
    {
        lastSkillTime = Time.time;
    }
}