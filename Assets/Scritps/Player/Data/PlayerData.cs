using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float acceleration = 1.0f;
    public float deceleration = 1.0f;
    public float maxSpeed = 10.0f;
    public float startSpeed = 3.0f;
    public float minSpeed = 0.1f;
    
    [Header("Jump Settings")]
    public float airAcceleration = 0.3f;
    public float airDeceleration = 0.3f;
    public float maxFallSpeed = 15.0f;
    public float jumpHeight = 1.0f;
    public float jumpTime = 1.0f;
    public float jumpCut = 0.7f;
    public int jumpNumber = 1;
    public float coyoteTime = 0.05f;
    
    [Header("Player Gravity")]
    public float fallGravityMult = 2f;

    [Header("Wall Settings")]
    public float wallSpeed = 0.5f;
    public float wallJumpHeight = 5f;
    public float wallJumpDistance = 5f;

    [Header("DashSettings")]
    public float dashSpeed = 5f;
    public float dashDistance = 2f;
    public float dashTime = 0.5f;
    public int dashNumber = 1;
    public float dashCoolDown = 0.5f;
    public float drag = 10f;
    public float afterImagesTimer = 0.2f;
}
