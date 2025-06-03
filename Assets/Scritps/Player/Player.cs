using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using static PlayerData;


public class Player : MonoBehaviour
{

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerOnWallState OnWallState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    public PlayerDeadState DeadState { get; private set; }
    #endregion

    

    #region Animation hashes
    //Animation hashes
    private int IdleHash = Animator.StringToHash("Idle");
    private int MoveHash = Animator.StringToHash("Run");
    private int DashHash = Animator.StringToHash("Dash");
    private int JumpHash = Animator.StringToHash("Jump");
    private int DeathHash = Animator.StringToHash("Death");
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public bool IsFacingRight { get; private set; }
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Collider2D frontWallCheck;
    [SerializeField] private Collider2D backWallCheck;

    [Header("Effects")]
    [SerializeField] private ParticleSystem dustRunParticles;
    [SerializeField] private ParticleSystem dustLandParticles;
    [SerializeField] private ParticleSystem wallSlideParticles;
    [SerializeField] private AfterImage afterImage;

    [SerializeField] private CinemachineImpulseSource cameraImpulseSource;
    #endregion
    public bool IsDead { get; private set; }    
    public Vector2 WallVelocity { get; private set; }   

    public Vector2 CurrentVelocity { get; private set; }
    public Vector2 GroundVelocity { get; private set; }
    public float PlayerGravityScale { get; private set; }

    private void Awake()
    {
        IsDead = false;
        IsFacingRight = true;
        InputHandler = GetComponent<PlayerInputHandler>();
        StateMachine = new PlayerStateMachine(); //create state machine
        //init states
        IdleState = new PlayerIdleState(this, StateMachine, playerData, IdleHash);
        MoveState = new PlayerMoveState(this, StateMachine, playerData, MoveHash);
        JumpState = new PlayerJumpState(this, StateMachine, playerData, JumpHash);
        InAirState = new PlayerInAirState(this, StateMachine, playerData, JumpHash);
        LandState = new PlayerLandState(this, StateMachine, playerData, IdleHash);
        OnWallState = new PlayerOnWallState(this, StateMachine, playerData, JumpHash); // need to change to wall hang animation
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, JumpHash);
        DashState = new PlayerDashState(this, StateMachine, playerData, JumpHash); //need to change to dash animation;
        DeadState = new PlayerDeadState(this, StateMachine, playerData, DeathHash);
        
    }

    private void Start()
    {
        //init components
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        //setup gravity
        float gravity = Physics2D.gravity.y;
        float myGravity = -2 * playerData.jumpHeight / (playerData.jumpTime * playerData.jumpTime);
        PlayerGravityScale = myGravity / gravity;
        RB.gravityScale = PlayerGravityScale;

        //init stateMachine with idleState
        StateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        CurrentVelocity = RB.linearVelocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        Flip();
    }

    public void SetVelocity(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
        CurrentVelocity = velocity;
    }
    #region Checks
    
    void Flip()
    {
        if (InputHandler.MovementInput.x > 0.0f && !IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = true;
        }
        else if (InputHandler.MovementInput.x < 0.0f && IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = false;
        }
    }

    public bool CheckGround()
    {
        Collider2D[] collisions = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask);
        bool grounded = collisions.Length > 0;
        GroundVelocity = Vector2.zero;
        foreach (Collider2D collision in collisions)
        {
            var collisionRB = collision.GetComponent<Rigidbody2D>();
            if (collisionRB != null)
            {
                GroundVelocity = collisionRB.linearVelocity;
            }
        }
        return grounded;
    }
    
    public WallChecks WallCheck()
    {
        WallChecks result = new WallChecks();
        Collider2D[] frontCollisions = Physics2D.OverlapAreaAll(frontWallCheck.bounds.min, frontWallCheck.bounds.max, groundMask);
        Collider2D[] backCollisions = Physics2D.OverlapAreaAll(backWallCheck.bounds.min, backWallCheck.bounds.max, groundMask);

        result.frontWallCheck = frontCollisions.Length > 0;
        result.backWallCheck = backCollisions.Length > 0;
        WallVelocity = Vector2.zero;
        foreach (Collider2D collision in frontCollisions)
        {
            var collisionRB = collision.GetComponent<Rigidbody2D>();
            if (collisionRB != null)
            {
                WallVelocity = collisionRB.linearVelocity;
            }
        }
        return result;
    }
    #endregion
    #region Player velocity control
    
    public void RestoreGravity()
    {
        RB.gravityScale = PlayerGravityScale;
    }

    public void SetGravityMult(float mult)
    {
        RB.gravityScale = PlayerGravityScale * mult;
    }

    public void JumpCancel()
    {
        RB.linearVelocity = new Vector2(CurrentVelocity.x, CurrentVelocity.y * playerData.jumpCut);
    }
    #endregion
    #region Particles

    //DUST Run Control
    public void StartEmitDustRunning() => dustRunParticles.Play();

    public void StopEmitDustRunning() => dustRunParticles.Stop();

    //Dust Land Control
    public void StartDustLand() => dustLandParticles.Play();

    public void StartEmitWallSlide() => wallSlideParticles.Play();
    public void StopEmitWallSlide() => wallSlideParticles.Stop();
    #endregion
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obsticle")
        {
            IsDead = true;
            StateMachine.ChangeState(DeadState);
            
        }
    }
    
    public void ShakeCamera()
    {
        cameraImpulseSource.GenerateImpulse();
    }

    public void StartAfterImage() => afterImage.Activate(true);
    public void StopAfterImage() => afterImage.Activate(false);

}
public class WallChecks
{
    public bool frontWallCheck = false;
    public bool backWallCheck = false;
    public bool IsTouching()
    {
        return frontWallCheck || backWallCheck;
    }

    public int GetDirection()
    {
        if (backWallCheck)
        {
            return -1;
        }
        if (frontWallCheck)
        {
            return 1;
        }
        return 0;
    }
    
}
