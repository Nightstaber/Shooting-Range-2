using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    
    CharacterController controller;

    // Movement
    float speed = 12f;
    float moveSpeed;

    //Jumping
    float gravity = 9.81f;
    float jumpHeight = 3f;
    float directionY = 0f;


    private void Start()
    {
        // Set controller to CharacterController
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sprint if left-shift is held down.
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = speed * 1.5f;
        else
            moveSpeed = speed;

        // Get keyboard input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Set direction based on keyboard input
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);


        // If player is grounded, and Jump button is pressed, set directionY to jumpheight.
        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                directionY = jumpHeight;
            }
        }


        // Set a Vector3 to include camera rotation in the movement.
        Vector3 move = transform.right * direction.x + transform.forward * direction.z;
        
        // Apply gravity
        directionY -= gravity * Time.deltaTime;

        // If grounded, stop gravity from applying.
        if (controller.isGrounded && directionY < 0)
        {
            directionY = -1f;
        }

        // Set the last direction of vector3 move.
        move.y = directionY;

        // Apply movement to character controller.
        controller.Move(move * moveSpeed * Time.deltaTime);

        


    }
}
