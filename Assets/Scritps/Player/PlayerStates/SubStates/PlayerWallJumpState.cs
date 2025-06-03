using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "WallJumpState";
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = true;
        player.InputHandler.UseJumpInput();

        player.RB.gravityScale = player.PlayerGravityScale;
        Vector2 initialJumpVelocity = Vector2.up * (playerData.wallJumpHeight -
            1 / 2 * player.RB.gravityScale * playerData.jumpTime * playerData.jumpTime) / playerData.jumpTime;
        initialJumpVelocity.y -= player.RB.linearVelocity.y; // for now
        
         int sign = player.transform.rotation.y == -1 ? 1 : -1;
        initialJumpVelocity.x = player.WallCheck().GetDirection() * sign * playerData.wallJumpDistance;

        player.RB.AddForce(initialJumpVelocity, ForceMode2D.Impulse);
        player.JumpState.UseJump();
    }
}
