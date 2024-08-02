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
    Dictionary<CharacterState, CharacterSkill> SkillMap;

    public bool bIsInvincible { get; private set; }
    public void SetInvincibility(bool invincible)
    {
        bIsInvincible = invincible;
    }

    public event System.Action OnCharacterDead;

    protected override void Start()
    {
        base.Start();
        CharacterRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (Stat != null)
        {
            OnCharacterDead += GameManager.Inst.GameOver;
        }

        SkillMap = new Dictionary<CharacterState, CharacterSkill>();  

        SkillMap.Add(CharacterState.MeleeAttack, GetComponent<PlayerASkill>());
        SkillMap.Add(CharacterState.QSkill, GetComponent<PlayerQSkill>());
        SkillMap.Add(CharacterState.WSkill, GetComponent<PlayerWSkill>());
        SkillMap.Add(CharacterState.ESkill, GetComponent<PlayerESkill>());
        SkillMap.Add(CharacterState.RSkill, GetComponent<PlayerRSkill>());
    }

    private void OnDestroy()
    {
        if (Stat != null)
        {
            OnCharacterDead -= GameManager.Inst.GameOver;
        }
    }

    public void TakeDamage(int damageAmount,EnemyType enemyType=EnemyType.Normal) 
    {
        if (bIsInvincible) 
        {
            return;
        }

        switch (enemyType) 
        {
            case EnemyType.Normal: TakeDamageByNormalEnemy(damageAmount);  break;
            case EnemyType.Red: TakeDamageByRedEnemy(damageAmount); break;
            case EnemyType.Blue: TakeDamageByBlueEnemy(damageAmount); break;
        }

        SetCharacterType();
    }

    private void TakeDamageByNormalEnemy ( int damageAmount)
    {
        switch (CurrentType) 
        {
            case CharacterType.Red: Stat.IncreaseHealth(damageAmount);  break;
            case CharacterType.Normal: SetDead(); break;
            case CharacterType.Blue: Stat.DecreaseHealth(damageAmount);  break;
        }
        
    }

    private void TakeDamageByRedEnemy(int damageAmount)
    {
        Stat.IncreaseHealth(damageAmount);
    }

    private void TakeDamageByBlueEnemy(int damageAmount)
    {
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
            ChangeSkill(CharacterState.MeleeAttack);
        }

        if (Input.GetKeyDown(KeyManager.Inst.QSkill))
        {
            ChangeSkill(CharacterState.QSkill);
        }

        if (Input.GetKeyDown(KeyManager.Inst.WSkill))
        {
            ChangeSkill(CharacterState.WSkill);
        }

        if (Input.GetKeyDown(KeyManager.Inst.ESkill))
        {
            ChangeSkill(CharacterState.ESkill);
        }

        if (Input.GetKeyDown(KeyManager.Inst.RSkill))
        {
            ChangeSkill(CharacterState.RSkill);
        }
    }

    void ChangeSkill(CharacterState NewState) 
    {
        base.SetState(NewState);

        if (SkillMap.TryGetValue(NewState, out var currentSkill))
        {
            CurrentSkill = currentSkill;
            CurrentSkill.StartSkill();
        }
        else
        {
            Debug.LogWarning($"No skill found for state {NewState}");
        }
    }

    // private void CheckOnGround()
    // {
    //     //Debug.Log("bIsGrounded: " + bIsGrounded);

    //     List<RaycastHit> hitInfos;
    //     Vector3 center = transform.position;
    //     hitInfos = Physics.SphereCastAll(center, characterController.radius, Vector3.down, 0.001f).ToList();

    //     hitInfos.RemoveAll(hit => (hit.transform.root.GetComponent<CharacterBase>() != null));

    //     hitInfos.RemoveAll(hit => (hit.transform.root.gameObject.layer == LayerMask.NameToLayer("Projectiles")));

    //     if (hitInfos.Count == 0)
    //     {
    //         // GroundCheckTimer = GROUND_CHECK_TIME;
    //         bIsGrounded = false;
    //         return;
    //     }

    //     for (int i = 0; i < hitInfos.Count; i++)
    //     {
    //         //Debug.Log("Hit Object Name: " + hitInfos[i].collider.gameObject.name);
    //         if (hitInfos[i].collider.tag == "Landable")
    //         {
    //             bIsGrounded = true;
    //             Gravity = DefaultGravity;
    //             return;
    //         }
    //     }
    // }
    
    protected override void Idle() 
    {
        base.SetState(CharacterState.Idle);
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
}
