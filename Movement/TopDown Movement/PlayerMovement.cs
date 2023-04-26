using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInputManager;

    private InputAction move;
    private InputAction fire;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Vector2 directions;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float maxSpeed;

    [Header("Physics")]
    [SerializeField]
    private float linearDrag = 4f;

    private void OnValidate() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake() {
        playerInputManager = new PlayerInput();
    }

    private void OnEnable() {
        move = playerInputManager.Player.Move;
        move.Enable();
        fire = playerInputManager.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
    
    }

    private void Start() {
        
    }

    private void Update() {
        directions = move.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        moveCharacter(directions.x, directions.y);
     }

    void moveCharacter(float horizontal, float vertical){
        Vector2 normalized = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        rb.velocity = normalized;
    }
    
    private void Fire(InputAction.CallbackContext context){
        GetComponent<PlayerMeleeAttack>().fire();
    }

    
}
