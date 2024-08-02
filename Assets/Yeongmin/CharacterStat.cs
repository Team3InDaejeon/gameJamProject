using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [SerializeField]
    protected float MaxHealth = 0.0f;
    [SerializeField]
    protected float MaxStrikingPower = 0.0f;
    [SerializeField]
    protected float MaxMoveSpeed = 10.0f;

    protected float CurrentHealth;
    protected float Current​StrikingPower;
    protected float Current​MoveSpeed;

   

    public CharacterStat()
    {
        CurrentHealth = MaxHealth;
        Current​StrikingPower = MaxStrikingPower;
        Current​MoveSpeed = MaxMoveSpeed;
    }

    public float GetHealth()  { return CurrentHealth;}
    public float GetATK()  { return Current​StrikingPower;}
    public float GetMoveSpeed()  { return Current​MoveSpeed;}

    public void SetHealth(float NewHealth)
    {
        CurrentHealth = Mathf.Clamp(NewHealth, 0, MaxHealth);
    }

    public void SetATK(float NewStrikingPower)
    {
        Current​StrikingPower = Mathf.Clamp(NewStrikingPower, 0, MaxStrikingPower);
    }

    public void SetMoveSpeed(float NewMoveSpeed)
    {
        Current​MoveSpeed = Mathf.Clamp(NewMoveSpeed, 0, MaxMoveSpeed);
    }

    public void IncreaseHealth(float Amount)
    {
        SetHealth(CurrentHealth + Amount);
    }

    public void DecreaseHealth(float Amount)
    {
        SetHealth(CurrentHealth - Amount);
    }

    public void IncreaseATK(float Amount)
    {
        SetATK(Current​StrikingPower + Amount);
    }

    public void DecreaseATK(float Amount)
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