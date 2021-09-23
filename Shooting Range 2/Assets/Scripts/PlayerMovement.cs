using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float moveSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    float directionY;

    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float x;
    float z;

    bool isGrounded;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = speed * 1.5f;
        else
            moveSpeed = speed;

        /*
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = controller.isGrounded;



        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump"))
            {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        */

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);



        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                directionY = jumpHeight;
            }
        }



        Vector3 move = transform.right * direction.x + transform.forward * direction.z;
        

        directionY -= gravity * Time.deltaTime;

        if (controller.isGrounded && directionY < 0)
        {
            directionY = -1f;
        }

        move.y = directionY;

        controller.Move(move * moveSpeed * Time.deltaTime);



    }
}
