using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Error = 0,
    Idle,
    Move,
    Jump,
    MeleeAttack,
    QSkill,
    WSkill,
    ESkill,
    RSkill,
    Dead
    //CC,
    // Faint, Sleep, Restraint, Silence, Attraction, Slow,Die, Dragged
};

public enum CharacterType
{
    Red,
    Normal,
    Blue
}


public abstract class CharacterBase : MonoBehaviour
{
    protected CharacterStat Stat;
    protected CharacterState State;
    protected CharacterType CurrentType;

    HealthManager HealthTempUI;

    virtual protected void Awake() 
    {
        Stat = this.GetComponent<CharacterStat>(); 
        State = CharacterState.Idle;
        Stat.OnHealthChanged += (int health) => HealthTempUI.UpdateGauge((int)health);
    }

    void OnDestroy()
    {
        Stat.OnHealthChanged -= (int health) => HealthTempUI.UpdateGauge((int)health);
    }

    virtual protected void SetState(CharacterState NewState) 
    {
        State = NewState;
    }

    virtual protected void SetType(CharacterType NewType)
    {
        CurrentType = NewType;
    }

    // 하위에서 구현해야 할 항목
    protected abstract void Idle();
    protected abstract void Move(float multiplier = 1f);
    protected abstract void Jump();
    protected abstract void SetDead();
}
