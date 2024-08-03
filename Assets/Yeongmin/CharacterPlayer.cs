using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterPlayer : CharacterBase, ICombat
{
    public LayerMask BackGroundLayerMask;

    [Header("")]
    [Tooltip("")]
    public GameObject Camera;

    [Tooltip("")]
    public float DefaultGravity = 1.0f;
    private float Gravity = 1.0f;
    private bool bIsGrounded = false;
    public LayerMask groundLayer; // 그라운드 레이어 마스크
    float SlowValue;

    public Rigidbody2D CharacterRigidbody { get; private set; }
    private Animator animator;
    float JumpForce = 10.0f;
    [SerializeField]
    float RayLength = 0.5f;
    float AirRayLength = 0.5f;

    CharacterSkill CurrentSkill;
    Dictionary<CharacterState, CharacterSkill> SkillMap = new Dictionary<CharacterState, CharacterSkill>();
    SpiralWhip SpiralWhipWeapon;
    int layermask;
    [Header("Effect Setting")]
    public GameObject hitEffect;
    public GameObject takeDamageEffect;

    public bool bIsInvincible { get; private set; }

    public event System.Action OnCharacterDead;

    bool bIsFlipped = false;

    public void SetInvincibility(bool invincible)
    {
        bIsInvincible = invincible;
    }


    protected override void Start()
    {
        base.Start();

        CharacterRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpiralWhipWeapon = GetComponentInChildren<SpiralWhip>();
        layermask = (-1) - (1 << LayerMask.NameToLayer("Background"));
        if (Stat != null)
        {
            OnCharacterDead += GameManager.Inst.GameOver;
            Stat.OnHealthChanged += (int health) => PlayerUIManager.Inst.UpdateGauge((int)health);
        }

        SkillMap.Add(CharacterState.QSkill, GetComponent<PlayerQSkill>());
        SkillMap.Add(CharacterState.WSkill, GetComponent<PlayerWSkill>());
        SkillMap.Add(CharacterState.ESkill, GetComponent<PlayerESkill>());
        SkillMap.Add(CharacterState.RSkill, GetComponent<PlayerRSkill>());

        if (SpiralWhipWeapon)
        {
            SpiralWhipWeapon.Initialize(Stat.GetATK(), transform);
        }

        // Debugging Scriptable Skill
        /*
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
        }*/

        int BackgroundLayer = LayerMask.NameToLayer("Background");
        BackGroundLayerMask = ~(1 << BackgroundLayer);
    }

    private void OnDestroy()
    {
        if (Stat != null)
        {
            OnCharacterDead -= GameManager.Inst.GameOver;
            Stat.OnHealthChanged -= (int health) => PlayerUIManager.Inst.UpdateGauge((int)health);
        }
    }
    public void TakeDamage(int damageAmount, EnemyType enemyType = EnemyType.Normal)
    {
        if (bIsInvincible)
        {
            return;
        }

        switch (enemyType)
        {
            case EnemyType.Normal: TakeDamageByNormalEnemy(damageAmount); break;
            case EnemyType.Red: TakeDamageByRedEnemy(damageAmount); break;
            case EnemyType.Blue: TakeDamageByBlueEnemy(damageAmount); break;
        }

        SetCharacterType();

        Stat.RaiseHealthChangedEvent();

        animator.SetTrigger("Attacked");
        GameObject effect = Instantiate(takeDamageEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    private void TakeDamageByNormalEnemy(int damageAmount)
    {
        switch (CurrentType)
        {
            case CharacterType.Red: Stat.IncreaseHealth(damageAmount); break;
            case CharacterType.Normal: SetDead(); break;
            case CharacterType.Blue: Stat.DecreaseHealth(damageAmount); break;
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
        if (Stat.GetHealth() == 0)
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
            animator.SetTrigger("Dead");
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

    private void OnDrawGizmos()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + Vector2.down * AirRayLength);
    }

    private void FixedUpdate()
    {
        CheckOnGround();
        CheckOnAir();
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
            Debug.Log("jump");
            Jump();
        }

        if (Input.GetKeyDown(KeyManager.Inst.MeleeAttack))
        {
            base.SetState(CharacterState.MeleeAttack);
            MeleeAttack();
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

    private void CheckOnAir()
    {
        /*Vector2 origin = (Vector2)transform.position + Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + AirRayLength);
        Vector2 direction = Vector2.down;

        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, AirRayLength);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Ground"))
            {
                bIsGrounded = true;
                animator.SetBool("IsInAir", false);
                return;
            }
        }
        bIsGrounded = false;
        animator.SetBool("IsInAir", true);*/





        Vector2 origin = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);
        Vector2 direction = Vector2.down;

        // 레이캐스트를 사용하여 "NothingLayer"를 제외한 모든 레이어 체크
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, RayLength, BackGroundLayerMask);
        //Debug.Log(hitInfo.collider);
        if (hitInfo.collider != null)
        {
            // 충돌한 콜라이더의 태그가 "Ground"인지 확인
            if (hitInfo.collider.CompareTag("Ground"))
            {
                bIsGrounded = true;
                animator.SetBool("IsInAir", false);
            }
            else
            {
                bIsGrounded = false;
                animator.SetBool("IsInAir", true);
            }
        }
        else
        {
            bIsGrounded = false;
            animator.SetBool("IsInAir", true);
        }
    }

    private void CheckOnGround()
    {
        /*Vector2 origin = (Vector2)transform.position + Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + RayLength);
        Vector2 direction = Vector2.down;
        float distance = RayLength;

        // 레이캐스트를 사용하여 바닥 체크
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, distance,~LayerMask.GetMask("Background"));
        Debug.Log(hitInfo);
        if (hitInfo.collider != null)
        {
            Debug.Log("HIT!");
            if (hitInfo.collider.CompareTag("Ground"))
            {
                Debug.Log("ground true");
                bIsGrounded = true;
                return;
            }
        }
        bIsGrounded = false;*/
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y - 0.1f);
        Vector2 direction = Vector2.down;

        // 레이캐스트를 사용하여 "NothingLayer"를 제외한 모든 레이어 체크
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, RayLength, BackGroundLayerMask);
        if (hitInfo.collider != null)
        {
            // 충돌한 콜라이더의 태그가 "Ground"인지 확인
            if (hitInfo.collider.CompareTag("Ground"))
            {
                bIsGrounded = true;
                // Debug.Log("Grounded on: " + hitInfo.collider.name);
            }
            else
            {
                bIsGrounded = false;
                // Debug.Log("Not grounded, hit: " + hitInfo.collider.name);
            }
        }
        else
        {
            bIsGrounded = false;
            // Debug.Log("Not grounded");
        }
    }

    protected override void Idle()
    {
        base.SetState(CharacterState.Idle);
    }

    public Vector2 GetDirection()
    {
        if (bIsFlipped)
        {
            return Vector2.left; 
        }
        return Vector2.right;
    }

    public void MoveWithMultiplier(float Force) 
    {
        if (Input.GetKeyDown(KeyManager.Inst.QSkill)) 
        {
            Vector2 direction = GetDirection();
            Vector2 force = direction * Force;
            CharacterRigidbody.AddForce(force, ForceMode2D.Impulse);
           
        }
    }

    protected override void Move(float multiplier = 1.0f)
    {
        base.SetState(CharacterState.Move);

        int horizontalInput = KeyManager.Inst.GetAxisRawHorizontal();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // 입력 값에 따라 스프라이트를 좌우로 뒤집음
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            bIsFlipped = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            bIsFlipped = false;
        }

        Vector2 v = new Vector2(horizontalInput * Stat.GetMoveSpeed() * multiplier, CharacterRigidbody.velocity.y);
        transform.Translate(v * Time.deltaTime);
    }


    protected override void Jump() 
    {
        if (bIsGrounded)
        {
            
            CharacterRigidbody.velocity = new Vector2(CharacterRigidbody.velocity.x, JumpForce);
            animator.SetTrigger("IsJump");
        }
    }

    public void EndJump()
    {
        if (false == bIsGrounded)
        {
            animator.SetTrigger("IsJump");
        }
    }

    public void MeleeAttack() 
    {
        animator.SetTrigger("attackTrigger");
        SpiralWhipWeapon.bIsAttackActive = true;
        GameObject effect = Instantiate(hitEffect, SpiralWhipWeapon.transform.position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    protected void SetWhipAngle(float angle)
    {
        if (bIsFlipped)
        {
            SpiralWhipWeapon.SetWhipAngle(180.0f-angle);
        }
        else 
        {
            SpiralWhipWeapon.SetWhipAngle (angle);
        }
    }

    public void EndMeleeAttack()
    {
        SpiralWhipWeapon.bIsAttackActive = false;
    }

    public void StartESkillAnimation() 
    {
        animator.SetTrigger("WSkillTrigger");
    }
}
