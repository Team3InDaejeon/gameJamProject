using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Unity.VisualScripting;
using System;
public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Death
}
public enum EnemyType
{
    Red,
    Normal,
    Blue
}
public class EnemyAI : CharacterBase,ICombat
{
    [Header("Stat Setting")]
    [SerializeField]
    protected float searchRange = 5;
    [SerializeField]
    protected float attackRange = 2;
    [SerializeField]
    protected float cooltime = 2;
    [SerializeField]
    protected float jumpPower = 1;
    [SerializeField]
    protected float moveSpeed=3;
    public EnemyType enemyType=EnemyType.Red;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer; // ?�격 ?�과�??�한 ?�프?�이???�더??

    [Header("Effect Setting")]
    public GameObject hitEffect;
    public GameObject takeDamageEffect;


    //AI Setting
    public float timeForMoving = 10;

    [Header("Test Setting")]
    public float rayLength=1.2f;

    StateMachine<EnemyState> fsm;
    public Rigidbody2D rb;
    Transform target;
    Vector2 randomPoint;
    Animator animator;
    float currentTimeForMoving=0;
    float currentAttackTime=0;
    bool isGrounded=true;
    float speedMultiplier=1;

    //MonoBehaviour Functions
    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        boxCollider=GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (Stat!=null){
            moveSpeed=Stat.GetMoveSpeed();
            ScriptableEnemy enemyInfo=Stat.GetCharacterInfo();
            Stat.SetMaxHealth(enemyInfo.Health);
            Stat.SetHealth(enemyInfo.Health);
            enemyType=enemyInfo.Type;
            Debug.Log($"Enemy Type : {enemyType}");
        }
        Initialize();
    }
    protected virtual void Update(){
        fsm.Driver.Update.Invoke();
    }
    void FixedUpdate()
    {
        // 바운??박스??바닥 부분의 중심 좌표 계산
        Vector2 origin = boxCollider.bounds.center;
        origin.y = boxCollider.bounds.min.y;

        // Raycast 발사
        isGrounded = Physics2D.Raycast(origin, Vector2.down, rayLength, LayerMask.GetMask("Platform"));

        // ?�버�?Ray 그리�?
        Debug.DrawRay(origin, Vector2.down * rayLength, Color.red);
        
    }
    void Initialize()
    {
        fsm.ChangeState(EnemyState.Idle);
    }
    //State Functions
    void Idle_Enter()
    {
        StartCoroutine(FindTarget(searchRange, 1, EnemyState.Walk));
        //TODO : Enemy Idle Animation Start
    }
    void Idle_Update()
    {
        Idle();
    }
    
    void Walk_Enter()
    {
        StartCoroutine(FindTarget(attackRange, .1f, EnemyState.Attack));
        //TODO : Enemy Walk Animation Start
    }
    void Walk_Update(){
        if(target==null){
            fsm.ChangeState(EnemyState.Idle);
        }
        if(Vector2.Distance(transform.position, target.position) > searchRange + 1f){
            fsm.ChangeState(EnemyState.Idle);
            target=null;
        }else{
            Move(speedMultiplier);
        }
    }
    void Attack_Enter()
    {
        if(animator!=null)
            animator.SetFloat("walkSpeed", 0);
        if(target!=null){
            Flip(target.position);
        }
        //TODO : Enemy Attack Animation Start
    }
    void Attack_Update(){
        if(currentAttackTime>=cooltime){
            //TODO : Enemy Attack
            currentAttackTime=0;
            GameObject effect=Instantiate(hitEffect,target.position,Quaternion.identity);
            Destroy(effect,2f);
        }
        else if(target != null && Vector2.Distance(transform.position, target.position) > attackRange+1f)
        {
            fsm.ChangeState(EnemyState.Walk);
        }
        else{
            currentAttackTime+=Time.deltaTime;
            if (target != null)
            {
                Flip(target.position);
            }
        }
    }

    void Death_Enter()
    {
        if (animator != null)
            animator.SetTrigger("deathTrigger");
        //TODO : Enemy Death Animation Start
        Destroy(gameObject,2f);
    }

    //Utility Functions
    protected override void Idle()
    {
        if (target != null)
        {
            fsm.ChangeState(EnemyState.Walk);
        }
        else if (currentTimeForMoving >= timeForMoving)
        {
            currentTimeForMoving = 0;
            randomPoint = new Vector2(UnityEngine.Random.Range(-5, 5), transform.position.y);
        }
        else
        {
            currentTimeForMoving += Time.deltaTime;
            Move(speedMultiplier);
        }
    }
    protected override void Move(float multiplier = 1)
    {
        Vector2 targetPosition = (target != null) ? (Vector2)target.position : randomPoint;
        MoveTowardsTarget(targetPosition, multiplier);
    }

    private void MoveTowardsTarget(Vector2 target, float multiplier = 1)
    {
        Vector2 targetPosition = new Vector2(target.x, transform.position.y);
        float adjustedSpeed = moveSpeed * multiplier;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, adjustedSpeed * Time.deltaTime);

        Vector2 headDirection =(targetPosition - (Vector2)transform.position).normalized;

        if (animator != null)
            animator.SetFloat("walkSpeed", Mathf.Abs(headDirection.x));

        Flip(target);

        if (target.y > transform.position.y + 0.5f && isGrounded)
        {
            Jump();
        }
    }
    void Flip(Vector2 target){
        float deltaX = transform.position.x - target.x;
        if (deltaX > 0 )
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (deltaX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public IEnumerator FindTarget(float searchRange, float searchTime, EnemyState newState)
    {
        while (true)
        {
            Collider2D[] collidersInSightRange = Physics2D.OverlapCircleAll(transform.position, searchRange - .1f, LayerMask.GetMask("Player"));
            if (collidersInSightRange.Length > 0)
            {
                target = collidersInSightRange[0].transform;
                fsm.ChangeState(newState);
                yield break;
            }

            yield return new WaitForSeconds(searchTime);
        }
    }

    public virtual void TakeDamage(int damage, EnemyType enemyType = EnemyType.Normal)
    {

        Stat.DecreaseHealth(damage);
        Debug.Log("Enemy Health : " + Stat.GetHealth());
        KnockBack();
        

        if (Stat.GetHealth() <= 0)
        {
            fsm.ChangeState(EnemyState.Death);
        }
        else{
            StartCoroutine(TakeDamageEffect());
        }
        
    }
    private IEnumerator TakeDamageEffect()
    {
        bool clicker=false;
        GameObject effect=Instantiate(takeDamageEffect,transform.position,Quaternion.identity);
        Destroy(effect,2f);
        if (spriteRenderer != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                if(clicker){
                    spriteRenderer.color = new Color(1,1,1,1);
                    clicker=false;
                }
                else{
                    spriteRenderer.color = new Color(1,1,1,0.5f);
                    clicker=true;
                }
                yield return new WaitForSeconds(0.35f);
                elapsedTime += 0.35f;
            }
            spriteRenderer.enabled = true;
        }

    }
    protected virtual void KnockBack(){
        if(transform.localScale.x==1){
            rb.AddForce(Vector2.left*5,ForceMode2D.Impulse);
        }else{
            rb.AddForce(Vector2.right*5,ForceMode2D.Impulse);
        }
        
    }
    protected override void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(randomPoint, 1);
    }

    protected override void SetDead()
    {
        
    }

    public Transform GetTarget(){
        return target;
    }
    public void SetTarget(Transform transform){
        target=transform;
    }
}
