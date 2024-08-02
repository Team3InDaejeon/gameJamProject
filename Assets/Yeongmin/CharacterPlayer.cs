using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterPlayer : CharacterBase
{
    [Header("")]
    [Tooltip("")]
    public float MouseSensitivity = 2f;
    public GameObject Camera;
    [Tooltip("")]
    public float CameraRotSpeed = 200f;
    protected float CameraRotX = 0f;
    protected float CameraRotY;

    [Tooltip("")] 
    public float DefaultGravity = 1.0f;
    private float Gravity = 1.0f;
    private bool bIsGrounded = false;

    private CharacterController characterController;
    float SlowValue;

    private Rigidbody2D CharacterRigidbody;
    private Animator animator;
    float JumpForce = 10.0f;

    void Awake() 
    {
        // characterController = this.GetComponent<CharacterController>();
        base.Awake();
        CharacterRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        
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

        // if (groundCheckTimer > 0.0f && !bIsGrounded)
        // {
        //     groundCheckTimer -= Time.fixedDeltaTime;
        // }
       // else if (!bIsGrounded)
       // {
       //     // groundCheckTimer = GROUND_CHECK_TIME;
       //     bIsGrounded = true;
       //     Gravity = defaultGravity;
       // }
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
