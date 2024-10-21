using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class SkillCooldownManager
{
    public event Action<float> OnCooldownUpdateEvent;

    private float LastSkillTime;
    public float CooldownDuration { get; private set; }

    public float RemainingCooldown => Mathf.Max(0, CooldownDuration - (Time.time - LastSkillTime));
    private bool bIsCooldownActive = false;

    public void Initialize(float NewCooldownDuration) 
    {
        CooldownDuration = NewCooldownDuration;
        LastSkillTime = -NewCooldownDuration; 
    }

    void Start() 
    {
        LastSkillTime = -CooldownDuration;
    }

    public void StartCooldown()
    {
        LastSkillTime = Time.time;
        bIsCooldownActive = true;
    }

    public void Update() 
    {
        if (bIsCooldownActive)
        {
            float remainingTime = RemainingCooldown;

            if (remainingTime <= 0)
            {
                bIsCooldownActive = false;
                remainingTime = 0;
            }
            OnCooldownUpdateEvent?.Invoke(remainingTime);
        }
    }

    public bool CheckCooldown()
    {
        return Time.time >= LastSkillTime + CooldownDuration;
    }
}