using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralWhip : MonoBehaviour
{
    [SerializeField]
    private float WhipLength = 3.2f;
    private int HitDamage = 0;
    private Transform CharacterTransform;
    [SerializeField]
    private BoxCollider2D TriggerBox;

    private float CurrentAngle = 0.0f;

    public bool bIsAttackActive = false;

    public void Initialize( int NewHitDamage, Transform NewCharacterTransform)
    {
        HitDamage = NewHitDamage;
        CharacterTransform = NewCharacterTransform;
    }

    void Awake() 
    {
        // TriggerBox = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        TriggerBox.offset = new Vector2(WhipLength, 0); // 채찍의 길이만큼 떨어진 곳에 위치
    }

    public void SetWhipAngle(float angle)
    {
        if (bIsAttackActive) 
        {
            CurrentAngle = angle % 360f;
            transform.position = CharacterTransform.position;
            transform.rotation = Quaternion.Euler(0, 0, CurrentAngle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (bIsAttackActive) 
        {
            ICombat target = other.GetComponent<ICombat>();
            if (target != null)
            {
                target.TakeDamage(HitDamage);
            }
            else 
            {
                Debug.Log("target is null");
            }
        }
        else
        {
            Debug.Log("bIsAttackActive is false");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 기즈모의 색상을 설정
        Gizmos.matrix = transform.localToWorldMatrix; // 로컬 좌표계를 맞추기 위한 매트릭스 설정
        Gizmos.DrawWireCube(TriggerBox.offset, TriggerBox.size); // 기즈모 박스 그리기
    }
}
