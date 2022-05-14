using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float moveForce;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    private Rigidbody2D playerRb;
    private Vector2 playerMovement;

    private void Start() {
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
