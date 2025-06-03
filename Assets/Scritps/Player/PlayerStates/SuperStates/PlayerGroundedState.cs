using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerData;

public class PlayerGroundedState : PlayerState
{

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "GroundedState";
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.RestoreGravity();
        player.JumpState.RestoreJump();
        player.DashState.RestoreDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Mathf.Abs(player.CurrentVelocity.x) < playerData.minSpeed)
        {
            player.SetVelocity(new Vector2(0, player.CurrentVelocity.y));
        }
        if (isExitingState)
        {
            return;
        }
        if (dashInput)
        {
            isExitingState = true;
            stateMachine.ChangeState(player.DashState);
        } else if (jumpInput)
        {
            stateMachine.ChangeState(player.JumpState);
        } else if (!player.CheckGround())
        {
            stateMachine.ChangeState(player.InAirState);
            player.InAirState.StartCoyoteTime();
        }
       

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        float targetSpeed = input.x * playerData.maxSpeed + player.GroundVelocity.x; // need to add ground velocity
        float speedDif = targetSpeed - player.CurrentVelocity.x;

        //if no input we decel
        float accelRate = (MathF.Abs(targetSpeed) > 0.01f) ? playerData.acceleration : playerData.deceleration;
        float movement = speedDif * accelRate;
        player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
