using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerData;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "MoveState";
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        this.PhysicsUpdate();
        player.StartEmitDustRunning();
    }

    public override void Exit()
    {
        base.Exit();
        player.StopEmitDustRunning();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // if velocity value small we think it's zero (better state transitions)
        
        if (isExitingState)
        {
            return;
        }
        if (player.CurrentVelocity.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

       

    }


}
