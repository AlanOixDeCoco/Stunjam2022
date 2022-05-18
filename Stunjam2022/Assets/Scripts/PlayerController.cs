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
    private readonly float DASH_STUN = 1;
    private readonly float WIPER_STUN = 2;
    private readonly float MAX_DASH_COOLDOWN = 1.2f;
    private readonly float DASH_POWER = 30;
    private readonly float BOUNCE_POWER = 1000;

    private readonly float[] drag = { 10, 10, 0.5f, 10, 0 };
    private float dashCooldown = 0f;
    private float stunTime = 0f;
    public states state;
    public Animator playerAnimator;
    public CapsuleCollider2D hitbox;

    private void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = states.IDLE;
    }

    private void FixedUpdate() {
        playerAnimator.SetInteger("State", (int)state);

        if (CanMove(state) && playerMovement.magnitude == 0.0)
        {
            state = states.IDLE;
        }

        if (state != states.STUN && state != states.JUMP)
        {
            if (state != states.DASH)
            {
                playerRb.AddForce(moveForce * Time.fixedDeltaTime * playerMovement);
            }
            if (Mathf.Abs(playerRb.velocity.x) > 0.001 && Mathf.Abs(playerRb.velocity.y) > 0.001)
            {
                float angle = Mathf.Acos(playerRb.velocity.normalized.y) * -Mathf.Sign(playerRb.velocity.x);
                playerRb.MoveRotation(angle * Mathf.Rad2Deg);
                playerRb.angularVelocity = 0;
            }
        }

        if (dashCooldown > 0)
        {
            dashCooldown -= Time.fixedDeltaTime;
            if (dashCooldown <= 0)
            {
                OnDashReloaded();
            }
        }
        
        if (state == states.STUN)
        {
            stunTime -= Time.fixedDeltaTime;
            if (stunTime <= 0)
            {
                OnStunFinished();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
        if (CanMove(state))
        {
            state = states.WALK;
        }
    }
    public void OnDash(InputAction.CallbackContext context){
        if (dashCooldown > 0)
        {
            return;
        }

        if(context.phase == InputActionPhase.Started && CanMove(state))
        {
            dashCooldown = MAX_DASH_COOLDOWN;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);

            playerRb.velocity = Vector2.zero;
            if (playerMovement.normalized.magnitude == 0f)
            {
                float rad = Mathf.Deg2Rad * playerRb.rotation;
                playerRb.AddForce(new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * dashForce);
            } else
            {
                playerRb.AddForce(playerMovement.normalized * dashForce);
            }
            playerRb.drag = drag[(int)state];
            state = states.DASH;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && state != states.JUMP && state != states.STUN)
        {
            playerRb.velocity = Vector2.zero;
            hitbox.enabled = false;
            state = states.JUMP;
            spriteRenderer.sortingLayerName = "FlyJump";
        }
    }

    public bool CanMove(states p_state)
    {
        return p_state == states.IDLE || p_state == states.WALK;
    }

    public void SetIdleWalk()
    {
        if (playerMovement.magnitude == 0.0)
        {
            state = states.IDLE;
        }
        else
        {
            state = states.WALK;
        }
    }

    public void OnDashFinished()
    {
        state = states.IDLE;
        playerRb.drag = drag[(int)state];
        SetIdleWalk();
    }

    public void OnJumpFinished()
    {
        SetIdleWalk();
    }
    public void OnJumpVulnerable()
    {
        hitbox.enabled = true;
        spriteRenderer.sortingLayerName = "FlyGrounded";
    }

    public void OnStun(Vector2 direction, float duration, bool replace = false)
    {
        state = states.STUN;
        playerRb.drag = drag[(int)state];
        playerRb.AddForce(direction);
        if (stunTime <= 0 || replace)
        {
            stunTime = duration;
        }
    }

    public void OnStunFinished()
    {
        stunTime = 0;
        state = states.IDLE;
        playerRb.drag = drag[(int)state];
    }

    public void OnDashReloaded()
    {
        spriteRenderer.color = new Color(1, 1, 1);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D entity = collision.collider;
        if (entity != null)
        {
            if (entity.name == "PlayerSprite")
            {
                PlayerController otherPlayer = entity.GetComponentInParent<PlayerController>();
                if ((state == states.DASH || state == states.STUN) && otherPlayer.state != states.DASH)
                {
                    otherPlayer.OnStun(playerRb.velocity * DASH_POWER, DASH_STUN);
                }
            }
            else if (entity.name == "WiperSprite")
            {
                Rigidbody2D wiperRb = entity.GetComponent<Rigidbody2D>();
                OnStun(wiperRb.velocity * DASH_POWER, WIPER_STUN, true);
            }

        }

        if (state == states.STUN)
        {
            playerRb.AddForce(collision.contacts[0].normal * BOUNCE_POWER);
        }
    }

}
