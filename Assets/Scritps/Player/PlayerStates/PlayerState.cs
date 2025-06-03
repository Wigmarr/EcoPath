using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static PlayerData;

public abstract class PlayerState
{

    public string StateName { get; protected set; }
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isExitingState;

    private string animName;
    private int animHash;
    protected float startTime;

    protected Vector2 input;
    protected bool jumpInput;
    protected bool dashInput;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, int animHash)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animHash = animHash;
        this.StateName = "Undefined State";
    }

    public virtual void Enter()
    {
        Debug.Log("Entered " + StateName);
        DoChecks();
        input = player.InputHandler.MovementInput;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;

        isExitingState = false;
        startTime = Time.time;
        player.Anim.CrossFade(animHash, 0.0f);
    }

    public virtual void Exit()
    {
        isExitingState = true;
        Debug.Log("Exited " + StateName);
    }

    public virtual void LogicUpdate() 
    {
        input = player.InputHandler.MovementInput;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        
       
    }

    public virtual void PhysicsUpdate() 
    {
        DoChecks();
    }


    public virtual void DoChecks() 
    {

    }



}
