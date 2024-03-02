using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController charCont;
    private Vector3 direction;
    private const float playerSpeed = 5f;
    private const float gravity = -0.1f;
    private const float sprintBoost = 1.5f;
    public float downVelocity = 0f;
    private const float jumpVelocity = 0.025f;
    private const float terminalVelocity = -0.05f;

    // Unity Methods
    void Awake() {
        charCont = GetComponent<CharacterController>();
    }

    void Update() {
        direction = MoveDirection();
        downVelocity = ApplyGravity(charCont, downVelocity, Time.deltaTime);
        Jump();
    }
    void FixedUpdate() {
        charCont.Move(direction * playerSpeed * Time.fixedDeltaTime * Sprinting());
    }

    // Helper Methods
    
    /// <summary>
    /// Checks if the player is grounded.
    /// This should be lazily evaluated, don't call it when not needed as it
    /// calls a raycast.
    /// </summary>
    /// <returns>Whether the player is close enough to the ground to jump</returns>
    private bool isGrounded() {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), (charCont.height / 2) + 0.2f);
    }
    private Vector3 MoveDirection() {
        return transform.TransformDirection(
            new(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"))
            );
    }
    private float Sprinting() {
        return (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) ? sprintBoost : 1f;
    }

    private void Jump() {
        if (Input.GetKey(KeyCode.Space)) {
            if (!isGrounded()) {return;}
            downVelocity = jumpVelocity;
            charCont.Move(new(0, downVelocity, 0));
        }
    }

    /// <summary>
    /// Applys gravity to a characterController.
    /// </summary>
    /// <returns>Returns the new velocity of the characterController</returns>
    private static float ApplyGravity(CharacterController charCont, float velocity, float time) {
        if (charCont.isGrounded) {return 0f;}
        float updatedVelocity = 
            (velocity + gravity * time) < terminalVelocity ? //Less than because negative
            velocity : 
            velocity + gravity * time;
        Vector3 moveDown = new(0, updatedVelocity, 0);
        charCont.Move(moveDown);
        return moveDown.magnitude * ((updatedVelocity < 0f) ? -1f : 1f);
    }
}
