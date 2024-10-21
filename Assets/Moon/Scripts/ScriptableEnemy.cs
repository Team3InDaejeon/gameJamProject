using UnityEngine;


[CreateAssetMenu(fileName = "NewEnemy", menuName = "GameJam/EnemyInfo", order = 0)]
public class ScriptableEnemy : ScriptableCharacterBase
{
    public EnemyType Type;
    public int Health;
}
