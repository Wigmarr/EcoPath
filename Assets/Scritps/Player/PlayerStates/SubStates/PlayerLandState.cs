using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "LandState";
    }

    public override void Enter()
    {
        base.Enter();
        player.StartDustLand();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState)
        {
            return;
        }
        if (Mathf.Abs(player.CurrentVelocity.x) > 0 )
        {
            stateMachine.ChangeState(player.MoveState);
        } else
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
    }
}
