using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int JumpLeft { get; private set; }

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "JumpState";
        RestoreJump();
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = true;
        player.InputHandler.UseJumpInput();
        if (!CanJump())
        {
            return;
        }

        player.RB.gravityScale = player.PlayerGravityScale;
        Vector2 initialJumpVelocity = Vector2.up * (playerData.jumpHeight -
            1 / 2 * player.RB.gravityScale * playerData.jumpTime* playerData.jumpTime)/playerData.jumpTime;
        initialJumpVelocity.y -= player.RB.linearVelocity.y; // for now
        
        
        player.RB.AddForce(initialJumpVelocity, ForceMode2D.Impulse);
        UseJump();
        player.StartEmitDustRunning();
        player.StopEmitDustRunning();
    }

    public bool CanJump()
    {
        return JumpLeft != 0;
    }
    
    public void RestoreJump()
    {
        JumpLeft = playerData.jumpNumber;
    }

    public void UseJump()
    {
        --JumpLeft;
        if (JumpLeft < 0) JumpLeft = 0;
    }
}
