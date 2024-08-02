﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("Character Stat")]
    [SerializeField]
    protected int MaxHealth = 100;
    [SerializeField]
    protected int MinHealth = 0;
    [SerializeField]
    protected int MaxStrikingPower = 10;
    [SerializeField]
    protected float MaxMoveSpeed = 10.0f;
    [SerializeField]
    protected ScriptableCharacterBase CharacterInfo;

    protected int CurrentHealth;
    protected int Current​StrikingPower;
    protected float Current​MoveSpeed;


    // Unity doesn't call constructors for MonoBehaviours, use Awake or Start instead
    private void Awake()
    {
        // If CharacterInfo is assigned, initialize the character stats from it
        if (CharacterInfo != null)
        {
            MaxStrikingPower = CharacterInfo.StrikingPower;
        }
        Debug.Log("CharacterStat Awake");

        // Set the current stats to their respective max values
        CurrentHealth = MaxHealth;
        CurrentStrikingPower = MaxStrikingPower;
        CurrentMoveSpeed = MaxMoveSpeed;
    }

    public int GetHealth()  { return CurrentHealth;}
    public int GetATK()  { return Current​StrikingPower;}
    public float GetMoveSpeed()  { return Current​MoveSpeed;}

    public void SetHealth(float NewHealth)
    {
        CurrentHealth = (int)Mathf.Clamp(NewHealth, MinHealth, MaxHealth);
    }

    public void SetATK(float NewStrikingPower)
    {
        Current​StrikingPower = (int)Mathf.Clamp(NewStrikingPower, 0, MaxStrikingPower);
    }

    public void SetMoveSpeed(float NewMoveSpeed)
    {
        Current​MoveSpeed = Mathf.Clamp(NewMoveSpeed, 0, MaxMoveSpeed);
    }

    public void IncreaseHealth(int Amount)
    {
        SetHealth(CurrentHealth + Amount);
    }

    public void DecreaseHealth(int Amount)
    {
        SetHealth(CurrentHealth - Amount);
    }

    public void IncreaseATK(int Amount)
    {
        SetATK(Current​StrikingPower + Amount);
    }

    public void DecreaseATK(int Amount)
    {
        SetATK(Current​StrikingPower - Amount);
    }

    public void IncreaseMoveSpeed(float Amount)
    {
        SetATK(Current​MoveSpeed + Amount);
    }

    public void DecreaseMoveSpeed(float Amount)
    {
        SetATK(Current​MoveSpeed - Amount);
    }
}