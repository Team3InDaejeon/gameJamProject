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
    public CharacterType CurrentType { get;  private set; }
    public CharacterStat GetStatComponent() { return Stat; }

    virtual protected void Start() 
    {
        Stat = GetComponent<CharacterStat>(); 
        State = CharacterState.Idle;
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
