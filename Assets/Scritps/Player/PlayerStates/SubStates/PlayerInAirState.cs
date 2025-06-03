using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool isGrounded;
    private WallChecks isTouchingWall;

    private bool coyoteTime;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "InAirState";
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckGround();

        isTouchingWall = player.WallCheck();
    }

    public override void Enter()
    {
        base.Enter();
        player.RestoreGravity();
        player.InputHandler.JumpCancleEvent += JumpCanceled;
        DoChecks();
        coyoteTime = false;
        
        

    }

    public override void Exit()
    {
        base.Exit();
        player.InputHandler.JumpCancleEvent -= JumpCanceled;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState)
        {
            return;
        }
        if (dashInput)
        {
            isExitingState = true;
            stateMachine.ChangeState(player.DashState);
        }else if (jumpInput)
        {
            if (!isTouchingWall.IsTouching())
            {
                if (coyoteTime) CheckCoyteTime();
                stateMachine.ChangeState(player.JumpState);
            } else
            {
                stateMachine.ChangeState(player.WallJumpState);
            }
        } else if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }else if (isTouchingWall.IsTouching())
        {
            int sign = player.transform.rotation.y != 0 ? -1 : 1;
            if (sign * input.x * isTouchingWall.GetDirection() > 0)
            {
                stateMachine.ChangeState(player.OnWallState);

            }
        }
    }

    private void CheckCoyteTime()
    {
        coyoteTime = Time.time > startTime + playerData.coyoteTime;
        if (Time.time > startTime + playerData.coyoteTime && coyoteTime)
        {
            player.JumpState.UseJump();
            coyoteTime = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        AirMove();
        LockFallSpeed();

    }

    private void LockFallSpeed()
    {
        if (player.CurrentVelocity.y < -30f)
        {
            player.SetVelocity(new Vector2(player.CurrentVelocity.x, -playerData.maxFallSpeed));
        }
    }

    private void AirMove()
    {
        if (player.CurrentVelocity.y < 0.0f)
        {
            player.SetGravityMult(playerData.fallGravityMult);
        }
        float targetSpeed = input.x * playerData.maxSpeed + player.GroundVelocity.x; // need to add ground velocity
        float speedDif = targetSpeed - player.CurrentVelocity.x;

        //if no input we decel
        float accelRate = (MathF.Abs(targetSpeed) > 0.01f) ? playerData.airAcceleration : playerData.airDeceleration;
        float movement = speedDif * accelRate;
        player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void JumpCanceled()
    {
        if (player.CurrentVelocity.y < 0.0f)
        {
            return;
        } 
        player.JumpCancel();
        //player.RB.gravityScale = player.PlayerGravityScale * (playerData.fallGravityMult+1) ;
    }
}
