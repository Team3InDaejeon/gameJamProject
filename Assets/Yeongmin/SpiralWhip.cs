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
        TriggerBox.offset = new Vector2(WhipLength, 0); // ä���� ���̸�ŭ ������ ���� ��ġ
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
        Gizmos.color = Color.yellow; // ������� ������ ����
        Gizmos.matrix = transform.localToWorldMatrix; // ���� ��ǥ�踦 ���߱� ���� ��Ʈ���� ����
        Gizmos.DrawWireCube(TriggerBox.offset, TriggerBox.size); // ����� �ڽ� �׸���
    }
}
