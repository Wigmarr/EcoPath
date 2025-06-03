using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private Vector3 playerStartPosition;
    private int dashesLeft = 1;
    private Vector2 dashDirection;
    private float lastDashedTime;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash) : base(player, stateMachine, playerData, animHash)
    {
        StateName = "DashState";
    }

    public override void Enter()
    {
        base.Enter();
        if (dashesLeft == 0)
        {
            isAbilityDone = true;
            return;
        }
        dashDirection = Vector2.zero;
        player.InputHandler.UseDashInput();
        if (!CanDash())
        {
            isAbilityDone = true;
            return;
        }
        UseDash();
        player.ShakeCamera();

        //playerStartPosition = player.transform.position;
        int sign = player.IsFacingRight ? 1 : -1;
        if (input.x == 0 && input.y == 0)
        {
           dashDirection = new Vector2(sign , 0f);
        }
        else if (input.x == 0 && input.y != 0)
        {
            dashDirection = new Vector2(0f, input.y );
        }
        else
        {
            dashDirection = new Vector2(input.x, input.y);
        }
        player.RB.linearDamping = playerData.drag;
        player.StartAfterImage();
        //player.StartTrail();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        base.DoChecks();
        if (isAbilityDone) { return; }
        if (Time.time >= startTime + playerData.dashTime)
        {
            player.RB.linearDamping = 0;
            lastDashedTime = Time.time;
            isAbilityDone = true;
            player.SetVelocity(player.CurrentVelocity.normalized * 5f);
            player.JumpCancel();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isAbilityDone) { return; }
        player.SetVelocity(dashDirection * playerData.dashSpeed);
    }
    private void UseDash() => dashesLeft -= 1;
    public void RestoreDash() => dashesLeft = playerData.dashNumber;

    public bool CanDash()
    {
        if (dashesLeft > 0 && Time.time >= lastDashedTime + playerData.dashCoolDown)
        {
            return true;
        }
        return false;
    }

    public override void Exit()
    {
        base.Exit();
        //player.StopTrail();
        player.StopAfterImage();
    }
}
