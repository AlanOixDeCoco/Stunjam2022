using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float moveForce;
    [SerializeField] private float moveSpeed;

    [Header("Drag (higher is less sloppy)")]
    [Range(.1f, .2f)]
    [SerializeField] private float normalDrag;
    [Range(.05f, .2f)]
    [SerializeField] private float sloppyDrag;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    private Rigidbody2D playerRb;
    
    private Vector2 playerMovement;
    [SerializeField] private bool isSloppy;

    private void Awake() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        playerRb.AddForce(playerMovement * moveForce * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }
    public void OnDash(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Started)
            playerRb.AddForce(playerMovement.normalized * dashForce);
    }
}
