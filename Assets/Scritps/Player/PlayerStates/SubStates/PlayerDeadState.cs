using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "Dead State";
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityMult(0f);
        player.SetVelocity(Vector2.zero);

    }
}
