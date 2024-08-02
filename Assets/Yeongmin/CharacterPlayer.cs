using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterPlayer : CharacterBase, ICombat
{
    [Header("")]
    [Tooltip("")]
    public GameObject Camera;

    [Tooltip("")] 
    public float DefaultGravity = 1.0f;
    private float Gravity = 1.0f;
    private bool bIsGrounded = false;

    float SlowValue;

    private Rigidbody2D CharacterRigidbody;
    private Animator animator;
    float JumpForce = 10.0f;

    CharacterSkill CurrentSkill;

    public event System.Action OnCharacterDead;

    void Awake() 
    {
        base.Awake();
        CharacterRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (Stat != null)
        {
            OnCharacterDead += GameManager.Inst.GameOver;
        }
    }

    private void OnDestroy()
    {
        if (Stat != null)
        {
            OnCharacterDead -= GameManager.Inst.GameOver;
        }
    }

    public void TakeDamage(EnemyType enemyType, int damageAmount) 
    {
        switch (enemyType) 
        {
            case EnemyType.Normal: TakeDamageByNormalEnemy(damageAmount);  break;
            case EnemyType.Red: TakeDamageByRedEnemy(damageAmount); break;
            case EnemyType.Blue: TakeDamageByBlueEnemy(damageAmount); break;
        }
    }

    private void TakeDamageByNormalEnemy ( int damageAmount)
    {
        // 플레이어가 노말일 때, 노말에게 맞으면 즉사  
        // 플레이어가 레드일 때, 노말에게 맞으면 수치가 증가  
        // 플레이어가 블루일 때, 노말에게 맞으면 수치가 감소  
        switch (CurrentType) 
        {
            case CharacterType.Red: Stat.IncreaseHealth(damageAmount);  break;
            case CharacterType.Normal: SetDead(); break;
            case CharacterType.Blue: Stat.DecreaseHealth(damageAmount);  break;
        }
        
    }

    private void TakeDamageByRedEnemy(int damageAmount)
    {
        // 플레이어가 노말일 때, 레드에게 맞으면 수치 증가  
        // 플레이어가 레드일 때, 레드에게 맞으면 수치가 증가  
        // 플레이어가 블루일 때, 레드에게 맞으면 수치가 증가  
        Stat.IncreaseHealth(damageAmount);
    }

    private void TakeDamageByBlueEnemy(int damageAmount)
    {
        // 플레이어가 노말일 때, 블루에게 맞으면 수치 감소  
        // 플레이어가 레드일 때, 블루에게 맞으면 수치가 감소 
        // 플레이어가 블루일 때, 블루에게 맞으면 수치가 감소  
        Stat.DecreaseHealth(damageAmount);
    }

    private void SetCharacterType() 
    {
        if (Stat.GetHealth() == 0 ) 
        {
            SetType(CharacterType.Normal);
            return;
        }
        else if (Stat.GetHealth() < 100 && Stat.GetHealth() > 0) 
        {
            SetType(CharacterType.Red);
            return;
        }
        else if (Stat.GetHealth() > 0 && Stat.GetHealth() > -100)
        {
            SetType(CharacterType.Blue);
            return;
        }

        SetDead();
    }

    protected override void SetDead() 
    {
        SetState(CharacterState.Dead);

        if (Stat != null)
        {
            OnCharacterDead?.Invoke();
        }
    }

    void Update()
    {
        InputProc();
    }

    private void FixedUpdate() 
    {
       // CheckOnGround();
    }

    private void InputProc() 
    {
        if (false == Input.anyKey)
        {
            Idle();
            return;
        }
        
        if (
            KeyManager.Inst.GetAxisRawHorizontal() != 0 ||
            KeyManager.Inst.GetAxisRawVertical() != 0)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyManager.Inst.Jump))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyManager.Inst.MeleeAttack))
        {
            MeleeAttack();
        }

        if (Input.GetKeyDown(KeyManager.Inst.QSkill))
        {
            QSkill();
        }

        if (Input.GetKeyDown(KeyManager.Inst.WSkill))
        {
            WSkill();
        }

        if (Input.GetKeyDown(KeyManager.Inst.ESkill))
        {
            ESkill();
        }

        if (Input.GetKeyDown(KeyManager.Inst.RSkill))
        {
            RSkill();
        }
    }

    private void CheckOnGround()
    {
        //Debug.Log("bIsGrounded: " + bIsGrounded);

        List<RaycastHit> hitInfos;
        Vector3 center = transform.position;
        hitInfos = Physics.SphereCastAll(center, characterController.radius, Vector3.down, 0.001f).ToList();

        hitInfos.RemoveAll(hit => (hit.transform.root.GetComponent<CharacterBase>() != null));

        hitInfos.RemoveAll(hit => (hit.transform.root.gameObject.layer == LayerMask.NameToLayer("Projectiles")));

        if (hitInfos.Count == 0)
        {
            // GroundCheckTimer = GROUND_CHECK_TIME;
            bIsGrounded = false;
            return;
        }

        for (int i = 0; i < hitInfos.Count; i++)
        {
            //Debug.Log("Hit Object Name: " + hitInfos[i].collider.gameObject.name);
            if (hitInfos[i].collider.tag == "Landable")
            {
                bIsGrounded = true;
                Gravity = DefaultGravity;
                return;
            }
        }
    }
    
    protected override void Idle() 
    {
        base.SetState(CharacterState.Idle);
       // Debug.Log("Idle");
    }

    protected override void Move(float multiplier = 1.0f) 
    {
        base.SetState(CharacterState.Move);

        float horizontalInput = KeyManager.Inst.GetAxisRawHorizontal();

        // 좌우 이동
        Vector2 v = new Vector2(horizontalInput * Stat.GetMoveSpeed(), CharacterRigidbody.velocity.y);
       transform.Translate(v * Stat.GetMoveSpeed() * Time.deltaTime);
    }


    protected override void Jump() 
    {
      //  if (bIsGrounded)
        {
            CharacterRigidbody.velocity = new Vector2(CharacterRigidbody.velocity.x, JumpForce);
        }
    }

    private void MeleeAttack()
    {
        base.SetState(CharacterState.MeleeAttack);
        Debug.Log("MeleeAttack");
    }

    private void QSkill()
    {
        base.SetState(CharacterState.QSkill);
        Debug.Log("QSkill ");
    }

    private void WSkill()
    {
        base.SetState(CharacterState.WSkill);
        Debug.Log("WSkill ");
    }

    private void ESkill()
    {
        base.SetState(CharacterState.ESkill);
        Debug.Log("ESkill ");
    }

    private void RSkill()
    {
        base.SetState(CharacterState.RSkill);
        Debug.Log("RSkill ");
    }

}
