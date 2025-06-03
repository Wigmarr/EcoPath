using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWallState : PlayerState
{
    private bool isGrounded;
    private WallChecks isTouchingWall;
    public PlayerOnWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "OnWallState";
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
        base.DoChecks();
        player.StartEmitWallSlide();
    }

    public override void Exit()
    {
        base.Exit();

        player.StopEmitWallSlide();
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
        }
        else if (jumpInput)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isGrounded)
        {
            stateMachine.ChangeState(player.IdleState);
        } else if (isTouchingWall.IsTouching()) // 
        {
            //rotation can be -180 or 0. input x can be -1, 0 , 1. if (player.transform.rotation.y + 1) * input.x <= 0, then it means our player not pressing movement keys or pressing it in opposite of the wall direction
            int sign = player.transform.rotation.y != 0 ? -1 : 1;
            if (sign * input.x * isTouchingWall.GetDirection() <= 0)
            {
                stateMachine.ChangeState(player.InAirState);
            }
        } else if (!isTouchingWall.IsTouching())
        {
            stateMachine.ChangeState(player.InAirState);
        } 
            
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.CurrentVelocity.y < 0.01f)
        {
            player.SetGravityMult(0);
            player.SetVelocity(Vector2.down * playerData.wallSpeed);
        }

    }
}
