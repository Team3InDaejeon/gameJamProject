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

    public Rigidbody2D CharacterRigidbody { get; private set; }
    private Animator animator;
    float JumpForce = 10.0f;

    CharacterSkill CurrentSkill;
    Dictionary<CharacterState, CharacterSkill> SkillMap = new Dictionary<CharacterState, CharacterSkill>();  

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
            Stat.OnHealthChanged += (int health) => PlayerUIManager.Inst.UpdateGauge((int)health);
        }


        // SkillMap.Add(CharacterState.MeleeAttack, GetComponent<PlayerASkill>());
        SkillMap.Add(CharacterState.QSkill, GetComponent<PlayerQSkill>());
        SkillMap.Add(CharacterState.WSkill, GetComponent<PlayerWSkill>());
        SkillMap.Add(CharacterState.ESkill, GetComponent<PlayerESkill>());
        SkillMap.Add(CharacterState.RSkill, GetComponent<PlayerRSkill>());

        // Debug.Log(SkillMap.Count);

        // Debugging Scriptable Skill
        foreach (var kvp in SkillMap)
        {
            CharacterSkill skill = kvp.Value;
            if (skill != null && skill.SkillInfo != null)
            {
                Debug.Log($"Index: {skill.SkillInfo.Index}, Name: {skill.SkillInfo.Name},Effect: {skill.SkillInfo.Effect}, Cooldown: {skill.SkillInfo.Cooltime}");
            }
            else
            {
                Debug.LogWarning($"Skill data for state {kvp.Key} is not properly assigned.");
            }
        }
    }

    private void OnDestroy()
    {
        if (Stat != null)
        {
            OnCharacterDead -= GameManager.Inst.GameOver;
            Stat.OnHealthChanged -= (int health) => PlayerUIManager.Inst.UpdateGauge((int)health);
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

        Stat.RaiseHealthChangedEvent();

    }

    private void TakeDamageByNormalEnemy ( int damageAmount)
    {
        switch (CurrentType) 
        {
            case CharacterType.Red: Stat.IncreaseHealth(damageAmount);  break;
            case CharacterType.Normal: SetDead(); break;
            case CharacterType.Blue: Stat.DecreaseHealth(damageAmount);  break;
        }
        Stat.RaiseHealthChangedEvent();
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

        if (CurrentSkill) 
        {
            CurrentSkill.UpdateSkill();
        }
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
            animator.SetBool("isRunning", false);
            return;
        }
        
        if (KeyManager.Inst.GetAxisRawHorizontal() != 0)
        {
            Move();
            animator.SetBool("isRunning", true);
        }

        if (Input.GetKeyDown(KeyManager.Inst.Jump))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyManager.Inst.MeleeAttack))
        {
            base.SetState(CharacterState.MeleeAttack);
            MeleeAttack();
            
            animator.SetTrigger("attackTrigger");
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

    public void MoveWithMultiplier(float Force) 
    {
        if (Input.GetKeyDown(KeyManager.Inst.QSkill)) 
        {
            Debug.Log("MoveSpeed From Move: " + Stat.GetMoveSpeed());
            // 좌우 이동
            Vector2 direction = transform.right;
            Vector2 force = direction * Force;
            CharacterRigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }

    protected override void Move(float multiplier = 1.0f)
    {
        base.SetState(CharacterState.Move);
        int horizontalInput = KeyManager.Inst.GetAxisRawHorizontal();

        // 좌우 이동
        Vector2 v = new Vector2(horizontalInput * Stat.GetMoveSpeed(), CharacterRigidbody.velocity.y);
        transform.Translate(v  * Time.deltaTime);
    }


    protected override void Jump() 
    {
        if (bIsGrounded)
        {
            CharacterRigidbody.velocity = new Vector2(CharacterRigidbody.velocity.x, JumpForce);
        }
    }

    public void MeleeAttack() 
    {
        // Stat.GetATK();
    }
}
