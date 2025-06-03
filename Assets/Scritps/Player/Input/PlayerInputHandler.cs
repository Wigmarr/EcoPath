using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* 
 * Handle Different input from Input System
 */

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }
    public bool JumpInput { get; private set; }
    public event Action JumpCancleEvent;
    public bool DashInput { get; private set; }

    [SerializeField] private float JumpHold = 0.2f;
    [SerializeField] private float DashHold = 0.2f;
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
            StartCoroutine(JumpInputTimer());
        } else if (context.canceled)
        {
            JumpCancleEvent?.Invoke();
        }
    }
    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashInput = true;
            StartCoroutine(DashInputTimer());
        }
    }

    IEnumerator JumpInputTimer()
    {
        yield return new WaitForSeconds(JumpHold);
        JumpInput = false;
    }

    IEnumerator DashInputTimer()
    {
        yield return new WaitForSeconds(DashHold);
        DashInput = false;
    }
}
