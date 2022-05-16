using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum states
{
    IDLE,
    WALK,
    DASH,
    JUMP,
    STUN
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float moveForce;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private Vector2 playerMovement;
    public states state;
    public Animator playerAnimator;
    public CapsuleCollider2D hitbox;

    private void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = states.IDLE;
    }

    private void FixedUpdate() {
        playerRb.AddForce(moveForce * Time.fixedDeltaTime * playerMovement);
        playerAnimator.SetInteger("State", (int)state);
        
        if (CanMove(state) && playerMovement.magnitude == 0.0)
        {
            state = states.IDLE;
        }

        if (state != states.STUN && state != states.JUMP)
        {
            float angle = Mathf.Acos(playerRb.velocity.normalized.y) * -Mathf.Sign(playerRb.velocity.x);
            playerRb.MoveRotation(angle * Mathf.Rad2Deg);
            playerRb.angularVelocity = 0;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (CanMove(state))
        {
            playerMovement = context.ReadValue<Vector2>();
            state = states.WALK;
        }
    }
    public void OnDash(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Started && CanMove(state))
        {
            playerRb.AddForce(playerMovement.normalized * dashForce);
            state = states.DASH;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && state != states.JUMP)
        {
            playerRb.velocity = Vector2.zero;
            playerMovement = Vector2.zero;

            hitbox.enabled = false;
            state = states.JUMP;
            spriteRenderer.sortingLayerName = "FlyJump";
        }
    }

    public bool CanMove(states p_state)
    {
        return p_state == states.IDLE || p_state == states.WALK;
    }

    public void OnDashFinished()
    {
        state = states.IDLE;
    }

    public void OnJumpFinished()
    {
        state = states.IDLE;
        hitbox.enabled = true;
        spriteRenderer.sortingLayerName = "FlyGrounded";
    }
}
