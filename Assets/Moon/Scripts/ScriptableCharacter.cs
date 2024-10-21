using UnityEngine;


[CreateAssetMenu(fileName = "NewCharacter", menuName = "GameJam/CharacterInfo", order = 1)]
public class ScriptableCharacter : ScriptableCharacterBase
{
    public float CriticalRate;
    public float CriticalDamage;
    public float AttackSpeed;
}
